#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms.Material.Animations
{
    /// <summary>
    /// 动画方向。
    /// </summary>
    enum AnimationDirection
    {
        /// <summary>
        /// In. Stops if finished.
        /// </summary>
        In,

        /// <summary>
        /// Out. Stops if finished.
        /// </summary>
        Out,

        /// <summary>
        /// Same as In, but changes to InOutOut if finished.
        /// </summary>
        InOutIn,

        /// <summary>
        /// Same as Out.
        /// </summary>
        InOutOut,

        /// <summary>
        /// Same as In, but changes to InOutRepeatingOut if finished.
        /// </summary>
        InOutRepeatingIn,

        /// <summary>
        /// Same as Out, but changes to InOutRepeatingIn if finished.
        /// </summary>
        InOutRepeatingOut
    }
}
