using System.ComponentModel.DataAnnotations;

namespace Ogani_master.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập.")]
        [StringLength(100)]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}