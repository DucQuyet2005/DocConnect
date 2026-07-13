using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories
{
    public class HoiDapRepository : IHoiDapRepository
    {
        private readonly DocConnectDbContext _context;

        public HoiDapRepository(DocConnectDbContext context)
        {
            _context = context;
        }

        public async Task<List<HoiDap>> GetTatCaHoiDapDaDuyetAsync()
        {
            return await _context.HoiDaps
                                 .Where(q => q.DaDuyet == true)
                                 .OrderByDescending(q => q.NgayTao)
                                 .ToListAsync();
        }

        public async Task AddHoiDapAsync(HoiDap hoiDap)
        {
            _context.HoiDaps.Add(hoiDap);
            await _context.SaveChangesAsync();
        }

        public async Task<HoiDap?> GetHoiDapByIdAsync(int id)
        {
            return await _context.HoiDaps.FindAsync(id);
        }
    }
}
