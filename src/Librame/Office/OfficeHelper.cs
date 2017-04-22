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
using NPOI.SS.Util;
using NPOI.XWPF.UserModel;
using System;
using System.Data;
using System.Text;

namespace Librame.Office
{
    using Utility;

    /// <summary>
    /// 办公助手。
    /// </summary>
    public class OfficeHelper
    {
        private const string FONT_FAMILY = "宋体";
        private const int FONT_SIZE = 18;
        private const bool FONT_IS_BOLD = true;


        /// <summary>
        /// 建立段落。
        /// </summary>
        /// <param name="doc">给定的 <see cref="XWPFDocument"/>。</param>
        /// <param name="text">给定的文本内容。</param>
        /// <param name="fontFamily">给定的字族。</param>
        /// <param name="fontSize">给定的字体大小。</param>
        /// <param name="isBold">是否加粗显示。</param>
        /// <param name="options">自定义操作方法。</param>
        /// <returns>返回 <see cref="XWPFParagraph"/>。</returns>
        public static XWPFParagraph BuildParagraph(XWPFDocument doc, string text, string fontFamily = FONT_FAMILY, int fontSize = FONT_SIZE, bool isBold = FONT_IS_BOLD, Action<XWPFRun> options = null)
        {
            var p = doc.CreateParagraph();
            p.Alignment = ParagraphAlignment.LEFT;

            var r = p.CreateRun();
            r.FontFamily = fontFamily;
            r.FontSize = fontSize;
            r.IsBold = isBold;
            r.SetText(text);

            options?.Invoke(r);

            return p;
        }

        /// <summary>
        /// 建立段落。
        /// </summary>
        /// <param name="cell">给定的 <see cref="XWPFTableCell"/>。</param>
        /// <param name="text">给定的文本内容。</param>
        /// <param name="fontFamily">给定的字族。</param>
        /// <param name="fontSize">给定的字体大小。</param>
        /// <param name="isBold">是否加粗显示。</param>
        /// <param name="options">自定义操作方法。</param>
        /// <returns>返回 <see cref="XWPFParagraph"/>。</returns>
        public static XWPFParagraph BuildParagraph(XWPFTableCell cell, string text, string fontFamily = FONT_FAMILY, int fontSize = FONT_SIZE, bool isBold = FONT_IS_BOLD, Action<XWPFRun> options = null)
        {
            var p = cell.AddParagraph();
            p.Alignment = ParagraphAlignment.LEFT;

            var r = p.CreateRun();
            r.FontFamily = fontFamily;
            r.FontSize = fontSize;
            r.IsBold = isBold;
            r.SetText(text);

            options?.Invoke(r);

            return p;
        }


        /// <summary>
        /// 构建工作手册。
        /// </summary>
        /// <param name="table">给定的数据表格。</param>
        /// <param name="title">给定的表格标题（可选；默认为工作薄名）。</param>
        /// <param name="sheetname">给定的工作薄名（可选；默认为数据表格名）。</param>
        /// <param name="infoAction">设置摘要信息的操作方法。</param>
        /// <param name="encoding">给定的字符编码（可选；默认为 Encoding.UTF8）。</param>
        /// <returns>返回 <see cref="HSSFWorkbook"/>。</returns>
        public static HSSFWorkbook BuildWorkbook(DataTable table,
            string title = StringUtility.Empty, string sheetname = StringUtility.Empty,
            Action<SummaryInformation> infoAction = null, Encoding encoding = null)
        {
            // 工作薄名
            if (string.IsNullOrEmpty(sheetname))
            {
                sheetname = (string.IsNullOrWhiteSpace(table.TableName) ?
                    "Sheet1" : table.TableName);
            }

            // 表格标题
            if (string.IsNullOrEmpty(title))
            {
                title = sheetname;
            }

            // 字符编码
            if (ReferenceEquals(encoding, null))
            {
                encoding = Encoding.UTF8;
            }

            var workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetname);

            #region SummaryInformation
            {
                var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                var si = PropertySetFactory.CreateSummaryInformation();

                if (ReferenceEquals(infoAction, null))
                {
                    si.Author = LibrameAssemblyConstants.AUTHOR;
                    si.ApplicationName = LibrameAssemblyConstants.NAME;
                    si.LastAuthor = LibrameAssemblyConstants.AUTHOR;
                    si.Comments = LibrameAssemblyConstants.DESCRIPTION;
                    si.Title = "Librame Office Adapter";
                    si.Subject = LibrameAssemblyConstants.NAME;
                    si.CreateDateTime = DateTime.Now;
                }
                else
                {
                    infoAction?.Invoke(si);
                }
                
                workbook.SummaryInformation = si;
            }
            #endregion

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            // 取得列宽
            var arrColWidth = new int[table.Columns.Count];
            foreach (DataColumn item in table.Columns)
            {
                // Encoding.GetEncoding(936)
                arrColWidth[item.Ordinal] = encoding.GetBytes(item.ColumnName.ToString()).Length;
            }
            for (var i = 0; i < table.Rows.Count; i++)
            {
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    // Encoding.GetEncoding(936)
                    int intTemp = encoding.GetBytes(table.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }

            int rowIndex = 0;
            foreach (DataRow row in table.Rows)
            {
                #region 表头 列头
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    {
                        var headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(title);

                        // CellStyle
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;// 左右居中    
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中 
                        // 设置单元格的背景颜色（单元格的样式会覆盖列或行的样式）    
                        headStyle.FillForegroundColor = (short)11;

                        // 定义font
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;

                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                    }
                    #endregion
                    
                    #region 列头及样式
                    {
                        var headerRow = sheet.CreateRow(1);

                        // CellStyle
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;// 左右居中
                        headStyle.VerticalAlignment = VerticalAlignment.Center;// 上下居中

                        // 定义font
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        foreach (DataColumn column in table.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    #endregion

                    if (title != "")
                    {
                        // header row
                        IRow row0 = sheet.CreateRow(0);
                        for (int i = 0; i < table.Columns.Count; i++)
                        {
                            var cell = row0.CreateCell(i, CellType.String);
                            cell.SetCellValue(title); // table.Columns[i].ColumnName
                        }
                    }

                    rowIndex = 2;
                }
                #endregion


                #region 内容

                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in table.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = dateStyle;//格式化显示
                            break;

                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;

                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;

                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;

                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;

                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }

            // 自动列宽
            for (int i = 0; i <= table.Columns.Count; i++)
                sheet.AutoSizeColumn(i, true);

            return workbook;
        }

    }
}
