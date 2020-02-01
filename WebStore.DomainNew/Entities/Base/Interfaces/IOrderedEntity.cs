using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.DomainNew.Entities.Base.Interfaces
{
    public interface IOrderedEntity : IBaseEntity
    {
        int Order { get; set; }
    }
}
