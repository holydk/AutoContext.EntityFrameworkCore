using AutoContext.EntityFrameworkCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace OtherAssemblyWithContext
{
    public class OtherAssemblyContextConfigurations
    {
        public static class ContextWithMappingsInOtherAssembly
        {
            #region Context

            public class Context1 : AutoContext.EntityFrameworkCore.AutoContextBase
            {
                public Context1(DbContextOptions options) : base(options)
                {
                }
            }

            #endregion
        }

        public static class ContextWithMappingsInOtherAssemblyAndCurrentAssembly
        {
            #region Context

            public class Context1 : AutoContext.EntityFrameworkCore.AutoContextBase
            {
                public Context1(DbContextOptions options) : base(options)
                {
                }
            }

            #endregion

            #region Entities

            public class Context1Entity1
            {
                public int Id { get; set; }
            }

            public class Context1Entity2
            {
                public int Id { get; set; }
            }

            #endregion

            #region Mappings

            [MappingConfiguration(contextType: typeof(Context1))]
            public abstract class Context1EntityTypeConfiguration<TEntity> : AbstractEntityTypeConfiguration<TEntity> where TEntity : class
            {
            }

            public class Context1MappingEntity1 : Context1EntityTypeConfiguration<Context1Entity1>
            {
            }

            public class Context1MappingEntity2 : Context1EntityTypeConfiguration<Context1Entity2>
            {
            }

            #endregion
        }
    }
}
