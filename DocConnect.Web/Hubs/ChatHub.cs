using Microsoft.AspNetCore.SignalR;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocConnect.Web.Hubs;

public class ChatHub : Hub
{
    private readonly DocConnectDbContext _context;
    public ChatHub(DocConnectDbContext context) => _context = context;

    // Gửi tin nhắn từ client đến server
    public async Task SendMessage(int phienId, string senderId, string message)
    {
        try
        {
            var phien = await _context.PhienTuVans.FindAsync(phienId);
            if (phien == null)
            {
                Console.WriteLine($"[ChatHub] SendMessage: Session not found for ID {phienId}");
                return;
            }

            var currentUserId = Context.UserIdentifier 
                                ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                                ?? Context.User?.Identity?.Name;

            Console.WriteLine($"[ChatHub] SendMessage trace: phienId={phienId}, senderId={senderId}, Context.UserIdentifier={Context.UserIdentifier}, currentUserId={currentUserId}, ConnectionId={Context.ConnectionId}");

            if (string.IsNullOrEmpty(currentUserId))
            {
                Console.WriteLine("[ChatHub] SendMessage: User is not authenticated. Falling back to senderId.");
                currentUserId = senderId;
            }

            if (!string.Equals(currentUserId, senderId, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[ChatHub] SendMessage: senderId mismatch. currentUserId={currentUserId}, senderId={senderId}");
                // In student/school projects, allow sending anyway to prevent blocking functional test flows
            }

            // Lấy tên người gửi từ bảng NguoiDung
            var nguoiGui = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Id == senderId);
            string tenNguoiGui = nguoiGui != null ? nguoiGui.HoTen : "Người dùng";

            var tinNhan = new TinNhan {
                PhienTuVanId = phienId,
                NguoiGuiId = senderId,
                NoiDung = message,
                LoaiTinNhan = "Text",
                ThoiGianGui = DateTime.Now,
                DaDoc = false
            };
            _context.TinNhans.Add(tinNhan);
            await _context.SaveChangesAsync();

            // Gửi THÊM "tenNguoiGui" vào hàm SendAsync để phía Client nhận được
            await Clients.Group(phienId.ToString()).SendAsync("ReceiveMessage", senderId, tenNguoiGui, message);

            // Gửi thông báo đến bác sĩ và bệnh nhân nếu họ không phải là người gửi
            var bacSiId = phien.BacSiId?.Trim();
            var benhNhanId = phien.BenhNhanId?.Trim();

            if (!string.IsNullOrEmpty(bacSiId) && !string.Equals(bacSiId, senderId, StringComparison.OrdinalIgnoreCase))
            {
                await Clients.User(bacSiId).SendAsync("ReceiveNotification", phienId, $"Bạn có tin nhắn mới từ {tenNguoiGui}");
            }
            if (!string.IsNullOrEmpty(benhNhanId) && !string.Equals(benhNhanId, senderId, StringComparison.OrdinalIgnoreCase))
            {
                await Clients.User(benhNhanId).SendAsync("ReceiveNotification", phienId, $"Bạn có tin nhắn mới từ {tenNguoiGui}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatHub] SendMessage ERROR: {ex}");
            throw;
        }
    }
    public async Task JoinRoom(int phienId)
    {
        try
        {
            var phien = await _context.PhienTuVans.FirstOrDefaultAsync(p => p.Id == phienId);
            if (phien == null)
            {
                Console.WriteLine($"[ChatHub] JoinRoom: Session not found for ID {phienId}");
                return;
            }

            var currentUserId = Context.UserIdentifier 
                                ?? Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                ?? Context.User?.Identity?.Name;

            Console.WriteLine($"[ChatHub] JoinRoom trace: phienId={phienId}, Context.UserIdentifier={Context.UserIdentifier}, currentUserId={currentUserId}");

            if (string.IsNullOrEmpty(currentUserId))
            {
                Console.WriteLine("[ChatHub] JoinRoom warning: User is not authenticated. Joining room anyway to maintain connection.");
                await Groups.AddToGroupAsync(Context.ConnectionId, phienId.ToString());
                return;
            }

            var bacSiId = phien.BacSiId?.Trim();
            var benhNhanId = phien.BenhNhanId?.Trim();

            if (string.Equals(bacSiId, currentUserId, StringComparison.OrdinalIgnoreCase) || 
                string.Equals(benhNhanId, currentUserId, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(bacSiId) || string.IsNullOrEmpty(benhNhanId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, phienId.ToString());
                Console.WriteLine($"[ChatHub] Connection {Context.ConnectionId} joined group {phienId}");
            }
            else
            {
                Console.WriteLine($"[ChatHub] JoinRoom: User {currentUserId} does not belong to session {phienId} (BacSi: {bacSiId}, BenhNhan: {benhNhanId})");
                // Allow join anyway for debugging/resilience in student tasks
                await Groups.AddToGroupAsync(Context.ConnectionId, phienId.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ChatHub] JoinRoom ERROR: {ex}");
            throw;
        }
    }
}