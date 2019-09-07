using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents base query mapping configuration for <see cref="AutoContext" />.
    /// Note: Use this class to inherit your mapping configurations if you have one context.
    /// </summary>
    /// <typeparam name="TEntity">The query type.</typeparam>
    [MappingConfiguration(contextType: typeof(AutoContext))]
    public abstract class AutoContextQueryTypeConfiguration<TEntity> : AbstractQueryTypeConfiguration<TEntity> where TEntity : class
    {
        
    }

    /// <summary>
    /// Represents base query type mapping configuration.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    public abstract class AbstractQueryTypeConfiguration<TQuery> : IMappingConfiguration, IQueryTypeConfiguration<TQuery> where TQuery : class
    {   
        #region Methods

        /// <summary>
        /// Configures the query type.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the query type</param>
        public virtual void Configure(QueryTypeBuilder<TQuery> builder)
        {
        }

        /// <summary>
        /// Apply this mapping configuration.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the database context.</param>
        public virtual void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

        #endregion
    }
}