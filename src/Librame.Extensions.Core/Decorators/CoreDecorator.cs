#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Decorators
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class CoreDecorator<TSource, TImplementation> : AbstractDecorator<TSource, TImplementation>
        where TSource : class
        where TImplementation : TSource
    {
        public CoreDecorator(TImplementation implementation)
            : base(implementation)
        {
        }
    }


    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class CoreDecorator<TSource> : AbstractDecorator<TSource>
        where TSource : class
    {
        public CoreDecorator(TSource source)
            : base(source)
        {
        }
    }
}
