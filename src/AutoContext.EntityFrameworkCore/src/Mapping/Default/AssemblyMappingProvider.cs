using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a default implementation of assembly mapping provider.
    /// </summary>
    public class AssemblyMappingProvider : IMappingProvider
    {
        #region Fields

        private readonly Assembly _mappingAssembly;
        private readonly Type _contextType;
            
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of <see cref="AssemblyMappingProvider" />.
        /// </summary>
        /// <param name="options">The context options.</param>
        /// <param name="currentContext">The current db context.</param>
        public AssemblyMappingProvider(
            IDbContextOptions options,
            ICurrentDbContext currentContext)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _contextType = currentContext.Context.GetType();

            var assemblyName = AutoContextOptionsExtension.Extract(options)?.MappingAssembly;
            if (assemblyName == null && _contextType == typeof(AutoContextBase))
            {
                throw new InvalidOperationException("You should explicitly specify the assembly name in UseMappingAssembly.");
            }

            _mappingAssembly = assemblyName == null
                ? _contextType.GetTypeInfo().Assembly
                : Assembly.Load(new AssemblyName(assemblyName));
        }
            
        #endregion

        #region Utilities
            
        #endregion

        #region Methods

        /// <summary>
        /// Gets mapping configurations.
        /// </summary>
        /// <returns>The enumerable of mapping configurations.</returns>
        public virtual IEnumerable<IMappingConfiguration> GetMappings()
        {
            // dynamically load the mapping configurations
            var typeConfigurations = _mappingAssembly.GetTypes().Where(type =>
            {
                return type.GetInterface(nameof(IMappingConfiguration)) != null
                         && !type.IsAbstract
                           && !type.IsGenericType
                             && type.GetCustomAttribute<MappingConfigurationAttribute>()?.ContextType == _contextType;
            })
            .Select(type => (IMappingConfiguration)Activator.CreateInstance(type))
            .ToList();

            return typeConfigurations;
        }
            
        #endregion
    }
}