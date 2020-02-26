using System;
using System.Collections.Generic;
using System.Linq;
using WebSore.Interfaces.Services;
using WebStore.Domain.Models;

namespace WebStore.Services.Product
{
    public class InMemoryGoodsService : IEntityListService<GoodsView>
    {
        private readonly List<GoodsView> _goods;
        public InMemoryGoodsService()
        {
            _goods = new List<GoodsView>
            {
                new GoodsView
                {
                    Id = 1,
                    Description = "Молоток",
                    EAN13 = "4602564782511",
                    Group = 100,
                    Price = 22.34F
                },
                new GoodsView
                {
                    Id = 2,
                    Description = "Дрель",
                    EAN13 = "4602564782619",
                    Group = 202,
                    Price = 100500.01F
                }
            };
        }

        public IEnumerable<GoodsView> GetAll()
        {
            return _goods;
        }

        public GoodsView GetById(int id)
        {
            return _goods.FirstOrDefault(e => e.Id.Equals(id));
        }

        public void Add(GoodsView model)
        {
            model.Id = ((_goods.Count > 0)?_goods.Max(e => e.Id) : 0) + 1;
            _goods.Add(model);
        }

        public GoodsView Edit(int id, GoodsView good)
        {
            if (good is null)
                throw new ArgumentNullException(nameof(good));

            var db_good = GetById(id);
            if (db_good is null) return null;

            return db_good;
        }

        public bool Delete(int id)
        {
            var good = GetById(id);
            return good != null && _goods.Remove(good);

        }
        public void SaveChanges() { }
    }
}
