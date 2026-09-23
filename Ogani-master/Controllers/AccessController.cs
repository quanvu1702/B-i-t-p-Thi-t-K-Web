using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani_master.Modelss;

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
            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new TUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(
            [Bind("Username,Password")] TUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Username)
                || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError(
                    "",
                    "Vui lòng nhập tên đăng nhập và mật khẩu."
                );

                return View(user);
            }

            string username = user.Username.Trim();

            var account = _db.TUsers
                .AsNoTracking()
                .FirstOrDefault(x => x.Username == username);

            // Phù hợp với mật khẩu văn bản trong database bài tập.
            bool dungMatKhau = account != null
                && string.Equals(
                    account.Password.TrimEnd(' '),
                    user.Password,
                    StringComparison.Ordinal
                );

            if (!dungMatKhau)
            {
                ModelState.AddModelError(
                    "",
                    "Tên đăng nhập hoặc mật khẩu không đúng."
                );

                user.Password = "";
                ModelState.Remove(nameof(user.Password));

                return View(user);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString(
                "UserName",
                account!.Username.TrimEnd(' ')
            );

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