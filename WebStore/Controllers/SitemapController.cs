using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices] IProductService ProductData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("Blog", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Contact", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebAPITest")),
            };

            nodes.AddRange(ProductData.GetCategories().Select(category => new SitemapNode(Url.Action("Shop", "Catalog", new { CategoryId = category.Id }))));

            foreach (var brand in ProductData.GetBrands())
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new { BrandId = brand.Id })));

            foreach (var product in ProductData.GetProducts(new ProductFilter()).Products)
                nodes.Add(new SitemapNode(Url.Action("ProductDetails", "Catalog", new { product.Id })));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
