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
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PanGu;
using PanGu.HighLight;
using System;
using System.Collections.Generic;

namespace Librame.FulltextSearch
{
    using Utility;

    /// <summary>
    /// 全文检索助手。
    /// </summary>
    public class FulltextSearchHelper
    {
        /// <summary>
        /// 初始化配置文件集合。
        /// </summary>
        /// <param name="adapter">给定的适配器。</param>
        public static void InitConfigFiles(Adaptation.IAdapter adapter)
        {
            adapter.GuardNull(nameof(adapter));

            // 因字典目录文件太大，取消嵌入资源（须手动复制）
            //// Dict\\ChsDoubleName1.txt
            //var chsDoubleName1ResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\ChsDoubleName1.txt");
            //adapter.ExportConfigFile("Dict\\ChsDoubleName1.txt", chsDoubleName1ResourceName);

            //// Dict\\ChsDoubleName2.txt
            //var chsDoubleName2ResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\ChsDoubleName2.txt");
            //adapter.ExportConfigFile("Dict\\ChsDoubleName2.txt", chsDoubleName2ResourceName);

            //// Dict\\ChsSingleName.txt
            //var chsSingleNameResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\ChsSingleName.txt");
            //adapter.ExportConfigFile("Dict\\ChsSingleName.txt", chsSingleNameResourceName);

            //// Dict\\Dict.dct
            //var dictResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\Dict.dct");
            //adapter.ExportConfigFile("Dict\\Dict.dct", dictResourceName);

            //// Dict\\Stopword.txt
            //var stopwordResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\Stopword.txt");
            //adapter.ExportConfigFile("Dict\\Stopword.txt", stopwordResourceName);

            //// Dict\\Synonym.txt
            //var synonymResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\Synonym.txt");
            //adapter.ExportConfigFile("Dict\\Synonym.txt", synonymResourceName);

            //// Dict\\Wildcard.txt
            //var wildcardResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\Dict\\Wildcard.txt");
            //adapter.ExportConfigFile("Dict\\Wildcard.txt", wildcardResourceName);

            // PanGu.xml
            var panGuResourceName = adapter.ToManifestResourceName("_configs\\FulltextSearch\\PanGu.xml");
            adapter.ExportConfigFile("PanGu.xml", panGuResourceName);
        }


        #region Create Index

        /// <summary>
        /// 创建索引。
        /// </summary>
        /// <param name="directory">给定的索引目录。</param>
        /// <param name="analyzer">给定的分析器。</param>
        /// <param name="infos">给定的搜索信息列表。</param>
        /// <returns>返回表示创建索引是否成功的布尔值。</returns>
        public static bool CreateIndex(Directory directory, Analyzer analyzer, IList<FulltextSearchInfo> infos)
        {
            IndexWriter writer = null;

            try
            {
                // false表示追加（true表示删除之前的重新写入）
                writer = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED);
            }
            catch
            {
                // false表示追加（true表示删除之前的重新写入）
                writer = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.LIMITED);
            }

            foreach (var info in infos)
            {
                if (!ReferenceEquals(info, null))
                    CreateIndex(writer, info);
            }

            writer.Optimize();
            writer.Dispose();

            return true;
        }
        
        private static bool CreateIndex(IndexWriter writer, FulltextSearchInfo info)
        {
            var doc = new Document();
            var type = info.GetType();
            var properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var pi = properties[i];
                string name = pi.Name;
                object obj = pi.GetValue(info, null);
                string value = obj?.ToString();

                // id 在写入索引时必是不分词，否则是模糊搜索和删除，会出现混乱
                if (name == "id" || name == "flag")
                {
                    // id 不分词
                    doc.Add(new Field(name, value, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
                else
                {
                    doc.Add(new Field(name, value, Field.Store.YES, Field.Index.ANALYZED));
                }
            }

            writer.AddDocument(doc);

            return true;
        }

        #endregion


        #region Search

        /// <summary>
        /// 在标题和内容字段中查询数据。
        /// </summary>
        /// <param name="version">给定的版本。</param>
        /// <param name="directory">给定的索引目录。</param>
        /// <param name="analyzer">给定的分析器。</param>
        /// <param name="keyword">给定的关键字。</param>
        /// <returns>返回搜索信息列表。</returns>
        public static IList<FulltextSearchInfo> Search(Lucene.Net.Util.Version version, Directory directory, Analyzer analyzer, string keyword)
        {
            // 一个字段查询：new QueryParser(version, field, analyzer);
            string[] fileds = { "title", "content" };
            var parser = new MultiFieldQueryParser(version, fileds, analyzer);
            var query = parser.Parse(keyword);
            int n = 1000;

            var searcher = new IndexSearcher(directory, true);
            var docs = searcher.Search(query, null, n);

            if (docs == null || docs.TotalHits == 0)
            {
                return null;
            }
            else
            {
                var list = new List<FulltextSearchInfo>();
                int counter = 1;
                foreach (ScoreDoc sd in docs.ScoreDocs)
                {
                    var doc = searcher.Doc(sd.Doc);
                    var info = ParseInfo(doc, keyword);

                    list.Add(info);

                    counter++;
                }

                return list;
            }

            //st.Stop();
            //Response.Write("查询时间：" + st.ElapsedMilliseconds + " 毫秒<br/>");
        }

        /// <summary>
        /// 在不同的类型下再根据标题和内容字段中查询数据（分页）。
        /// </summary>
        /// <param name="version">给定的版本。</param>
        /// <param name="directory">给定的索引目录。</param>
        /// <param name="analyzer">给定的分析器。</param>
        /// <param name="keyword">给定的关键字。</param>
        /// <param name="flag">给定的标记。</param>
        /// <param name="pageIndex">给定的页索引。</param>
        /// <param name="pageSize">给定的页大小。</param>
        /// <param name="totalCount">给定的总数。</param>
        /// <returns>返回搜索信息列表。</returns>
        public static IList<FulltextSearchInfo> Search(Lucene.Net.Util.Version version, Directory directory, Analyzer analyzer,
            string keyword, string flag, int pageIndex, int pageSize, out int totalCount)
        {
            if (pageIndex < 1) pageIndex = 1;

            //Stopwatch st = new Stopwatch();
            //st.Start();

            var boolQuery = new BooleanQuery();

            if (!string.IsNullOrEmpty(flag))
            {
                var parser = new QueryParser(version, "flag", analyzer);
                var query = parser.Parse(flag);

                boolQuery.Add(query, Occur.MUST);//与运算  
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                // 一个字段查询：new QueryParser(version, field, analyzer);
                string[] fileds = { "title", "content" };
                var parser = new MultiFieldQueryParser(version, fileds, analyzer);

                var query = parser.Parse(keyword);
                boolQuery.Add(query, Occur.MUST);//与运算  
            }

            var collector = TopScoreDocCollector.Create(pageIndex * pageSize, false);
            var searcher = new IndexSearcher(directory, true); 
            searcher.Search(boolQuery, collector);

            if (collector == null || collector.TotalHits == 0)
            {
                totalCount = 0;
                return null;
            }
            else
            {
                totalCount = collector.TotalHits;
                int start = pageSize * (pageIndex - 1);
                int limit = pageSize;
                
                int counter = 1;
                var list = new List<FulltextSearchInfo>();
                
                var hits = collector.TopDocs(start, limit).ScoreDocs;
                foreach (ScoreDoc sd in hits)
                {
                    var doc = searcher.Doc(sd.Doc);
                    var info = ParseInfo(doc);
                    list.Add(info);

                    counter++;
                }

                return list;
            }

            //st.Stop();  
            //Response.Write("查询时间：" + st.ElapsedMilliseconds + " 毫秒<br/>");
        }

        private static FulltextSearchInfo ParseInfo(Document doc, string keyword = null)
        {
            string id = doc.Get("id");
            string title = doc.Get("title");
            string content = doc.Get("content");
            string flag = doc.Get("flag");
            string imageUrl = doc.Get("imageurl");
            string updateTime = doc.Get("updatetime");
            string createDate = doc.Get("createdate");

            if (!string.IsNullOrEmpty(keyword))
            {
                var formatter = new SimpleHTMLFormatter("<font color=\"red\">", "</font>");
                var highLighter = new Highlighter(formatter, new Segment());
                highLighter.FragmentSize = 50;
                content = highLighter.GetBestFragment(keyword, content);

                string titleHighlight = highLighter.GetBestFragment(keyword, title);
                if (titleHighlight != string.Empty)
                    title = titleHighlight;
            }

            return new FulltextSearchInfo(id, title, content, flag, imageUrl, updateTime, createDate);
        }

        #endregion


        #region Delete

        /// <summary>
        /// 删除索引数据。
        /// </summary>
        /// <param name="directory">给定的索引目录。</param>
        /// <param name="analyzer">给定的分析器。</param>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回表示删除是否成功的布尔值。</returns>
        public static bool Delete(Directory directory, Analyzer analyzer, string id)
        {
            var term = new Term("id", id);
            //Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);  
            //Version version = new Version();  
            //MultiFieldQueryParser parser = new MultiFieldQueryParser(version, new string[] { "name", "job" }, analyzer);//多个字段查询  
            //Query query = parser.Parse("小王");  

            //IndexReader reader = IndexReader.Open(directory_luce, false);  
            //reader.DeleteDocuments(term);  
            //Response.Write("删除记录结果： " + reader.HasDeletions + "<br/>");  
            //reader.Dispose();  

            // writer.DeleteDocuments(term) 或者 writer.DeleteDocuments(query);
            return Deletion(directory, analyzer, (w) => w.DeleteDocuments(term));
        }
        
        /// <summary>
        /// 删除全部索引数据。
        /// </summary>
        /// <param name="directory">给定的索引目录。</param>
        /// <param name="analyzer">给定的分析器。</param>
        /// <returns>返回表示删除是否成功的布尔值。</returns>
        public static bool DeleteAll(Directory directory, Analyzer analyzer)
        {
            return Deletion(directory, analyzer, (w) => w.DeleteAll());
        }
        
        private static bool Deletion(Directory directory, Analyzer analyzer, Action<IndexWriter> deleteAction)
        {
            var isSuccess = true;

            try
            {
                using (var writer = new IndexWriter(directory, analyzer, false, IndexWriter.MaxFieldLength.LIMITED))
                {
                    deleteAction?.Invoke(writer);

                    writer.Commit();
                    //writer.Optimize();//

                    isSuccess = writer.HasDeletions();
                }
            }
            catch
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        #endregion


        //private Lucene.Net.Store.Directory _directory_luce = null;
        ///// <summary>  
        //        /// Lucene.Net的目录-参数  
        //        /// </summary>  
        //public Lucene.Net.Store.Directory directory_luce
        //{
        //    get
        //    {
        //        if (_directory_luce == null) _directory_luce = Lucene.Net.Store.FSDirectory.Open(directory);
        //        return _directory_luce;
        //    }
        //}


        //private System.IO.DirectoryInfo _directory = null;
        ///// <summary>  
        //        /// 索引在硬盘上的目录  
        //        /// </summary>  
        //public System.IO.DirectoryInfo directory
        //{
        //    get
        //    {
        //        if (_directory == null)
        //        {
        //            string dirPath = AppDomain.CurrentDomain.BaseDirectory + "SearchIndex";
        //            if (System.IO.Directory.Exists(dirPath) == false) _directory = System.IO.Directory.CreateDirectory(dirPath);
        //            else _directory = new System.IO.DirectoryInfo(dirPath);
        //        }
        //        return _directory;
        //    }
        //}


        //private Analyzer _analyzer = null;
        ///// <summary>  
        //        /// 分析器  
        //        /// </summary>  
        //public Analyzer analyzer
        //{
        //    get
        //    {
        //        //if (_analyzer == null)  
        //        {
        //            _analyzer = new Lucene.Net.Analysis.PanGu.PanGuAnalyzer();//盘古分词分析器  
        //            //_analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);//标准分析器  
        //        }
        //        return _analyzer;
        //    }
        //}


        //private static Lucene.Net.Util.Version _version = Lucene.Net.Util.Version.LUCENE_30;
        ///// <summary>  
        //        /// 版本号枚举类  
        //        /// </summary>  
        //public Lucene.Net.Util.Version version
        //{
        //    get
        //    {
        //        return _version;
        //    }
        //}

    }

}
