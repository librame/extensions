using System;

namespace Librame.Extensions.Tests
{
    interface ITestA<T>
    {
        void TestMethod();
    }


    interface ITestB : ITestA<string>
    {
    }


    public class TestClass : ITestB
    {
        static string _field1 = nameof(_field1);
        //string _field2 = null;
        public string Field3;

        static string Property1 { get; set; }
        string Property2 { get; set; }
        public string Property3 { get; set; }

        public void TestMethod()
        {
        }
    }

    public class TestClass<T> : TestClass
    {
        public T Item { get; }
    }


    public class TestSubClass : TestClass
    {
        public string Abbr { get; set; }
    }


    public class TestSubClassReference : TestClass<string>
    {
        public TestSubClassReference(TestSubClass testSub)
        {
            TestSub = testSub;
        }

        public TestSubClass TestSub { get; }
    }

}
