using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Models
{
    public class BrandViewModel : INamedEntity, IOrderedEntity
    {
        public BrandViewModel()
        {
            Quantity = 0;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int Quantity { get; set; }
    }

}
