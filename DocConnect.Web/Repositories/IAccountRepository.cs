using System.Threading.Tasks;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories
{
    public interface IAccountRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task AddUserAsync(NguoiDung user);
        Task<NguoiDung?> GetUserByEmailAsync(string email);
    }
}
