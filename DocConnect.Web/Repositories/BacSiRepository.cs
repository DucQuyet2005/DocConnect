using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using DocConnect.Web.Data;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models.ViewModels;

namespace DocConnect.Web.Repositories
{
    public class BacSiRepository : IBacSiRepository
    {
        private readonly DocConnectDbContext _context;

        public BacSiRepository(DocConnectDbContext context)
        {
            _context = context;
        }

        public async Task<List<BacSiViewModel>> GetDanhSachBacSiAsync(int chuyenKhoaId)
        {
            return await _context.Database
                .SqlQueryRaw<BacSiViewModel>(
                    "EXEC GetDanhSachBacSi @ChuyenKhoaId = {0}",
                    chuyenKhoaId)
                .ToListAsync();
        }

        public async Task<List<BacSiViewModel>> GetTop5BacSiAsync()
        {
            return await _context.Database
                .SqlQueryRaw<BacSiViewModel>("EXEC GetTop5BacSi")
                .ToListAsync();
        }

        public async Task<List<ChuyenKhoa>> GetChuyenKhoasAsync()
        {
            return await _context.ChuyenKhoas.ToListAsync();
        }

        public async Task<BacSiViewModel?> GetChiTietBacSiAsync(string id)
        {
            var p_BacSiId = new SqlParameter("@BacSiId", SqlDbType.NVarChar, 50) { Value = id };
            var list = await _context.Database
                .SqlQueryRaw<BacSiViewModel>(
                    "EXEC GetChiTietBacSi @BacSiId", 
                    p_BacSiId)
                .ToListAsync();
            return list.FirstOrDefault();
        }
    }
}
