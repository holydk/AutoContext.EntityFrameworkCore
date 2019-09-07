using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OtherAssemblyWithContext;

namespace AutoContext.EntityFrameworkCore.IntegrationTests.Mapping
{
    public class AssemblyMappingProviderTests
    {
        #region Fields

        private AutoContextPipeline _pipeline;

        #endregion

        #region Init

        [SetUp]
        public void Init()
        {
            _pipeline = new AutoContextPipeline();
        }
            
        #endregion

        [Test]
        [TestCase(typeof(AutoContext), "AutoContext.EntityFrameworkCore.IntegrationTests")]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1), null)]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2), null)]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1), "AutoContext.EntityFrameworkCore.IntegrationTests")]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2), "AutoContext.EntityFrameworkCore.IntegrationTests")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssembly.Context1), "OtherAssemblyWithMappings")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), null)]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), "OtherAssemblyWithMappings")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), "OtherAssemblyWithContext")]
        public void Each_context_in_assembly_should_contain_mappings(Type contextType, string assemblyName)
        {
            _pipeline.Initialize(contextType, assemblyName);

            using (var scope = _pipeline.ServiceProvider.CreateScope())
            {
                var context = (DbContext)scope.ServiceProvider.GetService(contextType);
                context.Model.GetEntityTypes().Should().NotBeNull();
            }
        }

        [Test]
        public void Context_with_mappings_in_other_assembly_should_have_two_mappings()
        {
            _pipeline.Initialize(typeof(OtherAssemblyWithContext.OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssembly.Context1), "OtherAssemblyWithMappings");
            
            using (var scope =_pipeline.ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<OtherAssemblyWithContext.OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssembly.Context1>();
                var entities = context.Model.GetEntityTypes();

                entities.Should().HaveCount(2);
                entities.Should().Contain(e =>
                    e.Name == typeof(OtherAssemblyWithMappings.OtherAssemblyMappingsConfigurations.ContextWithMappingsInOtherAssembly.Context1Entity1).FullName
                      || e.Name == typeof(OtherAssemblyWithMappings.OtherAssemblyMappingsConfigurations.ContextWithMappingsInOtherAssembly.Context1Entity2).FullName
                );
            }
        }
    }
}