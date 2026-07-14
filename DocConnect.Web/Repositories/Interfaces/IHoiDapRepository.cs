using System.Collections.Generic;
using System.Threading.Tasks;
using DocConnect.Web.Models.Entities;

namespace DocConnect.Web.Repositories.Interfaces
{
    public interface IHoiDapRepository
    {
        Task<List<HoiDap>> GetTatCaHoiDapDaDuyetAsync();
        Task AddHoiDapAsync(HoiDap hoiDap);
        Task<HoiDap?> GetHoiDapByIdAsync(int id);
    }
}
