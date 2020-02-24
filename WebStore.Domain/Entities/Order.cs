using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{
    public class Order : NamedEntity
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; } // внешний ключ в БД
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}