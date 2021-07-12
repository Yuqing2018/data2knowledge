using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicKG.Utility
{
    public class ExcelError
    {
        public int Line { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFatalError { get; set; }
    }

    public class ExcelCellWithType
    {
        public string Value { get; set; }
        public UInt32Value ExcelCellFormat { get; set; }
        public bool IsDateTimeType { get; set; }
    }

    public class ExcelHelper
    {
        public static IList<(int lineIndex, int errorCode)> ReadExcel(Stream excelFileStream,
            Func<int, IList<string>, ExcelError> rowHandler)
        {
            var openSettings = new OpenSettings()
            {
                RelationshipErrorHandlerFactory = RelationshipErrorHandler.CreateRewriterFactory(Rewriter)
            };

            var bytes = new byte[excelFileStream.Length];

            excelFileStream.Read(bytes, 0, bytes.Length);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (var document = SpreadsheetDocument.Open(stream, true, openSettings))
                {
                    var workbookPart = document.WorkbookPart;
                    var worksheetPart = workbookPart.WorksheetParts.First();
                    var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    var sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    var sst = sstpart?.SharedStringTable;

                    var table = new List<IList<string>>();
                    int index = 1;

                    var errors = new List<(int lineIndex, int errorCode)>();

                    foreach (var row in sheetData.Elements<Row>())
                    {
                        var rowAsList = new List<string>();
                        var lastColumnIndex = -1;
                        foreach (var cell in row.Elements<Cell>())
                        {
                            // insert empty cells
                            var currentColumnIndex = GetColumnIndex(cell.CellReference.Value);
                            while (lastColumnIndex + 1 < currentColumnIndex)
                            {
                                rowAsList.Add(string.Empty);
                                lastColumnIndex++;
                            }
                            lastColumnIndex = currentColumnIndex;
                            string cellValue;
                            if (cell.DataType != null && cell.DataType == CellValues.SharedString && sst != null)
                            {
                                var ssid = int.Parse(cell.CellValue.Text);
                                cellValue = sst.Elements<SharedStringItem>().ElementAt(ssid)?.InnerText;
                            }
                            else
                            {
                                var cellWithType = ReadExcelCell(cell, workbookPart);
                                if (cellWithType.IsDateTimeType)
                                    cellValue = cellWithType.Value;
                                else
                                    cellValue = cell.CellFormula != null
                                        ? cell.CellFormula.Text
                                        : cell.CellValue == null ? cell.InnerText : cell.CellValue.Text;
                            }
                            rowAsList.Add(ReplaceSpecialChar(cellValue?.Trim() ?? string.Empty));
                        }
                        TrimEnd(rowAsList);
                        //Ignore empty line.
                        if (rowAsList != null && rowAsList.Count > 0)
                        {
                            var error = rowHandler(index, rowAsList);
                            if (error != null)
                            {
                                errors.Add((index, error.ErrorCode));
                                if (error.IsFatalError)
                                {
                                    break;
                                }
                            }
                        }
                        index += 1;
                    }

                    return errors;
                }
            }
        }

        public static string Rewriter(Uri partUri, string id, string uri)
            => $"http://unknown";

        private static ExcelCellWithType ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if (cell.DataType?.Value == CellValues.SharedString)
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            var cellText = (text ?? string.Empty).Trim();

            var cellWithType = new ExcelCellWithType();

            if (cell.StyleIndex != null)
            {
                var cellFormat = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ChildElements[
                    int.Parse(cell.StyleIndex.InnerText)] as CellFormat;

                if (cellFormat != null)
                {
                    cellWithType.ExcelCellFormat = cellFormat.NumberFormatId;

                    var dateFormat = GetDateTimeFormat(cellFormat.NumberFormatId);
                    if (!string.IsNullOrEmpty(dateFormat))
                    {
                        cellWithType.IsDateTimeType = true;

                        if (!string.IsNullOrEmpty(cellText))
                        {
                            if (double.TryParse(cellText, out var cellDouble))
                            {
                                var theDate = DateTime.FromOADate(cellDouble);
                                cellText = theDate.ToString("yyyy-MM-dd");
                            }
                        }
                    }
                }
            }

            cellWithType.Value = cellText;

            return cellWithType;
        }

        private static readonly Dictionary<uint, string> DateFormatDictionary = new Dictionary<uint, string>()
        {
            [14] = "dd/MM/yyyy",
            [15] = "d-MMM-yy",
            [16] = "d-MMM",
            [17] = "MMM-yy",
            [18] = "h:mm AM/PM",
            [19] = "h:mm:ss AM/PM",
            [20] = "h:mm",
            [21] = "h:mm:ss",
            [22] = "M/d/yy h:mm",
            [30] = "M/d/yy",
            [34] = "yyyy-MM-dd",
            [45] = "mm:ss",
            [46] = "[h]:mm:ss",
            [47] = "mmss.0",
            [51] = "MM-dd",
            [52] = "yyyy-MM-dd",
            [53] = "yyyy-MM-dd",
            [55] = "yyyy-MM-dd",
            [56] = "yyyy-MM-dd",
            [58] = "MM-dd",
            [165] = "M/d/yy",
            [166] = "dd MMMM yyyy",
            [167] = "dd/MM/yyyy",
            [168] = "dd/MM/yy",
            [169] = "d.M.yy",
            [170] = "yyyy-MM-dd",
            [171] = "dd MMMM yyyy",
            [172] = "d MMMM yyyy",
            [173] = "M/d",
            [174] = "M/d/yy",
            [175] = "MM/dd/yy",
            [176] = "d-MMM",
            [177] = "d-MMM-yy",
            [178] = "dd-MMM-yy",
            [179] = "MMM-yy",
            [180] = "MMMM-yy",
            [181] = "MMMM d, yyyy",
            [182] = "M/d/yy hh:mm t",
            [183] = "M/d/y HH:mm",
            [184] = "MMM",
            [185] = "MMM-dd",
            [186] = "M/d/yyyy",
            [187] = "d-MMM-yyyy"
        };

        private static string GetDateTimeFormat(UInt32Value numberFormatId)
        {
            return DateFormatDictionary.ContainsKey(numberFormatId) ? DateFormatDictionary[numberFormatId] : string.Empty;
        }

        public static void TrimEnd(IList<string> row)
        {
            for (int i = row.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(row[i]))
                {
                    row.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        public static bool TryReadTable(Stream excelFileStream, out IList<IList<string>> table)
        {
            try
            {
                table = ReadTables(excelFileStream);
            }
            catch (Exception)
            {
                throw;
            }
            return table != null;
        }

        public static IList<IList<string>> ReadTables(Stream excelFileStream)
        {
            using (var document = SpreadsheetDocument.Open(excelFileStream, false))
            {
                var workbookPart = document.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();
                var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                var sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                var sst = sstpart?.SharedStringTable;
                var table = new List<IList<string>>();

                foreach (var row in sheetData.Elements<Row>())
                {
                    var rowAsList = new List<string>();
                    var lastColumnIndex = -1;
                    foreach (var cell in row.Elements<Cell>())
                    {
                        // insert empty cells
                        var currentColumnIndex = GetColumnIndex(cell.CellReference.Value);
                        while (lastColumnIndex + 1 < currentColumnIndex)
                        {
                            rowAsList.Add(string.Empty);
                            lastColumnIndex++;
                        }
                        lastColumnIndex = currentColumnIndex;
                        string cellValue;
                        if (cell.DataType != null && cell.DataType == CellValues.SharedString && sst != null)
                        {
                            var ssid = int.Parse(cell.CellValue.Text);
                            cellValue = sst.Elements<SharedStringItem>().ElementAt(ssid)?.InnerText;
                        }
                        else
                        {
                            cellValue = cell.CellFormula != null
                                ? cell.CellFormula.Text
                                : cell.CellValue == null ? cell.InnerText : cell.CellValue.Text;
                        }
                        rowAsList.Add(ReplaceSpecialChar(cellValue?.Trim() ?? string.Empty));
                    }
                    // Ignore empty line...
                    if (rowAsList.Any(r => !string.IsNullOrWhiteSpace(r)))
                    {
                        table.Add(rowAsList);
                    }
                }

                return table;
            }
        }

        private static string ReplaceSpecialChar(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return stringValue;
            }
            else
            {
                return stringValue.Replace("_x000d_", string.Empty).Replace("_x000D_", string.Empty);
            }
        }

        public static byte[] ConvertTableToExcelBytes(IList<IList<string>> table, bool addFilter = false, Columns cols = null)
        {
            using (var stream = new MemoryStream())
            {
                using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    var sheetData = new SheetData();

                    var rowIndex = -1;
                    foreach (var rowData in table)
                    {
                        var row = new Row { RowIndex = (uint)++rowIndex + 1 };
                        var cellIndex = -1;
                        foreach (var cellData in rowData)
                        {
                            row.AppendChild(new Cell(new InlineString(new Text(EncodeCell(cellData))))
                            {
                                DataType = CellValues.InlineString,
                                CellReference = GetCellReference(rowIndex, ++cellIndex)
                            });
                        }
                        sheetData.AppendChild(row);
                    }

                    var workbookPart = document.AddWorkbookPart();
                    var workbook = new Workbook();
                    workbook.AddNamespaceDeclaration(@"r", @"http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                    workbookPart.Workbook = workbook;
                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var worksheet = new Worksheet();
                    worksheetPart.Worksheet = worksheet;
                    if (cols != null)
                    {
                        worksheetPart.Worksheet.Append(cols);
                    }
                    worksheetPart.Worksheet.AppendChild(sheetData);
                    if (addFilter)
                    {
                        AutoFilter autoFilter = new AutoFilter { Reference = FormattableString.Invariant($@"A1:{(char)('A' + table.First().Count - 1)}1") };
                        worksheetPart.Worksheet.Append(autoFilter);
                    }
                    worksheet.Save();
                    workbook.AppendChild(new Sheets()).AppendChild(new Sheet
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        Name = @"Sheet1",
                        SheetId = 1,
                    });
                    workbook.Save();
                }

                stream.Position = 0;
                return StreamHelper.StreamToBytes(stream);
            }
        }

        private static string GetCellReference(int rowIndex, int columnIndex)
        {
            var sb = new StringBuilder();
            do
            {
                var mod = columnIndex % 26;
                sb.Insert(0, (char)('A' + mod));
                columnIndex /= 26;
            } while (columnIndex > 0);
            sb.Append(rowIndex + 1);
            return sb.ToString();
        }

        private static string EncodeCell(string cellData)
        {
            return Regex.Replace(cellData, @"[\x00-\x08\x0B\x0C\x0E-\x1F]", "");
        }

        public static int GetColumnIndex(string cellReference)
        {
            cellReference = cellReference.ToUpperInvariant();
            var index = 0;
            foreach (var ch in cellReference)
            {
                if (char.IsLetter(ch))
                {
                    index = index * 26 + (ch - 'A' + 1);
                }
                else
                {
                    break;
                }
            }
            return index - 1;
        }
    }
}
