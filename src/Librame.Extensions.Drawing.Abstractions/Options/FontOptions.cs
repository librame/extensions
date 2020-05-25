#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Options
{
    using Core.Combiners;

    /// <summary>
    /// 字体选项。
    /// </summary>
    public class FontOptions
    {
        /// <summary>
        /// 字体文件组合器。
        /// </summary>
        public FilePathCombiner FilePath { get; set; }
            = new FilePathCombiner("font.ttf");

        /// <summary>
        /// 大小。
        /// </summary>
        public int Size { get; set; }
            = 16;
    }
}
