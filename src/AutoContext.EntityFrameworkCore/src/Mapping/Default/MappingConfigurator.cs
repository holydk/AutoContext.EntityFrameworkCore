using Microsoft.EntityFrameworkCore;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a default implementation of mapping configurator.
    /// </summary>
    public class MappingConfigurator : IMappingConfigurator
    {
        #region Fields

        private readonly IMappingProvider _mappingProvider;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of <see cref="MappingConfigurator" />.
        /// </summary>
        /// <param name="mappingProvider">The mapping provider.</param>
        public MappingConfigurator(IMappingProvider mappingProvider)
        {
            _mappingProvider = mappingProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets mapping configurations.
        /// </summary>
        /// <returns>The enumerable of mapping configurations</returns>
        public virtual void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            foreach (var mapping in _mappingProvider.GetMappings())
            {
                mapping.ApplyConfiguration(modelBuilder);
            }
        }
            
        #endregion
    }
}