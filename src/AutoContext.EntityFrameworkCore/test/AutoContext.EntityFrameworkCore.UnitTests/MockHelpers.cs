using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;

namespace AutoContext.EntityFrameworkCore.UnitTests
{
    internal static class MockHelpers
    {
        public static Mock<IDbContextOptions> CreateMockDbContextOptions(IEnumerable<IDbContextOptionsExtension> extensions)
        {
            var mock = new Mock<IDbContextOptions>();

            mock.SetupGet(m => m.Extensions).Returns(extensions);

            return mock;
        }

        public static Mock<DbContextOptionsBuilder> CreateMockDbContextOptionsBuilder(DbContextOptions options)
        {
            var mock = new Mock<DbContextOptionsBuilder>();

            mock.SetupGet(m => m.Options).Returns(options);

            return mock;
        }

        public static Mock<AutoContextOptionsExtension> CreateMockAutoContextOptionsExtension(string assemblyName)
        {
            var mock = new Mock<AutoContextOptionsExtension>();

            mock.SetupGet(m => m.MappingAssembly).Returns(assemblyName);

            return mock;
        }

        public static Mock<ICurrentDbContext> CreateMockCurrentDbContext(DbContext context)
        {
            var mock = new Mock<ICurrentDbContext>();

            mock.SetupGet(m => m.Context).Returns(context);

            return mock;
        }
    }
}