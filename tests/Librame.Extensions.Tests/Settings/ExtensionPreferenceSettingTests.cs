using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ExtensionPreferenceSettingTests
    {
        private readonly StringBuilder _builder
            = new StringBuilder();


        [Fact]
        public void RunLockerTest()
        {
            var maxLevel = ExtensionSettings.ProcessorCount / 2;
            var currentLevel = 1;

            TaoWa();

            var filePath = "lockers_test.txt".CombineCurrentDirectory();
            File.WriteAllText(filePath, _builder.ToString());

            void TaoWa()
            {
                ExtensionSettings.Preference.RunLocker(index =>
                {
                    WriteLine(index);

                    if (currentLevel < maxLevel)
                    {
                        currentLevel++;
                        TaoWa();
                    }
                });
            }

            File.Delete(filePath);
        }

        private void WriteLine(int index)
        {
            var thread = Thread.CurrentThread;
            _builder.AppendLine($"Thread {thread.ManagedThreadId} use Locker {index}.");

            var sleep = 100;
            Thread.Sleep(sleep);
            _builder.AppendLine($"Thread {thread.ManagedThreadId} sleep {sleep} milliseconds.");

            _builder.AppendLine($"Thread {thread.ManagedThreadId} release Locker {index}.");
        }

    }
}
