using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Products
{
    //public class ProductDTO : INamedEntity, IOrderedEntity
    //{
    //    public int Id { get; set; }

    //    public string Name { get; set; }

    //    public int Order { get; set; }

    //    public decimal Price { get; set; }

    //    public string ImageUrl { get; set; }

    //    public BrandDTO Brand { get; set; }

    //    /// <summary>Сведения о секции товара в каталоге</summary>
    //    public SectionDTO Section { get; set; }
    //}


    /// <summary>Информация о товаре</summary>
    public class ProductDTO : INamedEntity, IOrderedEntity
    {
        /// <summary>Идентификатор товара в каталоге</summary>
        public int Id { get; set; }

        /// <summary>Название товара</summary>
        public string Name { get; set; }

        /// <summary>Порядковый номер для сортировки</summary>
        public int Order { get; set; }

        /// <summary>Цена</summary>
        public decimal Price { get; set; }

        /// <summary>Адрес изображения</summary>
        public string ImageUrl { get; set; }

        /// <summary>Сведения о бренде</summary>
        public BrandDTO Brand { get; set; }

        /// <summary>Сведения о секции товара в каталоге</summary>
        public SectionDTO Section { get; set; }
    }


}
