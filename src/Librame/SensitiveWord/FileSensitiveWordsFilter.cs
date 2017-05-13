#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.SensitiveWord
{
    using Resource;

    /// <summary>
    /// 文件型敏感词过滤器。
    /// </summary>
    public class FileSensitiveWordsFilter : ISensitiveWordsFilter
    {
        /// <summary>
        /// 用于更换敏感词的内容。
        /// </summary>
        internal const string REPLACEMENT = "【敏感词】";

        private SensitiveWordsResourceSchema _schema = null;

        /// <summary>
        /// 构造一个文件型敏感词过滤器实例。
        /// </summary>
        /// <param name="fileName">给定的敏感词定义文件名。</param>
        public FileSensitiveWordsFilter(string fileName)
        {
            InitializeSchema(fileName);
        }

        /// <summary>
        /// 初始化资源结构。
        /// </summary>
        /// <param name="fileName">给定的敏感词定义文件名。</param>
        protected virtual void InitializeSchema(string fileName)
        {
            var sourceInfo = ResourceSourceInfo.CreateInfo<SensitiveWordsResourceSchema>(fileName);

            var provider = LibrameArchitecture.AdapterManager.Resource.GetProvider(sourceInfo,
                SensitiveWordsResourceSchema.Default);

            _schema = (provider.ExistingSchema as SensitiveWordsResourceSchema);
        }


        /// <summary>
        /// 单词列表。
        /// </summary>
        public IList<string> Words
        {
            get { return _schema.Sensitivity.Words; }
        }

        /// <summary>
        /// 是否存在敏感词（需要执行过滤方法）。
        /// </summary>
        public bool Exists { get; private set; }


        /// <summary>
        /// 过滤内容。
        /// </summary>
        /// <param name="content">给定的要过滤的内容。</param>
        /// <param name="replacement">用于更换的字符串（可选）。</param>
        /// <returns>返回过滤后的字符串。</returns>
        public virtual string Filting(string content, string replacement = null)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            foreach (var w in Words)
            {
                // 如果已过滤为空字符串，则跳出
                if (string.IsNullOrEmpty(content))
                    break;
                
                // 如果包含敏感词
                if (content.Contains(w))
                {
                    if (!Exists)
                        Exists = true;

                    content = content.Replace(w, replacement);
                }
            }

            return content;
        }

    }
}
