#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System.Collections.Generic;

namespace Librame.FulltextSearch
{
    using Utility;

    /// <summary>
    /// 默认全文检索适配器。
    /// </summary>
    public class DefaultFulltextSearchAdapter : AbstractFulltextSearchAdapter, IFulltextSearchAdapter
    {
        /// <summary>
        /// 构造一个 <see cref="DefaultFulltextSearchAdapter"/> 默认实例。
        /// </summary>
        public DefaultFulltextSearchAdapter()
        {
            InitializeAdapter();

            PanGu.Segment.Init(ConfigDirectory.AppendPath("PanGu.xml"));
        }

        /// <summary>
        /// 初始化适配器。
        /// </summary>
        protected virtual void InitializeAdapter()
        {
            // 导出配置文件
            ExportConfigDirectory("PanGu.xml");

            // 因字典目录下文件太大，取消嵌入资源（须手动复制）
        }


        /// <summary>
        /// 字典目录（默认为 AdapterConfigDirectory\Dict）。
        /// </summary>
        public virtual string DictDirectory
        {
            get { return ConfigDirectory.AppendDirectoryName("Dict"); }
        }

        /// <summary>
        /// 索引目录（默认为 AdapterConfigDirectory\Index）。
        /// </summary>
        public virtual string IndexDirectory
        {
            get { return ConfigDirectory.AppendDirectoryName("Index"); }
        }


        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text">给定的文本。</param>
        /// <returns>返回字词数组。</returns>
        public virtual string[] Token(string text)
        {
            var keys = new List<string>();

            using (var a = new PanGuAnalyzer())
            {
                using (var sr = new System.IO.StringReader(text))
                {
                    using (var ts = a.ReusableTokenStream(text, sr))
                    {
                        ITermAttribute ta;
                        bool hasNext = ts.IncrementToken();
                        while (hasNext)
                        {
                            ta = ts.GetAttribute<ITermAttribute>();
                            keys.Add(ta.Term);

                            hasNext = ts.IncrementToken();
                        }

                        ts.CloneAttributes();
                    }
                }
            }

            return keys.ToArray();
        }

    }
}
