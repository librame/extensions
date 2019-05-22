using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SynchronizerTests
    {
        [Fact]
        public void SynchronizerTest()
        {
            var test1 = Guid.NewGuid().ToString();
            var test2 = Guid.NewGuid().ToString();

            var synchronizer = new Synchronizer<TestSyncManger, ITestSyncReader, ITestSyncWriter>(new TestSyncManger());

            var actions = new List<Action>();
            actions.Add(() => synchronizer.Write(writer => writer.SetValue(test1)));
            actions.Add(() => synchronizer.Write(writer => writer.SetValue(test2)));
            actions.Add(() => TestSyncManger.ReadWrite(synchronizer));
            Parallel.Invoke(actions.ToArray());

            string result = null;
            synchronizer.Read(reader => result = reader.GetValue());
            Assert.True(result != test1 || result != test2);
        }
    }


    interface ITestSyncReader : ISyncReader
    {
        string GetValue();
    }

    interface ITestSyncWriter : ISyncWriter
    {
        void SetValue(string value);
    }

    class TestSyncManger : ISyncManager, ITestSyncReader, ITestSyncWriter
    {
        string _value;

        public TestSyncManger()
        {
            _value = Guid.Empty.ToString();
        }

        public string GetValue()
        {
            return _value;
        }

        public void SetValue(string value)
        {
            _value = value;
        }


        public static void ReadWrite(Synchronizer<TestSyncManger, ITestSyncReader, ITestSyncWriter> synchronizer)
        {
            synchronizer.Write(x =>
            {
                x.SetValue(Guid.NewGuid().ToString());
            });

            synchronizer.Read(x =>
            {
                Console.WriteLine(x.GetValue());
            });
        }
    }
}
