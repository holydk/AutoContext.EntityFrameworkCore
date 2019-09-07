using AutoContext.EntityFrameworkCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AutoContext.EntityFrameworkCore.IntegrationTests.Mapping
{
    public class CurrentAssemblyConfigurations
    {
        public static class OnlyAutoContext
        {
            #region Entities

            public class AutoContextEntity1
            {
                public int Id { get; set; }
            }

            public class AutoContextEntity2
            {
                public int Id { get; set; }
            }
                
            #endregion

            #region Mappings

            public class AutoContextMappingEntity1 : AutoContextEntityTypeConfiguration<AutoContextEntity1>
            {
                
            }

            public class AutoContextMappingEntity2 : AutoContextEntityTypeConfiguration<AutoContextEntity2>
            {
                
            }
                
            #endregion
        }
 
        public static class ContextWithNoMappings
        {
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

            #region Context

            public class Context1 : AutoContext
            {
                public Context1(DbContextOptions options) : base(options)
                {
                }
            }

            #endregion
        }

        public static class MultipleContexts
        {
            #region Context

            public class Context1 : AutoContext
            {
                public Context1(DbContextOptions options) : base(options)
                {
                }
            }

            public class Context2 : AutoContext
            {
                public Context2(DbContextOptions options) : base(options)
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

            public class Context2Entity1
            {
                public int Id { get; set; }
            }

            public class Context2Entity2
            {
                public int Id { get; set; }
            }

            public class Context2Entity3
            {
                public int Id { get; set; }
            }
                
            #endregion

            #region Mappings

            [MappingConfiguration(contextType: typeof(Context1))]
            public abstract class Context1EntityTypeConfiguration<TEntity> : AbstractEntityTypeConfiguration<TEntity> where TEntity : class
            {
            }

            [MappingConfiguration(contextType: typeof(Context2))]
            public abstract class Context2EntityTypeConfiguration<TEntity> : AbstractEntityTypeConfiguration<TEntity> where TEntity : class
            {
            }

            public class Context1MappingEntity1 : Context1EntityTypeConfiguration<Context1Entity1>
            {
            }

            public class Context1MappingEntity2 : Context1EntityTypeConfiguration<Context1Entity2>
            {
            }

            public class Context2MappingEntity1 : Context2EntityTypeConfiguration<Context2Entity1>
            {
            }

            public class Context2MappingEntity2 : Context2EntityTypeConfiguration<Context2Entity2>
            {
            }

            public class Context2MappingEntity3 : Context2EntityTypeConfiguration<Context2Entity3>
            {
            }
                
            #endregion

        }
    }
}