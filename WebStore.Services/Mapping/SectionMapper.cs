using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static CategoryDTO ToDTO(this Category Section) => Section is null ? null : new CategoryDTO
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static Category FromDTO(this CategoryDTO Section) => Section is null ? null : new Category
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static IEnumerable<CategoryDTO> ToDTO(this IEnumerable<Category> Sections) => Sections?.Select(ToDTO);

        public static IQueryable<CategoryDTO> ToDTO(this IQueryable<Category> Sections) => Sections?.Select(Section => new CategoryDTO
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        });
    }
}
