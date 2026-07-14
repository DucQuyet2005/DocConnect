using System.Collections.Generic;
using System.Threading.Tasks;
using DocConnect.Web.Models.Entities;
using DocConnect.Web.Models.ViewModels;

namespace DocConnect.Web.Repositories.Interfaces
{
    public interface IBacSiRepository
    {
        Task<List<BacSiViewModel>> GetDanhSachBacSiAsync(int chuyenKhoaId);
        Task<List<BacSiViewModel>> GetTop5BacSiAsync();
        Task<List<ChuyenKhoa>> GetChuyenKhoasAsync();
        Task<BacSiViewModel?> GetChiTietBacSiAsync(string id);
    }
}
