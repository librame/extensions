#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 文件名组合器。
    /// </summary>
    public class FileNameCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 构造一个 <see cref="FileNameCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null or empty.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        public FileNameCombiner(string fileName)
            : base(ParseParameters(fileName, out var baseName, out var extension))
        {
            BaseName = baseName;
            Extension = extension;
        }

        /// <summary>
        /// 构造一个 <see cref="FileNameCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseName"/> is null or empty.
        /// </exception>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名（可空）。</param>
        public FileNameCombiner(string baseName, string extension)
            : base(CombineParameters(baseName, extension))
        {
            BaseName = baseName;
            Extension = extension;
        }


        /// <summary>
        /// 基础名。
        /// </summary>
        public string BaseName { get; private set; }

        /// <summary>
        /// 扩展名。
        /// </summary>
        public string Extension { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineParameters(BaseName, Extension);


        #region Change

        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner ChangeBaseName(Func<string, string> newBaseNameFactory)
            => ChangeBaseName(newBaseNameFactory?.Invoke(BaseName));

        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newBaseName"/> is null or empty.
        /// </exception>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner ChangeBaseName(string newBaseName)
        {
            BaseName = newBaseName.NotEmpty(nameof(newBaseName));
            return this;
        }


        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner ChangeExtension(Func<string, string> newExtensionFactory)
            => ChangeExtension(newExtensionFactory?.Invoke(Extension));

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner ChangeExtension(string newExtension)
        {
            Extension = newExtension;
            return this;
        }

        #endregion


        #region With

        /// <summary>
        /// 带有基础名。
        /// </summary>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner WithBaseName(Func<string, string> newBaseNameFactory)
            => WithBaseName(newBaseNameFactory?.Invoke(BaseName));

        /// <summary>
        /// 带有基础名。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newBaseName"/> is null or empty.
        /// </exception>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner WithBaseName(string newBaseName)
            => new FileNameCombiner(newBaseName, Extension);


        /// <summary>
        /// 带有扩展名。
        /// </summary>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner WithExtension(Func<string, string> newExtensionFactory)
            => WithExtension(newExtensionFactory?.Invoke(Extension));

        /// <summary>
        /// 带有扩展名。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public FileNameCombiner WithExtension(string newExtension)
            => new FileNameCombiner(BaseName, newExtension);

        #endregion


        /// <summary>
        /// 是否为指定的扩展名（忽略大小写）。
        /// </summary>
        /// <param name="extension">给定的扩展名。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsExtension(string extension)
            => Extension.Equals(extension, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(string other)
            => Source.Equals(other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is FileNameCombiner other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(FileNameCombiner a, FileNameCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(FileNameCombiner a, FileNameCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        public static implicit operator string(FileNameCombiner combiner)
            => combiner?.ToString();


        private static string CombineParameters(string baseName, string extension = null)
            => baseName.NotEmpty(nameof(baseName)) + extension; // 存在不包含扩展名的文件名


        private static string ParseParameters(string fileName,
            out string baseName, out string extension)
        {
            (baseName, extension) = fileName.GetFileBaseNameAndExtension(out _);
            return baseName + extension;
        }


        /// <summary>
        /// 尝试解析组合器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null or empty.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="result">输出 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryParseCombiner(string fileName, out FileNameCombiner result)
        {
            result = new FileNameCombiner(fileName);
            return result.BaseName.IsNotEmpty();
        }

    }
}
