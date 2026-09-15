using Microsoft.AspNetCore.Mvc;
using Ogani_master.Repository;

namespace Ogani_master.ViewComponents
{
    public class LoaiSpMenuViewComponent : ViewComponent
    {
        private readonly ILoaiSpRepository _repository;

        public LoaiSpMenuViewComponent(
            ILoaiSpRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_repository.GetAllLoaiSp());
        }
    }
}