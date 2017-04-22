#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Librame.Office
{
    using Utility;

    /// <summary>
    /// 默认办公适配器。
    /// </summary>
    public class DefaultOfficeAdapter : AbstractOfficeAdapter, IOfficeAdapter
    {
        /// <summary>
        /// 创建 WORD 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="createTable">创建表格的方法。</param>
        /// <param name="createTableParagraphs">创建表格的段落集合（可使用 <see cref="OfficeHelper.BuildParagraph(XWPFTableCell, string, string, int, bool, Action{XWPFRun})"/>）。</param>
        public virtual void CreateWord(string filename, Func<XWPFDocument, XWPFTable> createTable, Func<XWPFDocument, XWPFTable, XWPFParagraph[]> createTableParagraphs)
        {
            using (var fs = File.OpenWrite(filename))
            {
                CreateWord(fs, createTable, createTableParagraphs);
            }
        }
        /// <summary>
        /// 创建 WORD 格式文档。
        /// </summary>
        /// <param name="s">给定的文档流。</param>
        /// <param name="createTable">创建表格的方法。</param>
        /// <param name="createTableParagraphs">创建表格的段落集合（可使用 <see cref="OfficeHelper.BuildParagraph(XWPFTableCell, string, string, int, bool, Action{XWPFRun})"/>）。</param>
        public virtual void CreateWord(Stream s, Func<XWPFDocument, XWPFTable> createTable, Func<XWPFDocument, XWPFTable, XWPFParagraph[]> createTableParagraphs)
        {
            var doc = new XWPFDocument();

            // 创建表格
            var table = createTable?.Invoke(doc);

            // 取得段落集合
            var paragraphs = createTableParagraphs?.Invoke(doc, table);

            // 写入流
            doc.Write(s);
        }


        /// <summary>
        /// 创建 EXCEL 格式文档。
        /// </summary>
        /// <param name="sheetname">给定的工作表名。</param>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="action">给定的工作表操作方法。</param>
        public virtual void CreateExcel(string sheetname, string filename, Action<ISheet> action)
        {
            // 创建工作手册
            var workbook = new HSSFWorkbook();

            // 创建工作表
            var sheet = workbook.CreateSheet(sheetname);
            action?.Invoke(sheet);

            // 创建新文件
            using (var fs = File.OpenWrite(filename))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 导出 EXCEL 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="table">给定的数据表格。</param>
        /// <param name="title">给定的表格标题（可选；默认为工作薄名）。</param>
        /// <param name="sheetname">给定的工作薄名（可选；默认为数据表格名）。</param>
        /// <param name="infoAction">设置摘要信息的操作方法。</param>
        /// <param name="encoding">给定的字符编码（可选；默认为 Encoding.UTF8）。</param>
        public virtual void ExportExcel(string filename, DataTable table,
            string title = StringUtility.Empty, string sheetname = StringUtility.Empty,
            Action<SummaryInformation> infoAction = null, Encoding encoding = null)
        {
            var workbook = OfficeHelper.BuildWorkbook(table, title, sheetname, infoAction, encoding);

            using (var fs = File.OpenWrite(filename))
            {
                workbook.Write(fs);
            }
        }

        /// <summary>
        /// 下载 EXCEL 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="table">给定的数据表格。</param>
        /// <param name="title">给定的表格标题（可选；默认为工作薄名）。</param>
        /// <param name="sheetname">给定的工作薄名（可选；默认为数据表格名）。</param>
        /// <param name="infoAction">设置摘要信息的操作方法。</param>
        /// <param name="encoding">给定的字符编码（可选；默认为 Encoding.UTF8）。</param>
        public virtual void DownloadExcel(string filename, DataTable table,
            string title = StringUtility.Empty, string sheetname = StringUtility.Empty,
            Action<SummaryInformation> infoAction = null, Encoding encoding = null)
        {
            if (filename.Contains("\\") || filename.Contains("/"))
                filename = Path.GetFileName(filename);

            var workbook = OfficeHelper.BuildWorkbook(table, title, sheetname, infoAction, encoding);
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Buffer = true;
            response.Charset = encoding.BodyName;
            response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
            response.ContentEncoding = encoding;
            response.ContentType = "application/vnd.ms-excel; charset=" + encoding.BodyName;
            workbook.Write(response.OutputStream);
            response.End();
        }

    }
}
