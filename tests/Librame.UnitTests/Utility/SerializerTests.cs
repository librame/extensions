using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class SerializerTests
    {
        [TestMethod()]
        public void SerializerTest()
        {
            var select = new SelectList()
            {
                Text = "Text",
                Value = "Value",
                SelectedValue = true
            };

            var str = Serializer.SerializeBase64(select);
            Assert.IsTrue(str.Length > 0);

            var obj = Serializer.DeserializeBase64(str);
            Assert.IsNotNull(obj);
            Assert.AreEqual(select?.Text, ((SelectList)obj)?.Text);
        }

    }

}
