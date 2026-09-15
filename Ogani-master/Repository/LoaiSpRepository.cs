using Ogani_master.Modelss;
using System.Collections.Generic;
using System.Linq;

namespace Ogani_master.Repository
{
    public class LoaiSpRepository : ILoaiSpRepository
    {
        private readonly QlbanVaLiContext _context;

        public LoaiSpRepository(QlbanVaLiContext context)
        {
            _context = context;
        }

        public TLoaiSp Add(TLoaiSp loaiSp)
        {
            _context.TLoaiSps.Add(loaiSp);
            _context.SaveChanges();
            return loaiSp;
        }

        public TLoaiSp Update(TLoaiSp loaiSp)
        {
            _context.TLoaiSps.Update(loaiSp);
            _context.SaveChanges();
            return loaiSp;
        }

        public TLoaiSp Delete(string maLoaiSp)
        {
            var loaiSp = _context.TLoaiSps.Find(maLoaiSp);

            if (loaiSp == null)
            {
                throw new KeyNotFoundException(
                    $"Không tìm thấy loại sản phẩm: {maLoaiSp}");
            }

            _context.TLoaiSps.Remove(loaiSp);
            _context.SaveChanges();
            return loaiSp;
        }

        public TLoaiSp? GetLoaiSp(string maLoaiSp)
        {
            return _context.TLoaiSps.Find(maLoaiSp);
        }

        public IEnumerable<TLoaiSp> GetAllLoaiSp()
        {
            return _context.TLoaiSps
                .OrderBy(x => x.Loai)
                .ToList();
        }
    }
}