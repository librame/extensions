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
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Librame.Office
{
    using Utility;

    /// <summary>
    /// 办公适配器接口。
    /// </summary>
    public interface IOfficeAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 创建 WORD 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="createTable">创建表格的方法。</param>
        /// <param name="createTableParagraphs">创建表格的段落集合（可使用 <see cref="OfficeHelper.BuildParagraph(XWPFTableCell, string, string, int, bool, Action{XWPFRun})"/>）。</param>
        void CreateWord(string filename, Func<XWPFDocument, XWPFTable> createTable, Func<XWPFDocument, XWPFTable, XWPFParagraph[]> createTableParagraphs);

        /// <summary>
        /// 创建 WORD 格式文档。
        /// </summary>
        /// <param name="s">给定的文档流。</param>
        /// <param name="createTable">创建表格的方法。</param>
        /// <param name="createTableParagraphs">创建表格的段落集合（可使用 <see cref="OfficeHelper.BuildParagraph(XWPFTableCell, string, string, int, bool, Action{XWPFRun})"/>）。</param>
        void CreateWord(Stream s, Func<XWPFDocument, XWPFTable> createTable, Func<XWPFDocument, XWPFTable, XWPFParagraph[]> createTableParagraphs);


        /// <summary>
        /// 创建 EXCEL 格式文档。
        /// </summary>
        /// <param name="sheetname">给定的工作表名。</param>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="action">给定的工作表操作方法。</param>
        void CreateExcel(string sheetname, string filename, Action<ISheet> action);

        /// <summary>
        /// 导出 EXCEL 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="table">给定的数据表格。</param>
        /// <param name="title">给定的表格标题（可选；默认为工作薄名）。</param>
        /// <param name="sheetname">给定的工作薄名（可选；默认为数据表格名）。</param>
        /// <param name="infoAction">设置摘要信息的操作方法。</param>
        /// <param name="encoding">给定的字符编码（可选；默认为 Encoding.UTF8）。</param>
        void ExportExcel(string filename, DataTable table,
            string title = StringUtility.Empty, string sheetname = StringUtility.Empty,
            Action<SummaryInformation> infoAction = null, Encoding encoding = null);

        /// <summary>
        /// 下载 EXCEL 格式文档。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="table">给定的数据表格。</param>
        /// <param name="title">给定的表格标题（可选；默认为工作薄名）。</param>
        /// <param name="sheetname">给定的工作薄名（可选；默认为数据表格名）。</param>
        /// <param name="infoAction">设置摘要信息的操作方法。</param>
        /// <param name="encoding">给定的字符编码（可选；默认为 Encoding.UTF8）。</param>
        void DownloadExcel(string filename, DataTable table,
            string title = StringUtility.Empty, string sheetname = StringUtility.Empty,
            Action<SummaryInformation> infoAction = null, Encoding encoding = null);

    }
}
