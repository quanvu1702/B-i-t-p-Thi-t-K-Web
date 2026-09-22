using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ogani_master.Modelss;
using Ogani_master.ViewModels;

namespace Ogani_master.Controllers
{
    public class AccessController : Controller
    {
        private readonly QlbanVaLiContext _db;

        public AccessController(QlbanVaLiContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string username = model.Username.Trim();

            var user = _db.TUsers
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == username);

            // Dành cho database bài tập lưu mật khẩu văn bản thường.
            if (user == null ||
                !string.Equals(
                    user.Password.TrimEnd(' '),
                    model.Password,
                    StringComparison.Ordinal))
            {
                ModelState.AddModelError(
                    "",
                    "Tên đăng nhập hoặc mật khẩu không đúng."
                );

                model.Password = "";
                ModelState.Remove(nameof(model.Password));

                return View(model);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}