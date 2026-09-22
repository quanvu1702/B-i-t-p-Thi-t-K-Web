using System.ComponentModel.DataAnnotations;

namespace Ogani_master.ViewModels
{
    public class ThemSanPhamViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập mã sản phẩm.")]
        [StringLength(25)]
        public string MaSp { get; set; } = "";

        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm.")]
        [StringLength(150)]
        public string TenSp { get; set; } = "";

        [Required(ErrorMessage = "Bạn chưa chọn loại sản phẩm.")]
        [StringLength(25)]
        public string MaLoai { get; set; } = "";

        [Required(ErrorMessage = "Bạn chưa nhập giá nhỏ nhất.")]
        [Range(typeof(decimal), "0", "1000000000000",
            ErrorMessage = "Giá phải từ 0 đến 1.000.000.000.000.")]
        public decimal? GiaNhoNhat { get; set; }

        [Range(typeof(decimal), "0", "1000000000000",
            ErrorMessage = "Giá phải từ 0 đến 1.000.000.000.000.")]
        public decimal? GiaLonNhat { get; set; }

        [StringLength(100)]
        public string? AnhDaiDien { get; set; }

        [StringLength(255)]
        public string? GioiThieuSp { get; set; }
    }
}