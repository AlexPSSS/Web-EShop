using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IGoodsService
    {
        /// <summary>
        /// Получение списка товаров
        /// </summary>
        /// <returns></returns>
        IEnumerable<GoodsView> GetAll();

        /// <summary>
        /// Получение товара по id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        GoodsView GetById(int id);

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        void Commit();

        /// <summary>
        /// Добавить новый
        /// </summary>
        /// <param name="model"></param>
        void AddNew(GoodsView model);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
