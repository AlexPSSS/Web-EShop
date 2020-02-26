namespace WebStore.Domain.Models
{
    public class GoodsView
    {
            public int Id { get; set; }
            public string Description { get; set; }
            public string EAN13 { get; set; }
            public int Group { get; set; }
            public float Price { get; set; }
    }
}
