#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Data
{
    /// <summary>
    /// 抽象数据适配器基类。
    /// </summary>
    public abstract class AbstractDataAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Data"); }
        }


        /// <summary>
        /// 获取或设置 <see cref="DataSettings"/>。
        /// </summary>
        public DataSettings DataSettings { get; set; }

    }
}
