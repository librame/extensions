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

namespace Librame.Extensions.Core
{
    class CoreDecorator<TSource, TImplementation> : AbstractDecorator<TSource, TImplementation>
        where TSource : class
        where TImplementation : TSource
    {
        public CoreDecorator(TImplementation instance, ILoggerFactory loggerFactory)
            : base(instance, loggerFactory)
        {
        }
    }


    class CoreDecorator<TSource> : AbstractDecorator<TSource>
        where TSource : class
    {
        public CoreDecorator(TSource instance, ILoggerFactory loggerFactory)
            : base(instance, loggerFactory)
        {
        }
    }
}
