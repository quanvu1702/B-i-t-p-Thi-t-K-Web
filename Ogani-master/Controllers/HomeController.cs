using Microsoft.AspNetCore.Mvc;
using Ogani_master.Models;
using System.Diagnostics;

namespace Ogani_master.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using var db = new Ogani_master.Modelss.QlbanVaLiContext();

            var sanPham = db.TDanhMucSps
                .OrderBy(x => x.MaSp)
                .ToList();

            return View(sanPham);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
