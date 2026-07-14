using System.Collections.Generic;
using System.Threading.Tasks;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories.Interfaces
{
    public interface ITinNhanRepository
    {
        Task<HoSoBacSi?> GetHoSoBacSiByUserIdAsync(string userId);
        Task<PhienTuVan?> GetPhienTuVanActiveAsync(string bacSiId, string benhNhanId);
        Task<PhienTuVan?> GetPhienTuVanByIdAsync(int id);
        Task AddLichHenAsync(LichHen lichHen);
        Task AddPhienTuVanAsync(PhienTuVan phien);
        Task SaveChangesAsync();
        Task<List<TinNhan>> GetTinNhansByPhienIdAsync(int phienId);
        Task<List<PhienTuVan>> GetPhienTuVansByBacSiIdAsync(string bacSiId);
        Task<TinNhan?> GetTinNhanCuoiAsync(int phienId);
        Task AddTinNhanAsync(TinNhan tinNhan);
    }
}
