using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DocConnect.Web.Hubs
{
    public class ChatHub : Hub
    {
        // Khi kết nối thành công, tự động nhận diện và đưa người dùng vào đúng phòng
        public async Task ThamGiaPhong(string phienId)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa thông qua Cookie
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                // Cho kết nối này vào nhóm riêng của Phiên tư vấn đó
                await Groups.AddToGroupAsync(Context.ConnectionId, phienId);
            }
        }

        // Không cần hàm SendMessage ở đây nữa vì ta sẽ dùng IHubContext đẩy trực tiếp từ Controller sau khi lưu DB thành công (đảm bảo tin nhắn luôn được lưu vào SQL Server trước khi hiển thị)
    }
}