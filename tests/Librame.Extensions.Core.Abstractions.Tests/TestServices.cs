using System;

namespace Librame.Extensions.Core.Tests
{
    using Services;

    public interface ITestAnimal : IService
    {
    }

    public class TestCat : AbstractService, ITestAnimal
    {
    }

    public class TestDog : AbstractService, ITestAnimal
    {
    }

    public class TestTiger : AbstractService, ITestAnimal
    {
    }
}
