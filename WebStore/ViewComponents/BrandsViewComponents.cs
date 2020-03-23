using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.ViewComponents
{
    [ViewComponent(Name = "Brands")]
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public BrandsViewComponent(IProductService productService) => _productService = productService;

        public async Task<IViewComponentResult> InvokeAsync(string BrandId)
        {
            return View(new BrandCompleteViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(BrandId, out var id) ? id : (int?)null
            });
        }

        private IEnumerable<BrandViewModel> GetBrands() => _productService
           .GetBrands()
           .Where(brand => brand.ProductsCount > 0)
           .Select(brand => new BrandViewModel
           {
               Id = brand.Id,
               Name = brand.Name,
               Order = brand.Order,
               Quantity = brand.ProductsCount,
           })
           .OrderBy(brand => brand.Order);

    }
}
