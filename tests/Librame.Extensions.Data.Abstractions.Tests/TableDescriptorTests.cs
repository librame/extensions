using System;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Stores;

    public class TableDescriptorTests
    {
        [Fact]
        public void AllTest()
        {
            var table = TableDescriptor.Create<AbstractIdentifierEntity<int>>();
            var defaultName = "AbstractIdentifierEntities";

            // Prefix
            Assert.Equal(defaultName, table);
            Assert.Equal($"__{defaultName}", table.Copy().InsertPrivatePrefix());
            Assert.Equal($"Data_{defaultName}", table.Copy().InsertDataPrefix());

            // Suffix
            var now = DateTimeOffset.UtcNow;
            Assert.Equal($"{defaultName}_{now:yy}", table.Copy().AppendYearSuffix(now));
            Assert.Equal($"{defaultName}_{now:yyMM}", table.Copy().AppendYearAndMonthSuffix(now));
            Assert.Equal($"{defaultName}_{now:yyMMdd}", table.Copy().AppendTimestampSuffix(now));

            Assert.Equal($"{defaultName}_{now:yy}{now.AsQuarterOfYear().FormatString(2)}",
                table.Copy().AppendYearAndQuarterSuffix(now));

            Assert.Equal($"{defaultName}_{now:yy}{now.AsWeekOfYear().FormatString(2)}",
                table.Copy().AppendYearAndWeekSuffix(now));
        }

    }
}
