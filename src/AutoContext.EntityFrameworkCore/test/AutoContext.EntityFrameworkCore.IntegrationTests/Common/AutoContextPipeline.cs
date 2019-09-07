using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AutoContext.EntityFrameworkCore.IntegrationTests
{
    public class AutoContextPipeline
    {
        public IServiceCollection ServiceCollection { get; set; }

        public IServiceProvider ServiceProvider => ServiceCollection?.BuildServiceProvider();

        public event Action<IServiceCollection> OnPostConfigureServices = services => { };

        public void Initialize(Type contextType, string assemblyName)
        {
            ServiceCollection = new ServiceCollection();

            Action<DbContextOptionsBuilder> builder = options =>
            {
                options.UseInMemoryDatabase(contextType.FullName);
                options.UseMappingAssembly(assemblyName);
            };

            typeof(Microsoft.Extensions.DependencyInjection.EntityFrameworkServiceCollectionExtensions)
                .GetMethod("AddDbContext", 1, new Type[] 
                {
                    typeof(IServiceCollection), 
                    typeof(Action<DbContextOptionsBuilder>), 
                    typeof(ServiceLifetime), 
                    typeof(ServiceLifetime)
                })
                .MakeGenericMethod(contextType)
                .Invoke(null, new object[] 
                { 
                    ServiceCollection, 
                    builder, 
                    ServiceLifetime.Scoped, 
                    ServiceLifetime.Scoped 
                });

            OnPostConfigureServices(ServiceCollection);
        }
    }
}