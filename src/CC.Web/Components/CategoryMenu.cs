
using System.Threading.Tasks;
using CC.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CC.Web.Components
{
    public class CategoryMenu : ViewComponent
    {
        ICatalogService _catalog;

        public CategoryMenu(ICatalogService catalog)
        {
            _catalog = catalog;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _catalog.GetCategories();
            return View(categories);
        }
    }
}