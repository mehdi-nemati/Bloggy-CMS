using Autofac;
using AutoMapper;
using Bloggy.Data;
using Bloggy.Data.Contracts;
using Bloggy.Data.Repositories;
using Bloggy.Entities;
using Bloggy.Service.PostCategoryService;
using Bloggy.Shared.Config;
using System.Reflection;

namespace Bloggy.Web.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void AddServices(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(IScopedDependency).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(BloggyDbContext).Assembly;
            var servicesAssembly = typeof(IPostCategoryService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        public static void InitializeAutoMapper(this IServiceCollection services)
        {
           services.AddAutoMapper(config =>
            {
                config.AddCustomMappingProfile(typeof(IPostCategoryService).Assembly);
            });
        }

        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config, params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            var list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IHaveMapping)))
                .Select(type => (IHaveMapping)Activator.CreateInstance(type));

            var profile = new CustomMappingProfile(list);

            config.AddProfile(profile);
        }

        public class CustomMappingProfile : Profile
        {
            public CustomMappingProfile(IEnumerable<IHaveMapping> haveCustomMappings)
            {
                foreach (var item in haveCustomMappings)
                    item.CreateMappings(this);
            }
        }
    }
}
