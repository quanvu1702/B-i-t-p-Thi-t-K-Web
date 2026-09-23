using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogani_master.Modelss;
using Ogani_master.ViewModels;

namespace Ogani_master.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly QlbanVaLiContext _db;

        public ProductsController(QlbanVaLiContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetProducts(string? maLoai)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return Unauthorized();
            }

            var query = _db.TDanhMucSps.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(maLoai))
            {
                query = query.Where(x => x.MaLoai == maLoai);
            }

            var sanPham = query
                .OrderBy(x => x.MaSp)
                .Select(x => new SanPhamApiViewModel
                {
                    MaSp = x.MaSp,
                    TenSp = x.TenSp,
                    AnhDaiDien = x.AnhDaiDien,
                    GiaNhoNhat = x.GiaNhoNhat
                })
                .ToList();

            return Ok(sanPham);
        }
    }
}