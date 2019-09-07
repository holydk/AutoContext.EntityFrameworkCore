using Microsoft.EntityFrameworkCore;

namespace AutoContext.EntityFrameworkCore
{
    /// <summary>
    /// Represents a base object context.
    /// </summary>
    public class AutoContext : DbContext
    {
        #region Ctor

        public AutoContext(DbContextOptions options)
            : base(options)
        {
            
        }
            
        #endregion

        #region Utilities

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.ApplyConfigurations(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }
            
        #endregion
    }
}