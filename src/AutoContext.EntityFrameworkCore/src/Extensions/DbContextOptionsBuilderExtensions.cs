using AutoContext.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// SQL Server specific extension methods for <see cref="DbContextOptionsBuilder" />.
    /// </summary>
    public static class SqlServerDbContextOptionsExtensions
    {
        #region Methods

        /// <summary>
        /// Configures source of the mapping configurations from assembly.
        /// If the mapping assembly name is null then will be use assembly of database context.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the context. </param>
        /// <param name="mappingAssemblyName"> The name of assembly that contains mapping configurations. </param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseMappingAssembly(this DbContextOptionsBuilder optionsBuilder, string mappingAssemblyName = null)
        {
            if (optionsBuilder == null) throw new System.ArgumentNullException(nameof(optionsBuilder));

            var extension = (AutoContextOptionsExtension)GetOrCreateExtension(optionsBuilder).WithMappingAssembly(mappingAssemblyName);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }
            
        #endregion

        #region Utilities

        private static AutoContextOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.Options.FindExtension<AutoContextOptionsExtension>()
               ?? new AutoContextOptionsExtension();
            
        #endregion
    }
}