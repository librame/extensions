using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class JobThreadPoolTests
    {
        [Fact]
        public void AddJobTest()
        {
            using (var pool = new JobThreadPool())
            {
                for (int i = 0; i < 10; i++)
                {
                    var job = new JobDescriptor(i);

                    job.Execution = (t, args) =>
                    {
                        Assert.True(t.ManagedThreadId > 0);
                    };

                    pool.Add(job);
                }

                pool.Execute();
            }
        }

    }
}
