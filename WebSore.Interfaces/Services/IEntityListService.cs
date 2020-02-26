using System.Collections.Generic;

namespace WebSore.Interfaces.Services
{
    /// <summary>
    /// Интерфейс для работы с сотрудниками
    /// </summary>
    public interface IEntityListService<T>
    {
        /// <summary>
        /// Получение списка сотрудников
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Получение сотрудника по id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// Добавить нового
        /// </summary>
        /// <param name="model"></param>
        void Add(T model);

        /// <summary>
        /// Редактирование сотрудника по id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        T Edit(int id, T model);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        bool Delete(int id);

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        void SaveChanges();
    }
}
