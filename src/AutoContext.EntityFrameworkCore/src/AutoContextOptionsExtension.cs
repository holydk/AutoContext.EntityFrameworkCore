using System;
using System.Linq;
using System.Text;
using AutoContext.EntityFrameworkCore.Mapping;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace AutoContext.EntityFrameworkCore
{
    /// <summary>
    /// Represents a AutoContext extension.
    /// </summary>
    public class AutoContextOptionsExtension : IDbContextOptionsExtension
    {
        #region Fields

        private string _logFragment;
        private string _mappingAssembly;
            
        #endregion

        #region Properties
            
        /// <summary>
        /// Creates a message fragment for logging typically containing information about
        /// any useful non-default options that have been configured.
        /// </summary>
        public string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    if (!string.IsNullOrWhiteSpace(_mappingAssembly))
                    {
                        builder.Append($"MappingAssembly {_mappingAssembly} ");
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        /// <summary>
        /// The name of the assembly that contains mappings.
        /// </summary>
        public virtual string MappingAssembly => _mappingAssembly;

        #endregion

        #region Ctor

        public AutoContextOptionsExtension()
        {
        }

        public AutoContextOptionsExtension(AutoContextOptionsExtension copyFrom)
        {
            if (copyFrom is null)
            {
                throw new ArgumentNullException(nameof(copyFrom));
            }

            _mappingAssembly = copyFrom._mappingAssembly;
        }        
            
        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance with all options the same as for this instance, but with the given option changed.
        /// It is unusual to call this method directly. Instead use <see cref="DbContextOptionsBuilder" />.
        /// </summary>
        /// <param name="mappingAssembly"> The option to change. </param>
        /// <returns> A new instance with the option changed. </returns>
        public virtual AutoContextOptionsExtension WithMappingAssembly(string mappingAssembly)
        {
            var clone = Clone();

            clone._mappingAssembly = mappingAssembly;

            return clone;
        }

        /// <summary>
        /// Adds the services required to make the selected options work. This is used when there
        /// is no external <see cref="IServiceProvider" /> and EF is maintaining its own service
        /// provider internally. This allows database providers (and other extensions) to register their
        /// required services when EF is creating an service provider.
        /// </summary>
        /// <param name="services"> The collection to add services to. </param>
        /// <returns> True if a database provider and was registered; false otherwise. </returns>
        public virtual bool ApplyServices(IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new EntityFrameworkServicesBuilder(services)
                .TryAddProviderSpecificServices(map => 
                {
                    map.TryAddScoped<IMappingProvider, AssemblyMappingProvider>();
                    map.TryAddScoped<IMappingConfigurator, MappingConfigurator>();
                });

            builder.TryAddCoreServices();      

            return true;
        }

        /// <summary>
        /// Returns a hash code created from any options that would cause a new <see cref="IServiceProvider" />
        /// to be needed. Most extensions do not have any such options and should return zero.
        /// </summary>
        /// <returns> A hash over options that require a new service provider when changed. </returns>
        public virtual long GetServiceProviderHashCode() => 0;

        /// <summary>
        /// Gives the extension a chance to validate that all options in the extension are valid.
        /// Most extensions do not have invalid combinations and so this will be a no-op.
        /// If options are invalid, then an exception should be thrown.
        /// </summary>
        /// <param name="options"> The options being validated. </param>
        public virtual void Validate(IDbContextOptions options)
        {
        }

        /// <summary>
        /// Finds an existing <see cref="AutoContextOptionsExtension" /> registered on the given options
        /// or throws if none has been registered.
        /// </summary>
        /// <param name="options"> The context options to look in. </param>
        /// <returns> The extension. </returns>
        public static AutoContextOptionsExtension Extract(IDbContextOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var relationalOptionsExtensions
                = options.Extensions
                    .OfType<AutoContextOptionsExtension>()
                    .ToList();

            if (relationalOptionsExtensions.Count == 0)
            {
                throw new InvalidOperationException("NoProviderConfigured");
            }

            if (relationalOptionsExtensions.Count > 1)
            {
                throw new InvalidOperationException("MultipleProvidersConfigured");
            }

            return relationalOptionsExtensions[0];
        }
            
        #endregion

        #region Utilities

        /// <summary>
        /// Override this method in a derived class to ensure that any clone created is also of that class.
        /// </summary>
        /// <returns> A clone of this instance, which can be modified before being returned as immutable. </returns>
        protected virtual AutoContextOptionsExtension Clone() => new AutoContextOptionsExtension(this);
            
        #endregion
    }
}