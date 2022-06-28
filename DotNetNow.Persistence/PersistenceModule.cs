using System.Linq;
using System.Reflection;
using Autofac;
using DotNetNow.Utility;
using Module = Autofac.Module;

namespace DotNetNow.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var classes =
                ReflectionHelper.GetTypesInNamespace(Assembly.GetExecutingAssembly(), "Persistence.Repositories");
            var interfaces = ReflectionHelper.GetTypesInNamespace(Assembly.GetExecutingAssembly(),
                "Persistence.Persistence.Interface");

            foreach (var classType in classes)
            {
                var interfaceType = interfaces.FirstOrDefault(item => string.Equals($"I{classType.Name}", item.Name));
                if (interfaceType != null)
                {
                    builder.RegisterGeneric(classType)
                        .As(interfaceType);
                }
            }
            // builder.RegisterGeneric(typeof(GenericRepository<>))
            //         .As(typeof(IGenericRepository<>));
            // builder.RegisterGeneric(typeof(QueryableRepository<>))
            //     .As(typeof(IQueryableRepository<>));
        }
    }
}