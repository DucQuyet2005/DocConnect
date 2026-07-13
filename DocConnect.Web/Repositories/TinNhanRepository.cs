using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories
{
    public class TinNhanRepository : ITinNhanRepository
    {
        private readonly DocConnectDbContext _context;

        public TinNhanRepository(DocConnectDbContext context)
        {
            _context = context;
        }

        public async Task<HoSoBacSi?> GetHoSoBacSiByUserIdAsync(string userId)
        {
            return await _context.HoSoBacSis.FirstOrDefaultAsync(h => h.NguoiDungId == userId);
        }

        public async Task<PhienTuVan?> GetPhienTuVanActiveAsync(string bacSiId, string benhNhanId)
        {
            return await _context.PhienTuVans
                .Include(p => p.BenhNhan)
                .Include(p => p.BacSi)
                .FirstOrDefaultAsync(p => p.BacSiId == bacSiId && p.BenhNhanId == benhNhanId && (p.TrangThai == "Active" || p.TrangThai == "ChoTraLoi"));
        }

        public async Task<PhienTuVan?> GetPhienTuVanByIdAsync(int id)
        {
            return await _context.PhienTuVans
                    .Include(p => p.BenhNhan)
                    .Include(p => p.BacSi)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddLichHenAsync(LichHen lichHen)
        {
            _context.LichHens.Add(lichHen);
            await Task.CompletedTask;
        }

        public async Task AddPhienTuVanAsync(PhienTuVan phien)
        {
            _context.PhienTuVans.Add(phien);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TinNhan>> GetTinNhansByPhienIdAsync(int phienId)
        {
            return await _context.TinNhans
                .Where(t => t.PhienTuVanId == phienId)
                .OrderBy(t => t.ThoiGianGui)
                .ToListAsync();
        }

        public async Task<List<PhienTuVan>> GetPhienTuVansByBacSiIdAsync(string bacSiId)
        {
            return await _context.PhienTuVans
                .Include(p => p.BenhNhan)
                .Where(p => p.BacSiId == bacSiId)
                .ToListAsync();
        }

        public async Task<TinNhan?> GetTinNhanCuoiAsync(int phienId)
        {
            return await _context.TinNhans
                    .Where(t => t.PhienTuVanId == phienId)
                    .OrderByDescending(t => t.ThoiGianGui)
                    .FirstOrDefaultAsync();
        }

        public async Task AddTinNhanAsync(TinNhan tinNhan)
        {
            _context.TinNhans.Add(tinNhan);
            await Task.CompletedTask;
        }
    }
}
