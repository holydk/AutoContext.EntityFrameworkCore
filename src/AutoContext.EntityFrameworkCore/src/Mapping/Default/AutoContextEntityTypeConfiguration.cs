using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents base entity mapping configuration.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class AbstractEntityTypeConfiguration<TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        #region Methods

        /// <summary>
        /// Configures the entity.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity.</param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
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