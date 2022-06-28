using AutoMapper;
using DotNetNow.Application.DTO;
using DotNetNow.Domain.Entity;

namespace DotNetNow.Application
{
    public class ApplicationMappingModule : Profile
    {
        public ApplicationMappingModule()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
                CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
