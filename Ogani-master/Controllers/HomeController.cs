using Microsoft.AspNetCore.Mvc;
using Ogani_master.Models;
using System.Diagnostics;
using Ogani_master.Filters;

namespace Ogani_master.Controllers
{
    public class HomeController : Controller
    {
        [SessionLogin]
        public IActionResult Index()
        {
            using var db = new Ogani_master.Modelss.QlbanVaLiContext();

            var sanPham = db.TDanhMucSps
                .OrderBy(x => x.MaSp)
                .ToList();

            return View(sanPham);
        }
        [SessionLogin]
        public IActionResult SanPhamTheoLoai(string? maLoai)
        {
            if (string.IsNullOrWhiteSpace(maLoai))
            {
                return RedirectToAction(nameof(Index));
            }

            using var db = new Ogani_master.Modelss.QlbanVaLiContext();

            var loai = db.TLoaiSps
                .FirstOrDefault(x => x.MaLoai == maLoai);

            if (loai == null)
            {
                return NotFound("Không tìm thấy loại sản phẩm.");
            }

            var sanPham = db.TDanhMucSps
                .Where(x => x.MaLoai == maLoai)
                .OrderBy(x => x.MaSp)
                .ToList();

            ViewBag.TenLoai = loai.Loai;

            return View(sanPham);
        }
        [SessionLogin]  
        public IActionResult ChiTietSanPham(string? maSp)
        {
            if (string.IsNullOrWhiteSpace(maSp))
            {
                return RedirectToAction(nameof(Index));
            }

            using var db = new Ogani_master.Modelss.QlbanVaLiContext();

            var sanPham = db.TDanhMucSps
                .FirstOrDefault(x => x.MaSp == maSp);

            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var anhSanPham = db.TAnhSps
                .Where(x => x.MaSp == maSp)
                .OrderBy(x => x.ViTri)
                .ThenBy(x => x.TenFileAnh)
                .ToList();

            ViewBag.AnhSanPham = anhSanPham;

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
