using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.ViewComponents
{
    [ViewComponent(Name = "Cats")]
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public CategoriesViewComponent(IProductService productService) => _productService = productService;

        public async Task<IViewComponentResult> InvokeAsync(string CategoryId)
        {
            var category_id = int.TryParse(CategoryId, out var id) ? id : (int?)null;

            var сategories = GetCategories(category_id, out var parent_category_id);

            return View(new CategoryCompleteViewModel
            {
                Categories = сategories,
                CurrentCategoryId = category_id,
                CurrentParentCategoryId = parent_category_id
            });
        }

        private IEnumerable<CategoryViewModel> GetCategories(int? CategoryId, out int? ParentCategoryId)
        {
            ParentCategoryId = null;

            var categories = _productService.GetCategories();

            var parent_categories = categories.Where(category => category.ParentId is null).ToArray();

            var parent_categories_views = parent_categories
               .Select(parent_category => new CategoryViewModel
               {
                   Id = parent_category.Id,
                   Name = parent_category.Name,
                   Order = parent_category.Order
               })
               .ToList();

            foreach (var parent_category_view in parent_categories_views)
            {
                var childs = categories.Where(category => category.ParentId == parent_category_view.Id);
                foreach (var child_category in childs)
                {
                    if (child_category.Id == CategoryId)
                        ParentCategoryId = parent_category_view.Id;

                    parent_category_view.ChildCategories.Add(
                        new CategoryViewModel
                        {
                            Id = child_category.Id,
                            Name = child_category.Name,
                            Order = child_category.Order,
                            ParentCategory = parent_category_view
                        });
                }
                parent_category_view.ChildCategories.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parent_categories_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return parent_categories_views;
        }

    }
}
