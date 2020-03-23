using Microsoft.AspNetCore.Mvc;
using System;
using WebStore.Controllers;
using WebStore.Domain.Models.BreadCrumbs;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductService _ProductData;

        public BreadCrumbsViewComponent(IProductService ProductData) => _ProductData = ProductData;

        private void GetParameters(out BreadCrumbsType Type, out int id, out BreadCrumbsType FromType)
        {
            Type = Request.Query.ContainsKey("CategoryId")
                ? BreadCrumbsType.Category
                : Request.Query.ContainsKey("BrandId")
                    ? BreadCrumbsType.Brand
                    : BreadCrumbsType.None;

            if ((string)ViewContext.RouteData.Values["action"] == nameof(CatalogController.ProductDetails))
            {
                Type = BreadCrumbsType.Product;
            }

            id = 0;
            FromType = BreadCrumbsType.Category;

            switch (Type)
            {
                default: throw new ArgumentOutOfRangeException(nameof(Type), Type, null);

                case BreadCrumbsType.None: 
                    break;

                case BreadCrumbsType.Category:
                    id = int.Parse(Request.Query["CategoryId"].ToString());
                    break;

                case BreadCrumbsType.Brand:
                    id = int.Parse(Request.Query["BrandId"].ToString());
                    break;

                case BreadCrumbsType.Product:
                    id = int.Parse(ViewContext.RouteData.Values["id"].ToString());
                    if (Request.Query.ContainsKey("FromBrand"))
                    {
                        FromType = BreadCrumbsType.Brand;
                    }
                    break;
            }
        }

        public IViewComponentResult Invoke()
        {
            GetParameters(out var Type, out var id, out var FromType);

            switch (Type)
            {
                default: return View(Array.Empty<BreadCrumbsViewModel>());

                case BreadCrumbsType.Category:
                    return View(new[]
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Category,
                            Id = id.ToString(),
                            Name = _ProductData.GetCategoryById(id).Name
                        }
                    });

                case BreadCrumbsType.Brand:
                    return View(new[]
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Brand,
                            Id = id.ToString(),
                            Name = _ProductData.GetBrandById(id).Name
                        }
                    });

                case BreadCrumbsType.Product:
                    var product = _ProductData.GetProductById(id);
                    return View(new[]
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = FromType,
                            Id = FromType == BreadCrumbsType.Category
                                ? product.Category.Id.ToString()
                                : product.Brand.Id.ToString(),
                            Name = FromType == BreadCrumbsType.Category
                                ? _ProductData.GetCategoryById(product.Category.Id).Name
                                : _ProductData.GetBrandById(product.Brand.Id).Name
                        },
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Product,
                            Id = product.Id.ToString(),
                            Name = product.Name
                        },
                    });
            }
        }
    }
}
