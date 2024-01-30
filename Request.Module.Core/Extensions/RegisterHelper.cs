using Ardalis.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Request.Module.Application.Aggregates.LeaveRequestAggregate.Services;
using Request.Module.Application.Base;
using Request.Module.Infrastructure.Persistence;
using Request.Module.Infrastructure.Persistence.Data;
using Request.Module.Infrastructure.Persistence.Data.Repository;

namespace Request.Module.Application.Extensions
{
    public static class RegisterHelper
    {
        public static IServiceCollection RegisterRequestModule(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                 .RegisterAutoMapper()
                 .RegisterMediatR()
                 .RegisterEventSpecifics()
                 .RegisterDbContext(configuration)
                 .RegisterRepositories()
                 .RegisterServices();

            return services;
        }

        private static IServiceCollection RegisterMediatR(this IServiceCollection services)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RegisterHelper).Assembly));
            return services;
        }

        private static IServiceCollection RegisterEventSpecifics(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();
            services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RequestContext>(options =>
                      options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
            return services;
        }

        /// <summary>
        /// IBaseRepository<>'den türeyen repository'leri register eder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            var assembly = typeof(IBaseRepository<>).Assembly;
            var repositoryTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>)));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces()
      .FirstOrDefault(type => type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IBaseRepository<>)));

                services.AddScoped(interfaceType, repositoryType);
            }

            return services;
        }

        /// <summary>
        /// Interface'i ServiceBase'den türeyenleri register eder
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var assembly = typeof(ServiceBase).Assembly;
            var serviceTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
        .Any(i => i.IsAssignableTo(typeof(ServiceBase))));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault(x => !x.IsGenericType && x != typeof(ServiceBase));

                services.AddScoped(interfaceType, serviceType);
            }

            return services;
        }

        private static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfigurations));
            return services;
        }
    }

    public static class AppHelper
    {
        /// <summary>
        /// Model ile DB'yi syncler
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appService">app.Services</param>
        public static void UseAutoMigration(ConfigurationManager configuration, IServiceProvider appService)
        {
            var automigration = configuration.GetValue<bool>("AutoMigration", false);

            // automigration
            if (automigration)
            {
                using (var scope = appService.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<RequestContext>();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
    }
}
