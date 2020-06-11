using System;

namespace Librame.Extensions.Core.Tests
{
    using Singletons;

    public sealed class SealedGuidSingleton : AbstractSingleton<SealedGuidSingleton>
    {
        private SealedGuidSingleton()
            : base()
        {
        }


        public Guid Guid { get; }
            = Guid.NewGuid(); // => Guid.NewGuid(); 会导致每次变化
    }

    public class GuidSingleton
    {
        public Guid Guid { get; }
            = Guid.NewGuid(); // => Guid.NewGuid(); 会导致每次变化
    }

    public class DateTimeSingleton
    {
        public DateTime Time { get; }
            = DateTime.Now; // => DateTime.Now; 会导致每次变化
    }
}
