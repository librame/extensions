#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 文件路径组合器。
    /// </summary>
    public class FilePathCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is null or empty.
        /// </exception>
        /// <param name="filePath">给定的文件路径。</param>
        public FilePathCombiner(string filePath)
            : base(filePath)
        {
            FileName = CreateFileNameCombiner(filePath.GetFileNameWithoutPath(out var basePath));
            BasePath = basePath;
        }

        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null or empty.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径（可空；如果为空，则默认使用 fileName 中可能包含的路径）。</param>
        public FilePathCombiner(string fileName, string basePath)
            : base(CombineParameters(fileName, basePath))
        {
            // fileName 可能包含路径
            FileName = CreateFileNameCombiner(fileName.GetFileNameWithoutPath(out var _basePath));
            
            // 如果基础路径为空，则默认使用 fileName 中可能包含的路径
            BasePath = basePath.NotEmptyOrDefault(_basePath, throwIfDefaultInvalid: false);
        }

        /// <summary>
        /// 构造一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName"/> is null.
        /// </exception>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径（可空）。</param>
        public FilePathCombiner(FileNameCombiner fileName, string basePath)
            : base(CombineParameters(fileName, basePath))
        {
            FileName = fileName;
            BasePath = basePath;
        }


        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// 文件名。
        /// </summary>
        public FileNameCombiner FileName { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineParameters(FileName, BasePath);


        /// <summary>
        /// 如果当前基础路径为空，则改变基础路径。
        /// </summary>
        /// <param name="defaultBasePath">给定的默认基础路径。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeBasePathIfEmpty(string defaultBasePath)
        {
            if (BasePath.IsEmpty())
                BasePath = defaultBasePath.NotEmpty(nameof(defaultBasePath));

            return this;
        }

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeBasePath(Func<string, string> newBasePathFactory)
            => ChangeBasePath(newBasePathFactory?.Invoke(BasePath));

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newBasePath"/> is null or empty.
        /// </exception>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeBasePath(string newBasePath)
        {
            BasePath = newBasePath.NotEmpty(nameof(newBasePath));
            return this;
        }


        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(Func<FileNameCombiner, FileNameCombiner> newFileNameFactory)
            => ChangeFileName(newFileNameFactory?.Invoke(FileName));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(Func<FileNameCombiner, string> newFileNameFactory)
            => ChangeFileName(newFileNameFactory?.Invoke(FileName));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newFileName"/> is null.
        /// </exception>
        /// <param name="newFileName">给定的新 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(FileNameCombiner newFileName)
        {
            FileName = newFileName.NotNull(nameof(newFileName));
            return this;
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newFileName"/> is null or empty.
        /// </exception>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回当前 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner ChangeFileName(string newFileName)
        {
            var newFileNameCombiner = CreateFileNameCombiner(newFileName);
            return ChangeFileName(newFileNameCombiner);
        }


        /// <summary>
        /// 依据当前文件组合器的文件名与指定的基础路径，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithBasePath(Func<string, string> newBasePathFactory)
            => WithBasePath(newBasePathFactory?.Invoke(BasePath));

        /// <summary>
        /// 依据当前文件组合器的文件名与指定的基础路径，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newBasePath"/> is null or empty.
        /// </exception>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithBasePath(string newBasePath)
            => new FilePathCombiner(FileName, newBasePath);


        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithFileName(Func<FileNameCombiner, FileNameCombiner> newFileNameFactory)
            => WithFileName(newFileNameFactory?.Invoke(FileName));

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithFileName(Func<FileNameCombiner, string> newFileNameFactory)
            => WithFileName(newFileNameFactory?.Invoke(FileName));

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newFileName"/> is null.
        /// </exception>
        /// <param name="newFileName">给定的新 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithFileName(FileNameCombiner newFileName)
            => new FilePathCombiner(newFileName, BasePath);

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newFileName"/> is null or empty.
        /// </exception>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public FilePathCombiner WithFileName(string newFileName)
            => new FilePathCombiner(newFileName, BasePath);


        /// <summary>
        /// 是指定的文件名（忽略大小写）。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsFileName(string fileName)
            => IsFileName(CreateFileNameCombiner(fileName));

        /// <summary>
        /// 是指定的文件名（忽略大小写）。
        /// </summary>
        /// <param name="fileName">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsFileName(FileNameCombiner fileName)
            => FileName == fileName;


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
            => obj is FilePathCombiner other && Equals(other);


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
        /// <param name="a">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(FilePathCombiner a, FilePathCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(FilePathCombiner a, FilePathCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static implicit operator string(FilePathCombiner combiner)
            => combiner?.ToString();


        private static FileNameCombiner CreateFileNameCombiner(string fileName)
            => new FileNameCombiner(fileName);


        private static string CombineParameters(string fileName, string basePath = null)
        {
            fileName.NotEmpty(nameof(fileName));

            // 此处不限制基础路径为空方便以文件名构造实例，稍后再以改变路径完成实例
            return basePath.IsNotEmpty() ? basePath.CombinePath(fileName) : fileName;
        }


        /// <summary>
        /// 尝试解析组合器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filePath"/> is null or empty.
        /// </exception>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="result">输出 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryParseCombiner(string filePath, out FilePathCombiner result)
        {
            result = new FilePathCombiner(filePath);
            return result.FileName.IsNotNull();
        }

    }
}
