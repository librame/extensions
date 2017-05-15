using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Forms;

namespace Librame.UnitTests.Forms
{
    [TestClass()]
    public class FormsAdapterTests
    {
        private readonly IFormsAdapter _adapter = null;

        public FormsAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Forms;
        }


        [TestMethod()]
        public void FormsPropertiesTest()
        {
            Assert.IsNotNull(_adapter.FormsSettings);
            Assert.IsNotNull(_adapter.Scheme);
            Assert.IsNotNull(_adapter.Skin);
        }

    }
}