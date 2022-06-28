using Autofac;
using DotNetNow.Application.DTO;
using DotNetNow.Application.Interface;
using DotNetNow.Application.Main.Generic;
using DotNetNow.Domain.Entity;
using DotNetNow.Persistence.Interface;

namespace DotNetNow.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericService<,,>))
                .As(typeof(IGenericService<>));
            
            builder.RegisterType<GenericService<CategoryDTO, Category, IGenericRepository<Category>>>()
                .As(typeof(IGenericService<CategoryDTO>));

            builder.RegisterType<GenericService<ProductDTO, Product, IGenericRepository<Product>>>()
                .As(typeof(IGenericService<ProductDTO>));
        }
    }
}
