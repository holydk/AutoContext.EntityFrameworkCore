using Microsoft.EntityFrameworkCore;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents database context model mapping configuration.
    /// </summary>
    public interface IMappingConfiguration
    {
        /// <summary>
        /// Apply this mapping configuration to builder.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the database context.</param>
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}