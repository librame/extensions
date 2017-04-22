#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
using Librame.Data;
using Librame.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Librame.UnitTests.Data.Tools
{
    using Entities;
    
    /// <summary>
    /// 导入工具。
    /// </summary>
    public class ImportFacility : LibrameBase<ImportFacility>
    {
        /// <summary>
        /// 导入。
        /// </summary>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="jsonPath">给定的 JSON 数据路径。</param>
        public static void Import(IRepository<Prefecture> repository, string jsonPath)
        {
            try
            {
                // 读取文件内容
                var json = FileUtility.ReadContent(jsonPath, Archit.Adapters.Settings.Encoding);
                if (string.IsNullOrEmpty(json))
                    return;

                // 解析为集合对象
                var nodes = JsonConvert.DeserializeObject<List<PrefectureNode>>(json);
                if (ReferenceEquals(nodes, null) || nodes.Count < 1)
                    return;

                foreach (var n in nodes)
                {
                    // 保存
                    SavePrefecture(repository, n);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                throw ex;
            }
        }

        private static void SavePrefecture(IRepository<Prefecture> repository, PrefectureNode node)
        {
            // 转换为实体
            var parent = ToPrefecture(node);

            // 可能会出现部分拼音为空的情况
            if (string.IsNullOrEmpty(parent.Name))
            {
                var pinyin = Archit.Adapters.PinyinAdapter.Parse(parent.Title);

                parent.Name = pinyin.Name;
                parent.Acronym = pinyin.Acronym;
                parent.Initial = pinyin.Initial;
            }

            // 保存到数据库
            parent = repository.Create(parent);
            //Log.Info(parent.AsString());

            // 如果有子级列表
            if (!ReferenceEquals(node.Childs, null) && node.Childs.Count > 0)
            {
                foreach (var c in node.Childs)
                {
                    SavePrefecture(repository, c);
                }
            }
        }

        private static Prefecture ToPrefecture(PrefectureNode node)
        {
            return new Prefecture()
            {
                RegionId = 1,
                ParentId = node.ParentId,
                CodeId = node.CodeId,
                Initial = node.Initial,
                Acronym = node.Acronym,
                Name = node.Name,
                Title = node.Title
            };
        }

    }
}