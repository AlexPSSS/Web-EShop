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

            //var parentSections = categories.Where(p => !p.ParentId.HasValue).ToArray();
            var brandList = new List<BrandViewModel>();
            // получим и заполним родительские категории
            foreach (var branOne in brands)
            {
                brandList.Add(new BrandViewModel()
                {
                    Id = branOne.Id,
                    Name = branOne.Name,
                    Order = branOne.Order
                });
            }

            // получим и заполним дочерние категории
            //foreach (var CategoryViewModel in brandList)
            //{
            //    var childCategories = categories.Where(c => c.ParentId == CategoryViewModel.Id);
            //    foreach (var childCategory in childCategories)
            //    {
            //        CategoryViewModel.ChildCategories.Add(new CategoryViewModel()
            //        {
            //            Id = childCategory.Id,
            //            Name = childCategory.Name,
            //            Order = childCategory.Order,
            //            ParentCategory = CategoryViewModel
            //        });
            //    }
            //    CategoryViewModel.ChildCategories = CategoryViewModel.ChildCategories.OrderBy(c => c.Order).ToList();
            //}

            brandList = brandList.OrderBy(c => c.Order).ToList();
            return brandList;

        }
    }
}
