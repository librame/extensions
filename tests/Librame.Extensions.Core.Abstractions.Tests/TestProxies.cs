using System;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Core.Tests
{
    using Proxies;

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
        public TestSourceProxy()
            : base(new TestSource())
        {
            Dependency.AddPostActions(p => p.Name, InvokeDependencyKind.PropertySet,
                (source, result) => source.NameChanged = source.Name);
        }

    }

}
