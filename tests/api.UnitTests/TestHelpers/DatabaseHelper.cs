using Microsoft.EntityFrameworkCore;
using System;

namespace api.UnitTests.TestHelpers
{
    public static class DatabaseHelper
    {
        public static MssqlDbContext CreateInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<MssqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MssqlDbContext(options);
        }
    }
}