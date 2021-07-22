using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.Services.Test.Helpers
{
    public class ExcelError
    {
        public int Line { get; set; }

        public int ErrorCode { get; set; }

        public bool IsFatalError { get; set; }
    }

    public static class StreamHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream StringToStream(string str)
        {
            byte[] content = Encoding.Default.GetBytes(str);
            Stream stream = new MemoryStream(content);
            return stream;
        }

        /// <summary> 
        /// 从文件读取 Stream 
        /// </summary> 
        public static Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary> 
        /// 将 Stream 写入文件 
        /// </summary> 
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
    }

    public class ExcelReader
    {
        public static IList<(int lineIndex, int errorCode)> ReadExcel(Stream excelFileStream,
            Func<int, IList<string>, ExcelError> rowHandler)
        {
            using (var document = SpreadsheetDocument.Open(excelFileStream, false))
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

        public static IList<IList<string>> ReadTables(Stream excelFileStream,int sheetIndex = 0)
        {
            using (var document = SpreadsheetDocument.Open(excelFileStream, false))
            {
                var workbookPart = document.WorkbookPart;
                var worksheetParts = workbookPart.WorksheetParts.ToList();
                var worksheetPart = worksheetParts[sheetIndex];
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

        public static byte[] ConvertTableToExcelBytes(List<List<string>> table, bool addFilter = false, Columns cols = null)
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
