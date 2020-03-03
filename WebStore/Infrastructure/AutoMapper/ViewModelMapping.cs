using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;

namespace WebStore.Infrastructure.AutoMapper
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<RegisterUserViewModel, User>()
                //.ForMember(UserModel => UserModel.UserName, opt => opt.MapFrom(ViewModel => ViewModel.UserName))
                ;

        }
    }
}
