using System.Collections.Generic;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a mapping provider.
    /// <para>
    /// The service lifetime is <see cref="Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />. This means that each
    /// <see cref="Microsoft.EntityFrameworkCore.DbContext" /> instance will use its own instance of this service.
    /// The implementation may depend on other services registered with any lifetime.
    /// The implementation does not need to be thread-safe.
    /// </para>
    /// </summary>
    public interface IMappingProvider
    {
        /// <summary>
        /// Gets mapping configurations.
        /// </summary>
        /// <returns>The enumerable of mapping configurations.</returns>
        IEnumerable<IMappingConfiguration> GetMappings();
    }
}