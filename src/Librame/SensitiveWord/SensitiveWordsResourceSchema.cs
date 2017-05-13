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
using System.Xml.Serialization;

namespace Librame.SensitiveWord
{
    using Resource.Schema;
    using Utility;
    
    /// <summary>
    /// 敏感词资源结构。
    /// </summary>
    [XmlRoot("Resource")]
    public class SensitiveWordsResourceSchema : ResourceSchema
    {
        /// <summary>
        /// 构造一个默认敏感词资源结构。
        /// </summary>
        public SensitiveWordsResourceSchema()
        {
            Sensitivity = new SensitivitySchemaSection();
        }
        /// <summary>
        /// 构造一个敏感词资源结构。
        /// </summary>
        /// <param name="words">给定的初始敏感词列表。</param>
        public SensitiveWordsResourceSchema(IList<string> words)
        {
            Sensitivity = new SensitivitySchemaSection(words);
        }
        /// <summary>
        /// 构造一个敏感词资源结构。
        /// </summary>
        /// <param name="sensitivity">给定的敏感性结构部分。</param>
        protected SensitiveWordsResourceSchema(SensitivitySchemaSection sensitivity)
        {
            Sensitivity = sensitivity.NotNull(nameof(sensitivity));
        }


        /// <summary>
        /// 敏感词结构部分。
        /// </summary>
        public SensitivitySchemaSection Sensitivity { get; set; }


        /// <summary>
        /// 默认实例。
        /// </summary>
        internal static SensitiveWordsResourceSchema Default
        {
            get
            {
                var words = new List<string>
                {
                    "肉棒",
                    "被干",
                    "潮吹",
                    "吃精",
                    "大波",
                    "荡妇",
                    "荡女",
                    "龟头",
                    "A片",
                    "宏志",
                    "法轮",
                    "法伦",
                    "中南海",
                    "台独",
                    "港独",
                    "藏独",
                    "疆独",
                    "戒严",
                    "大跃进",
                    "文革",
                    "民主",
                    "独裁",
                    "A片",
                    "枪支",
                    "弹药",
                    "炸药",
                    "大刀"
                };

                return new SensitiveWordsResourceSchema(words);
            }
        }
    }


    /// <summary>
    /// 敏感性结构部分。
    /// </summary>
    public class SensitivitySchemaSection : AbstractSchemaSection
    {
        /// <summary>
        /// 构造一个默认敏感性结构部分实例。
        /// </summary>
        public SensitivitySchemaSection()
        {
            Words = new List<string>();
        }
        /// <summary>
        /// 构造一个敏感性结构部分实例。
        /// </summary>
        /// <param name="words">给定的初始敏感词列表。</param>
        public SensitivitySchemaSection(IList<string> words)
        {
            Words = words.NotNull(nameof(words));
        }

        /// <summary>
        /// 部分名称。
        /// </summary>
        protected override string SectionName
        {
            get { return "Sensitivity"; }
        }


        /// <summary>
        /// 单词列表。
        /// </summary>
        public IList<string> Words { get; set; }
    }

}
