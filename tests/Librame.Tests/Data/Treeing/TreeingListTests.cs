using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.Tests.Data.Treeing
{
    using Entities;

    [TestClass()]
    public class TreeingListTests
    {
        private readonly IList<Prefecture> _prefectures = null;

        public TreeingListTests()
        {
            _prefectures = new List<Prefecture>()
            {
                new Prefecture() { Id = 1, ParentId = 0, Name = "北京市" },
                new Prefecture() { Id = 2, ParentId = 0, Name = "上海市" },
                new Prefecture() { Id = 3, ParentId = 0, Name = "天津市" },
                new Prefecture() { Id = 4, ParentId = 0, Name = "重庆市" },
                new Prefecture() { Id = 5, ParentId = 4, Name = "渝中区" },
                new Prefecture() { Id = 6, ParentId = 4, Name = "渝北区" },
                new Prefecture() { Id = 7, ParentId = 4, Name = "沙坪坝区" },
                new Prefecture() { Id = 8, ParentId = 4, Name = "九龙坡区" },
                new Prefecture() { Id = 9, ParentId = 4, Name = "江津区" },
                new Prefecture() { Id = 10, ParentId = 4, Name = "永川区" },
                new Prefecture() { Id = 11, ParentId = 10, Name = "胜利路" },
                new Prefecture() { Id = 12, ParentId = 10, Name = "中山路" }
            };
        }


        [TestMethod()]
        public void ToNodesTest()
        {
            var nodes = _prefectures.AsTreeing();

            Assert.IsNotNull(nodes);
        }

    }
}
