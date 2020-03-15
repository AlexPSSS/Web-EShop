using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDTO ToDTO(this Category Section) => Section is null ? null : new SectionDTO
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static Category FromDTO(this SectionDTO Section) => Section is null ? null : new Category
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        };

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Category> Sections) => Sections?.Select(ToDTO);

        public static IQueryable<SectionDTO> ToDTO(this IQueryable<Category> Sections) => Sections?.Select(Section => new SectionDTO
        {
            Id = Section.Id,
            Name = Section.Name,
            Order = Section.Order,
            ParentId = Section.ParentId,
        });
    }
}
