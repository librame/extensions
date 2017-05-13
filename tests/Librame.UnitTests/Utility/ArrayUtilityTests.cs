using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class ArrayUtilityTests
    {
        private readonly int[] _array = new int[] { 1, 5, 3, 6, 10, 55, 9, 2, 87, 12, 34, 75, 33, 47 };

        [TestMethod()]
        public void BubbleSortTest()
        {
            var array = new int[_array.Length];
            _array.CopyTo(array, 0);

            ArrayUtility.BubbleSort(array);
            Assert.AreNotEqual(_array, array);
        }

        [TestMethod()]
        public void SelectionSortTest()
        {
            var array = new int[_array.Length];
            _array.CopyTo(array, 0);

            ArrayUtility.SelectionSort(array);
            Assert.AreNotEqual(_array, array);
        }
    }


    [TestClass()]
    public class ArrayUtilityExtensionsTests
    {

    }
}
