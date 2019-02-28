using Xunit;

namespace Librame.Threads.Tests
{
    public class SimpleThreadPoolTests
    {
        [Fact]
        public void AddJobTest()
        {
            using (var pool = new SimpleThreadPool())
            {
                for (int i = 0; i < 10; i++)
                {
                    var job = new JobDescriptor(i);

                    job.Execution = (t, args) =>
                    {
                        Assert.True(t.ManagedThreadId > 0);
                    };

                    pool.AddJob(job);
                }
            }
        }

    }
}
