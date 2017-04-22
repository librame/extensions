#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Scheduler
{
    /// <summary>
    /// 抽象调度程序适配器。
    /// </summary>
    public abstract class AbstractScheduleAdapter : Adaptation.AbstractAdapter
    {
        /// <summary>
        /// 获取调度程序信息。
        /// </summary>
        public override Adaptation.AdapterInfo AdapterInfo
        {
            get { return new Adaptation.AdapterInfo("Scheduler"); }
        }

    }
}
