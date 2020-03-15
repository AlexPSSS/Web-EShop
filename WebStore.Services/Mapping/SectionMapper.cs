using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class SectionMapper
    {
        public static SectionDTO ToDTO(this Category Section) => Section is null ? null : new SectionDTO
        {
            Id = Section.Id,
            Name = Section.Name
        };

        public static Category FromDTO(this SectionDTO Section) => Section is null ? null : new Category
        {
            Id = Section.Id,
            Name = Section.Name
        };
    }
}
