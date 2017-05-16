using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Forms;

namespace Librame.Tests.Forms
{
    [TestClass()]
    public class FormsAdapterTests
    {
        private readonly IFormsAdapter _adapter = null;

        public FormsAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.Forms;
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