﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.DomainNew.Entities.Base;
using WebStore.DomainNew.Entities.Base.Interfaces;

namespace WebStore.DomainNew.Entities
{
    [Table("Products")]
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")] // не обязательно в целом
        public virtual Category Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
    }
}
