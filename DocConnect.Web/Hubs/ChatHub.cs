using Microsoft.AspNetCore.SignalR;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocConnect.Web.Hubs;

public class ChatHub : Hub
{
    private readonly DocConnectDbContext _context;
    public ChatHub(DocConnectDbContext context) => _context = context;

    public async Task SendMessage(int phienId, string senderId, string message)
    {
        var phien = await _context.PhienTuVans.FindAsync(phienId);
        if (phien == null) return;

        var currentUserId = Context.UserIdentifier;
        if (!string.Equals(currentUserId, senderId, StringComparison.OrdinalIgnoreCase)) return;

        // Lấy tên người gửi từ bảng NguoiDung
        var nguoiGui = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Id == senderId);
        string tenNguoiGui = nguoiGui != null ? nguoiGui.HoTen : "Người dùng";

        var tinNhan = new TinNhan {
            PhienTuVanId = phienId,
            NguoiGuiId = senderId,
            NoiDung = message,
            LoaiTinNhan = "Text",
            ThoiGianGui = DateTime.Now
        };
        _context.TinNhans.Add(tinNhan);
        await _context.SaveChangesAsync();

        // Gửi THÊM "tenNguoiGui" vào hàm SendAsync để phía Client nhận được
        await Clients.Group(phienId.ToString()).SendAsync("ReceiveMessage", senderId, tenNguoiGui, message);

        // Phần thông báo vẫn giữ nguyên, rất tốt!
        if (!string.Equals(phien.BacSiId, senderId, StringComparison.OrdinalIgnoreCase))
        {
            await Clients.User(phien.BacSiId).SendAsync("ReceiveNotification", phienId, $"Bạn có tin nhắn mới từ {tenNguoiGui}");
        }
        if (!string.Equals(phien.BenhNhanId, senderId, StringComparison.OrdinalIgnoreCase))
        {
            await Clients.User(phien.BenhNhanId).SendAsync("ReceiveNotification", phienId, $"Bạn có tin nhắn mới từ {tenNguoiGui}");
        }
    }
    public async Task JoinRoom(int phienId)
    {
        var phien = await _context.PhienTuVans.FirstOrDefaultAsync(p => p.Id == phienId);
        if (phien == null)
        {
            return;
        }

        var currentUserId = Context.UserIdentifier;
        if (string.IsNullOrEmpty(currentUserId))
        {
            return;
        }

        if (phien.BacSiId == currentUserId || phien.BenhNhanId == currentUserId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, phienId.ToString());
        }
    }
}