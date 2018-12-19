#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 默认分表规则。
    /// </summary>
    public class DefaultShardingRule : BaseShardingRule
    {
        /// <summary>
        /// 键名集合。
        /// </summary>
        public new class Keys : AbstractShardingRule.Keys
        {
            /// <summary>
            /// 默认键名。
            /// </summary>
            public static readonly string Default = BuildKey(nameof(Default));

            /// <summary>
            /// 实体键名复数加默认键名。
            /// </summary>
            public static readonly string EntitiesDefault = $"{Entities}_{Default}";
        }

    }
}
