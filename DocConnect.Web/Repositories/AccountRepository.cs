using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DocConnectDbContext _context;

        public AccountRepository(DocConnectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.NguoiDungs.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(NguoiDung user)
        {
            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<NguoiDung?> GetUserByEmailAsync(string email)
        {
            return await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task UpdateUserAsync(NguoiDung user)
        {
            _context.NguoiDungs.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<NguoiDung?> GetUserByIdAsync(string id)
        {
            return await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
