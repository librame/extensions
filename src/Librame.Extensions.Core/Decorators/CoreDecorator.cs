#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class CoreDecorator<TSource, TImplementation> : AbstractDecorator<TSource, TImplementation>
        where TSource : class
        where TImplementation : TSource
    {
        public CoreDecorator(TImplementation instance, ILoggerFactory loggerFactory)
            : base(instance, loggerFactory)
        {
        }
    }


    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class CoreDecorator<TSource> : AbstractDecorator<TSource>
        where TSource : class
    {
        public CoreDecorator(TSource instance, ILoggerFactory loggerFactory)
            : base(instance, loggerFactory)
        {
        }
    }
}
