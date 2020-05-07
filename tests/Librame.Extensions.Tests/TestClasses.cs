using System;

namespace Librame.Extensions.Tests
{
    public class TestClass
    {
        static string _field1 = nameof(_field1);
        //string _field2 = null;
        public string Field3;

        static string Property1 { get; set; }
        string Property2 { get; set; }
        public string Property3 { get; set; }
    }


    public class TestSubClass : TestClass
    {
        public string Abbr { get; set; }
    }


    public class TestSubClassReference
    {
        public TestSubClassReference(TestSubClass testSub)
        {
            TestSub = testSub;
        }

        public TestSubClass TestSub { get; }
    }
}
