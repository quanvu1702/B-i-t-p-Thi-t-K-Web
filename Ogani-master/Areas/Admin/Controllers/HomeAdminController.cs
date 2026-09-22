using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani_master.Modelss;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ogani_master.ViewModels;

namespace Ogani_master.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly QlbanVaLiContext _db;

        public HomeAdminController(QlbanVaLiContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DanhMucSanPham(int page = 1)
        {
            const int pageSize = 8;

            var query = _db.TDanhMucSps.AsNoTracking();

            int totalItems = query.Count();
            int totalPages = Math.Max(
                1,
                (int)Math.Ceiling(totalItems / (double)pageSize)
            );

            page = Math.Clamp(page, 1, totalPages);

            var sanPham = query
                .OrderBy(x => x.MaSp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.PageSize = pageSize;

            return View(sanPham);
        }
        private void NapDanhSachLoai(string? maLoai = null)
        {
            ViewBag.DanhSachLoai = new SelectList(
                _db.TLoaiSps.AsNoTracking()
                    .OrderBy(x => x.Loai)
                    .ToList(),
                "MaLoai",
                "Loai",
                maLoai
            );
        }

        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            NapDanhSachLoai();
            return View(new ThemSanPhamViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(ThemSanPhamViewModel model)
        {
            model.MaSp = model.MaSp?.Trim() ?? "";
            model.TenSp = model.TenSp?.Trim() ?? "";

            if (_db.TDanhMucSps.Any(x => x.MaSp == model.MaSp))
            {
                ModelState.AddModelError(
                    nameof(model.MaSp),
                    "Mã sản phẩm này đã tồn tại."
                );
            }

            if (!string.IsNullOrWhiteSpace(model.MaLoai)
                && !_db.TLoaiSps.Any(x => x.MaLoai == model.MaLoai))
            {
                ModelState.AddModelError(
                    nameof(model.MaLoai),
                    "Loại sản phẩm không hợp lệ."
                );
            }

            if (model.GiaNhoNhat.HasValue
                && model.GiaLonNhat.HasValue
                && model.GiaLonNhat.Value < model.GiaNhoNhat.Value)
            {
                ModelState.AddModelError(
                    nameof(model.GiaLonNhat),
                    "Giá lớn nhất phải lớn hơn hoặc bằng giá nhỏ nhất."
                );
            }

            if (!ModelState.IsValid)
            {
                NapDanhSachLoai(model.MaLoai);
                return View(model);
            }

            var sanPham = new TDanhMucSp
            {
                MaSp = model.MaSp,
                TenSp = model.TenSp,
                MaLoai = model.MaLoai,
                GiaNhoNhat = model.GiaNhoNhat,
                GiaLonNhat = model.GiaLonNhat,
                AnhDaiDien = model.AnhDaiDien?.Trim(),
                GioiThieuSp = model.GioiThieuSp?.Trim()
            };

            _db.TDanhMucSps.Add(sanPham);
            _db.SaveChanges();

            TempData["ThongBao"] = "Đã thêm sản phẩm: " + sanPham.MaSp;
            return RedirectToAction(nameof(DanhMucSanPham));
        }
        [HttpGet]
        public IActionResult SuaSanPham(string? maSp)
        {
            if (string.IsNullOrWhiteSpace(maSp))
            {
                return NotFound("Chưa có mã sản phẩm.");
            }

            var sanPham = _db.TDanhMucSps
                .AsNoTracking()
                .FirstOrDefault(x => x.MaSp == maSp);

            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            var model = new ThemSanPhamViewModel
            {
                MaSp = sanPham.MaSp,
                TenSp = sanPham.TenSp ?? "",
                MaLoai = sanPham.MaLoai ?? "",
                GiaNhoNhat = sanPham.GiaNhoNhat,
                GiaLonNhat = sanPham.GiaLonNhat,
                AnhDaiDien = sanPham.AnhDaiDien,
                GioiThieuSp = sanPham.GioiThieuSp
            };

            NapDanhSachLoai(model.MaLoai);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(ThemSanPhamViewModel model)
        {
            var sanPham = _db.TDanhMucSps
                .FirstOrDefault(x => x.MaSp == model.MaSp);

            if (sanPham == null)
            {
                return NotFound("Không tìm thấy sản phẩm cần sửa.");
            }

            if (!string.IsNullOrWhiteSpace(model.MaLoai)
                && !_db.TLoaiSps.Any(x => x.MaLoai == model.MaLoai))
            {
                ModelState.AddModelError(
                    nameof(model.MaLoai),
                    "Loại sản phẩm không hợp lệ."
                );
            }

            if (model.GiaNhoNhat.HasValue
                && model.GiaLonNhat.HasValue
                && model.GiaLonNhat.Value < model.GiaNhoNhat.Value)
            {
                ModelState.AddModelError(
                    nameof(model.GiaLonNhat),
                    "Giá lớn nhất phải lớn hơn hoặc bằng giá nhỏ nhất."
                );
            }

            if (!ModelState.IsValid)
            {
                NapDanhSachLoai(model.MaLoai);
                return View(model);
            }

            sanPham.TenSp = model.TenSp.Trim();
            sanPham.MaLoai = model.MaLoai;
            sanPham.GiaNhoNhat = model.GiaNhoNhat;
            sanPham.GiaLonNhat = model.GiaLonNhat;
            sanPham.AnhDaiDien = model.AnhDaiDien?.Trim();
            sanPham.GioiThieuSp = model.GioiThieuSp?.Trim();

            _db.SaveChanges();

            TempData["ThongBao"] = "Đã cập nhật sản phẩm: " + sanPham.MaSp;
            return RedirectToAction(nameof(DanhMucSanPham));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult XoaSanPham(string maSp)
        {
            if (string.IsNullOrWhiteSpace(maSp))
            {
                return BadRequest("Chưa có mã sản phẩm.");
            }

            var sanPham = _db.TDanhMucSps
                .FirstOrDefault(x => x.MaSp == maSp);

            if (sanPham == null)
            {
                TempData["Loi"] = "Sản phẩm không tồn tại hoặc đã bị xóa.";
                return RedirectToAction(nameof(DanhMucSanPham));
            }

            // Không xóa sản phẩm đang có các bản ghi chi tiết.
            bool coChiTiet = _db.TChiTietSanPhams
                .Any(x => x.MaSp == maSp);

            if (coChiTiet)
            {
                TempData["Loi"] =
                    "Không thể xóa sản phẩm này vì đang có chi tiết sản phẩm.";

                return RedirectToAction(nameof(DanhMucSanPham));
            }

            // Xóa các bản ghi ảnh liên quan trước khi xóa sản phẩm.
            var danhSachAnh = _db.TAnhSps
                .Where(x => x.MaSp == maSp)
                .ToList();

            _db.TAnhSps.RemoveRange(danhSachAnh);
            _db.TDanhMucSps.Remove(sanPham);

            try
            {
                _db.SaveChanges();

                TempData["ThongBao"] =
                    "Đã xóa sản phẩm: " + sanPham.MaSp;
            }
            catch (DbUpdateException)
            {
                TempData["Loi"] =
                    "Chưa xóa được sản phẩm. Có thể dữ liệu đang được sử dụng "
                    + "hoặc đã thay đổi. Hãy tải lại trang và kiểm tra.";
            }

            return RedirectToAction(nameof(DanhMucSanPham));
        }
    }
}