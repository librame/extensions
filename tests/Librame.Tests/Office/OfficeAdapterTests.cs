using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Office;
using Librame.Utility;
using System.Data;
using System.IO;

namespace Librame.Tests.Office
{
    [TestClass()]
    public class OfficeAdapterTests
    {
        private readonly IOfficeAdapter _adapter;

        public OfficeAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.Office;
        }


        [TestMethod()]
        public void ExportExcelTest()
        {
            var dt = new DataTable("Test Table");

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            for (var i = 1; i < 21; i++)
                dt.Rows.Add(i, "Test " + i);

            var filename = TestHelper.DefaultDirectory.AppendPath("test.xls");
            _adapter.ExportExcel(filename, dt);

            Assert.IsNotNull(File.Exists(filename));
        }

    }
}