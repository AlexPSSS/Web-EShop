using System;
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

        public BrandsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string BrandId)
        {
            return View(new BrandCompleteViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(BrandId, out var id) ? id : (int?)null
            });
        }

        private List<BrandViewModel> GetBrands()
        {
            var brands = _productService.GetBrands();

            var brandList = new List<BrandViewModel>();
            // 
            foreach (var branOne in brands)
            {
                brandList.Add(new BrandViewModel()
                {
                    Id = branOne.Id,
                    Name = branOne.Name,
                    Order = branOne.Order,
                });
            }

            brandList = brandList.OrderBy(c => c.Order).ToList();
            return brandList;

        }
    }
}
