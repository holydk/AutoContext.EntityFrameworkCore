using Microsoft.EntityFrameworkCore;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a APIs to apply configurations to database.
    /// <para>
    /// The service lifetime is <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />. This means that each
    /// <see cref="DbContext" /> instance will use its own instance of this service.
    /// The implementation may depend on other services registered with any lifetime.
    /// The implementation does not need to be thread-safe.
    /// </para>
    /// </summary>
    public interface IMappingConfigurator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        void ApplyConfigurations(ModelBuilder modelBuilder);
    }
}