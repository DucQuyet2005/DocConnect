using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocConnect.Web.Models.Entities
{
   [Table("ChuyenKhoa")]
    public class ChuyenKhoa
    {
        public int Id { get; set; }
        public string TenChuyenKhoa { get; set; } = string.Empty;

        // Định nghĩa mối quan hệ: Một chuyên khoa có nhiều hồ sơ bác sĩ
        public virtual ICollection<HoSoBacSi> HoSoBacSis { get; set; } = new List<HoSoBacSi>();
        public virtual ICollection<HoiDap> HoiDaps { get; set; }= new List<HoiDap>();
    }
}