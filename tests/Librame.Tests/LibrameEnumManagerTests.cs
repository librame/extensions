﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.Tests
{
    [TestClass()]
    public class LibrameEnumManagerTests
    {
        [TestMethod()]
        public void EnumManagerAccountStatusListTest()
        {
            var list = LibrameEnumManager<SelectList>.GetAccountStatusList((value, text) => new SelectList()
            {
                Value = value,
                Text = text
            });

            Assert.IsNotNull(list);
        }

    }
}