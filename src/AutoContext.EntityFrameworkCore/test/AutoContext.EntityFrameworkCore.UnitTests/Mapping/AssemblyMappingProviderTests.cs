using System;
using System.Linq;
using System.Reflection;
using AutoContext.EntityFrameworkCore.Mapping;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using OtherAssemblyWithContext;

namespace AutoContext.EntityFrameworkCore.UnitTests.Mapping
{
    public class AssemblyMappingProviderTests
    {
        #region Fields

        private AssemblyMappingProvider _subject;
        private Mock<AutoContextOptionsExtension> _mockAutoContextOptionsExtension;
        private Mock<IDbContextOptions> _mockDbContextOptions;
        private Mock<ICurrentDbContext> _mockCurrentDbContext;

        #endregion

        #region Init

        [SetUp]
        public void Init()
        {
            var currentAssemblyName = Assembly.GetExecutingAssembly().FullName;
            _mockAutoContextOptionsExtension = MockHelpers.CreateMockAutoContextOptionsExtension(currentAssemblyName);

            _mockDbContextOptions = MockHelpers.CreateMockDbContextOptions(new [] { _mockAutoContextOptionsExtension.Object });

            var builder = new DbContextOptionsBuilder();
            _mockCurrentDbContext = MockHelpers.CreateMockCurrentDbContext(new AutoContext(builder.Options));

            InitSubject();
        }
            
        #endregion

        #region Current assembly tests

        [Test]
        [TestCase(null, "AutoContext.EntityFrameworkCore.UnitTests")]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1), null)]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2), null)]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1), "AutoContext.EntityFrameworkCore.UnitTests")]
        [TestCase(typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2), "AutoContext.EntityFrameworkCore.UnitTests")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssembly.Context1), "OtherAssemblyWithMappings")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), null)]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), "OtherAssemblyWithMappings")]
        [TestCase(typeof(OtherAssemblyContextConfigurations.ContextWithMappingsInOtherAssemblyAndCurrentAssembly.Context1), "OtherAssemblyWithContext")]
        public void GetMappings_each_context_in_assembly_should_contain_mappings(Type contextType, string assemblyName)
        {
            var builder = new DbContextOptionsBuilder();

            if (contextType != null)
            {
                var context = (DbContext)Activator.CreateInstance(contextType, builder.Options);
                UseDbContext(context);
            }

            UseMappingAssembly(assemblyName);
            InitSubject();
            
            _subject.GetMappings().Should().NotBeEmpty();
        }

        [Test]
        public void AssemblyMappingProvider_when_mapping_assembly_is_null_and_there_is_one_context_that_is_AutoContext_should_call_exception()
        {
            UseMappingAssembly(null);
            
            Action a = () => InitSubject();
            a.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void GetMappings_when_mappings_is_not_exists_for_context_in_current_assembly_should_return_empty_enumerable()
        {
            var builder = new DbContextOptionsBuilder();
            UseDbContext(new CurrentAssemblyConfigurations.ContextWithNoMappings.Context1(builder.Options));
            InitSubject();

            _subject.GetMappings().Should().BeEmpty();
        }

        [Test]
        public void GetMappings_for_AutoContext_should_contain_two_mapping_in_current_assembly()
        {
            var mappings = _subject.GetMappings();

            mappings.Count().Should().Be(2);
            mappings.Should().Contain(m => 
                m.GetType() == typeof(CurrentAssemblyConfigurations.OnlyAutoContext.AutoContextMappingEntity1)
                  || m.GetType() == typeof(CurrentAssemblyConfigurations.OnlyAutoContext.AutoContextMappingEntity2)
            );
        }

        [Test]
        public void GetMappings_for_MultipleContextsContext1_should_contain_two_mapping_in_current_assembly()
        {
            var builder = new DbContextOptionsBuilder();
            UseDbContext(new CurrentAssemblyConfigurations.MultipleContexts.Context1(builder.Options));
            InitSubject();

            var mappings = _subject.GetMappings();

            mappings.Count().Should().Be(2);
            mappings.Should().Contain(m => 
                m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1MappingEntity1)
                  || m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context1MappingEntity2)
            );
        }

        [Test]
        public void GetMappings_for_MultipleContextsContext2_should_contain_tree_mapping_in_current_assembly()
        {
            var builder = new DbContextOptionsBuilder();
            UseDbContext(new CurrentAssemblyConfigurations.MultipleContexts.Context2(builder.Options));
            InitSubject();

            var mappings = _subject.GetMappings();

            mappings.Count().Should().Be(3);
            mappings.Should().Contain(m => 
                m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity1)
                  || m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity2)
                       || m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity3)
            );
        }

        [Test]
        public void GetMappings_when_mapping_assembly_is_null_but_Context_and_mappings_located_in_current_assembly_should_contain_tree_mapping_in_current_assembly()
        {
            UseMappingAssembly(null);
            var builder = new DbContextOptionsBuilder();
            UseDbContext(new CurrentAssemblyConfigurations.MultipleContexts.Context2(builder.Options));
            InitSubject();

            var mappings = _subject.GetMappings();

            mappings.Count().Should().Be(3);
            mappings.Should().Contain(m => 
                m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity1)
                  || m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity2)
                       || m.GetType() == typeof(CurrentAssemblyConfigurations.MultipleContexts.Context2MappingEntity3)
            );
        }
            
        #endregion

        #region Other assembly
            
        #endregion
    
        #region Utilities

        private void UseMappingAssembly(string assemblyName)
        {
            _mockAutoContextOptionsExtension.SetupGet(m => m.MappingAssembly).Returns(assemblyName);
        }

        private void UseDbContext(DbContext context)
        {
            _mockCurrentDbContext.SetupGet(m => m.Context).Returns(context);
        }

        private void InitSubject()
        {
            _subject = new AssemblyMappingProvider(
                _mockDbContextOptions.Object, 
                _mockCurrentDbContext.Object);
        }
            
        #endregion
    }
}