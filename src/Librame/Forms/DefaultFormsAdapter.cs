#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms
{
    using Utility;

    /// <summary>
    /// 默认窗体适配器。
    /// </summary>
    public class DefaultFormsAdapter : AbstractFormsAdapter, IFormsAdapter
    {
        /// <summary>
        /// 获取方案构建器。
        /// </summary>
        public virtual Schemes.ISchemeBuilder Scheme
        {
            get { return SingletonManager.Resolve(key => new Schemes.SchemeBuilder()); }
        }

        /// <summary>
        /// 获取皮肤提供程序。
        /// </summary>
        public virtual ISkinProvider Skin
        {
            get { return SingletonManager.Resolve(key => new SkinProvider(FormsSettings, Scheme)); }
        }

    }
}
