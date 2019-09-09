namespace Librame.Extensions.Data.Tests
{
    public class TestDbContextAccessorFactory : DbContextAccessorFactory<TestDbContextAccessor>
    {
        public TestDbContextAccessorFactory()
            : base(TestServiceProvider.Current)
        {
        }

    }
}
