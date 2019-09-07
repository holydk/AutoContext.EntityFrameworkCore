using System;
using AutoContext.EntityFrameworkCore.Mapping;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for the Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade
    /// returned from Microsoft.EntityFrameworkCore.DbContext.Database.
    /// </summary>
    public static class DatabaseFacadeExtensions
    {
        #region Methods

        /// <summary>
        /// Apply mapping configurations to database.
        /// </summary>
        /// <param name="databaseFacade">The database facade for context.</param>
        /// <param name="modelBuilder">The model builder.</param>
        public static void ApplyConfigurations(this DatabaseFacade databaseFacade, ModelBuilder modelBuilder)
        {
            GetService<IMappingConfigurator>(databaseFacade).ApplyConfigurations(modelBuilder);
        }
            
        #endregion
        
        #region Utilities

        /// <summary>
        /// Gets a TService from database facade.
        /// </summary>
        /// <param name="databaseFacade">The database facade for context.</param>
        /// <typeparam name="TService">The requered service.</typeparam>
        /// <returns>The instance of TService.</returns>
        private static TService GetService<TService>(this IInfrastructure<IServiceProvider> databaseFacade)
        {
            if (databaseFacade is null)
            {
                throw new ArgumentNullException(nameof(databaseFacade));
            }

            var service = (TService)databaseFacade.Instance.GetService(typeof(TService));
            if (service == null)
            {
                throw new InvalidOperationException($"Service {typeof(TService).Name} not found.");
            }

            return service;
        }
            
        #endregion
    }
}