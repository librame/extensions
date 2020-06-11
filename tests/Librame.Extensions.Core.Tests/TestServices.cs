using System;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Decorators;
    using Services;

    public class TestService : AbstractService
    {
        public TestService()
            : base()
        {
        }

        public string Message { get; set; }
            = "1";
    }

    public class TestServiceDecorator : TestService
    {
        public TestServiceDecorator(IDecorator<TestService> decorator)
            : base()
        {
            Message = decorator.Source.Message + "2";
        }
    }

    public class TestServiceImplementation : TestService
    {
        public TestServiceImplementation()
            : base()
        {
            Message = "2";
        }
    }

    public class TestServiceImplementationDecorator
    {
        public TestServiceImplementationDecorator(IDecorator<TestService, TestServiceImplementation> decorator)
            : base()
        {
            OtherMessage = decorator.Source.Message + "1";
        }

        public string OtherMessage { get; }
    }

    public class TestInjectionService
    {
        [InjectionService]
        private ICoreBuilder _fieldBuilder = null;


        public TestInjectionService(IInjectionService injectionService)
        {
            injectionService.Inject(this);
        }


        [InjectionService]
        public ICoreBuilder PropertyBuilder { get; set; }


        public void InjectTest()
        {
            _fieldBuilder.NotNull(nameof(_fieldBuilder));
            PropertyBuilder.NotNull(nameof(PropertyBuilder));
        }

    }

}
