using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;

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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = GetBrands();

            return View(brands);
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
