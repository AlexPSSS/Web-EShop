using AutoMapper;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Infrastructure.AutoMapper
{
    public class DTOMapping : Profile
    {
        public DTOMapping()
        {
            CreateMap<ProductDTO, ProductViewModel>().ReverseMap();
            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ReverseMap();

            //CreateMap<SectionDTO, Category>().ReverseMap();
        }
    }
}
