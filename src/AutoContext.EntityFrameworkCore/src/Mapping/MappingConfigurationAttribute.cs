using System;

namespace AutoContext.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Identifies the <see cref="DbContext" /> that a class belongs to. For example, this attribute is used
    /// to identify which context a mapping configuration applies to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappingConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingConfigurationAttribute" /> class.
        /// </summary>
        /// <param name="contextType"> The associated context. </param>
        public MappingConfigurationAttribute(Type contextType)
        {
            ContextType = contextType ?? throw new ArgumentNullException(nameof(contextType));
        }

        /// <summary>
        /// Gets the associated context.
        /// </summary>
        public Type ContextType { get; }
    }
}