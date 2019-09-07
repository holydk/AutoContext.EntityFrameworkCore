using AutoContext.EntityFrameworkCore.Mapping;

namespace OtherAssemblyWithMappings
{
    public class OtherAssemblyMappingsConfigurations
    {
        public static class ContextWithMappingsInOtherAssembly
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

            #region Mappings

            [MappingConfiguration(contextType: typeof(OtherAssemblyWithContext.OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssembly.Context1))]
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
    
        public static class ContextWithMappingsInOtherAssemblyAndCurrentAssembly
        {
            #region Entities

            public class Context1Entity3
            {
                public int Id { get; set; }
            }

            public class Context1Entity4
            {
                public int Id { get; set; }
            }

            #endregion

            #region Mappings

            public class Context1MappingEntity3 : OtherAssemblyWithContext.OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1EntityTypeConfiguration<Context1Entity3>
            {
            }

            public class Context1MappingEntity4 : OtherAssemblyWithContext.OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1EntityTypeConfiguration<Context1Entity4>
            {
            }

            #endregion
        }
    }
}
