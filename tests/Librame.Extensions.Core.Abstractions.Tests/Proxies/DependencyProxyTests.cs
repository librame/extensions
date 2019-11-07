using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class DependencyProxyTests
    {
        public interface ITestSource
        {
            [Required]
            string Name { get; set; }

            string NameChanged { get; set; }
        }

        public class TestSource : ITestSource
        {
            public string Name { get; set; }

            public string NameChanged { get; set; }
                = nameof(Name);
        }


        public class TestSourceProxy : AbstractDependencyProxy<ITestSource>
        {
            //public static readonly ITestSource Instance
            //    = Create<ITestSource, TestSourceProxy>();


            public TestSourceProxy()
                : base(new TestSource())
            {
                Dependency.AddPostActions(p => p.Name, InvokeDependencyKind.PropertySet,
                    (source, result) => source.NameChanged = source.Name);
            }
        }


        [Fact]
        public void AllTest()
        {
            //var proxy = TestSourceProxy.Instance;
            var proxy = DispatchProxy.Create<ITestSource, TestSourceProxy>();

            proxy.Name = nameof(TestSource);
            Assert.Equal(proxy.Name, proxy.NameChanged);

            proxy.Name = nameof(TestSourceProxy);
            Assert.Equal(proxy.Name, proxy.NameChanged);

            Assert.Throws<ValidationException>(() =>
            {
                proxy.Name = null;
            });
        }
    }
}
