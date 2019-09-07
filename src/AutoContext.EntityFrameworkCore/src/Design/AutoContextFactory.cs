using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AutoContext.EntityFrameworkCore
{
    /// <summary>
    /// Represents a abstractions for database context factory.
    /// Note: The database context should contain the constructor with DbContextOptions.
    /// </summary>
    /// <typeparam name="TContext">The database context.</typeparam>
    public abstract class AutoContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext: DbContext
    {
        #region Properties
            
        /// <summary>
        /// Gets mapping assembly field name.
        /// Default value is MappingAssemblyName.
        /// </summary>
        protected virtual string MappingAssemblyFieldName => "MappingAssemblyName";

        #endregion

        #region Methods

        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>New instance of TContext.</returns>
        public virtual TContext CreateDbContext(string[] args)
        {
            // build configuration
            var builder = new ConfigurationBuilder();         
            ConfigureConfigurationBuilder(builder);
            
            var configuration = builder.Build(); 

            // build options
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();         
            ConfigureDbContextOptions(configuration, optionsBuilder);

            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        }
            
        #endregion

        #region Utilities

        /// <summary>
        /// Configures the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        protected abstract void ConfigureConfigurationBuilder(ConfigurationBuilder builder);

        /// <summary>
        /// Configures the context options from configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="builder">The options builder.</param>
        protected virtual void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder<TContext> builder)
        {
            var mappingAssemblyName = configuration.GetValue<string>(MappingAssemblyFieldName, null);
            builder.UseMappingAssembly(mappingAssemblyName);
        }
            
        #endregion
    }
}