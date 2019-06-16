using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    internal class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame();

                services.AddSingleton<IRequestHandler<Ping, Pong>, PingHandler>();
                services.AddSingleton<IRequestPreProcessor<Ping>, PingPreProcessor>();
                services.AddSingleton<IRequestPostProcessor<Ping, Pong>, PingPongPostProcessor>();

                services.AddScoped<InjectionServiceTest>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }


    public class Ping : IRequest<Pong>
    {
        public string Message { get; set; }
    }
    public class Pong
    {
        public string Message { get; set; }
    }

    public class PingHandler : IRequestHandler<Ping, Pong>
    {
        public Task<Pong> HandleAsync(Ping request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new Pong { Message = request.Message + " Pong" });
        }
    }

    public class PingPreProcessor : IRequestPreProcessor<Ping>
    {
        public Task ProcessAsync(Ping request, CancellationToken cancellationToken = default)
        {
            request.Message = request.Message + " Ping";

            return Task.FromResult(0);
        }
    }
    public class PingPongPostProcessor : IRequestPostProcessor<Ping, Pong>
    {
        public Task Process(Ping request, Pong response, CancellationToken cancellationToken)
        {
            response.Message = response.Message + " " + request.Message;

            return Task.FromResult(0);
        }
    }


    public class InjectionServiceTest
    {
        [InjectionService]
        IBuilder _fieldBuilder = null;


        public InjectionServiceTest(IInjectionService injectionService)
        {
            injectionService.Inject(this);
        }


        [InjectionService]
        public IBuilder PropertyBuilder { get; set; }


        public void InjectTest()
        {
            _fieldBuilder.NotNull(nameof(_fieldBuilder));
            PropertyBuilder.NotNull(nameof(PropertyBuilder));
        }

    }
}
