using System.Collections.Generic;
using System.Linq;

namespace WebStore.Domain.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int ItemsCount => Items?.Sum(x => x.Quantity) ?? 0;
    }

    public class CartViewModel
    {
        public Dictionary<ProductViewModel, int> Items { get; set; } = new Dictionary<ProductViewModel, int>();
        public int ItemsCount => Items?.Sum(x => x.Value) ?? 0;
    }

}
