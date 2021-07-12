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

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Helpers
{
    public class ExcelHelper
    {
        public static byte[] ConvertTableToExcelBytes(List<ExcelSheet> excelSheets)
        {
            using (var stream = new MemoryStream())
            {
                using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = document.AddWorkbookPart();
                    var workbook = new Workbook();
                    workbook.AddNamespaceDeclaration(@"r", @"http://schemas.openxmlformats.org/officeDocument/2006/relationships");
                    workbookPart.Workbook = workbook;
                    foreach (var sheet in excelSheets)
                    {
                        var table = sheet.Table;
                        var sheetData = new SheetData();
                        var rowIndex = -1;
                        foreach (var rowData in table)
                        {
                            var row = new Row { RowIndex = (uint)++rowIndex + 1 };
                            var cellIndex = 0;
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
                        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        var worksheet = new Worksheet(sheetData);
                        worksheetPart.Worksheet = worksheet;
                        if (sheet.Columns != null)
                        {
                            worksheetPart.Worksheet.Append(sheet.Columns);
                        }

                        if (sheet.AddFilter)
                        {
                            AutoFilter autoFilter = new AutoFilter { Reference = FormattableString.Invariant($@"A1:{(char)('A' + sheet.Table.First().Count - 1)}1") };
                            worksheetPart.Worksheet.Append(autoFilter);
                        }
                        worksheet.Save();
                        sheet.MergeCells?.ForEach(item =>
                        {
                            MergeCells(worksheetPart.Worksheet, item.startCell, item.endCell);
                        });

                        UInt32 sheetId;
                        if (workbookPart.Workbook.Sheets == null)
                        {
                            workbookPart.Workbook.AppendChild(new Sheets());
                            sheetId = 1;
                        }
                        else
                        {
                            sheetId = Convert.ToUInt32(workbookPart.Workbook.Sheets.Count() + 1);
                        }

                        workbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            Name = $@"{sheet.SheetName}",
                            SheetId = sheetId,
                        });
                    }

                    workbook.Save();
                }
                stream.Position = 0;
                return StreamToBytes(stream);
            }
        }

        public static string GetCellReference(int rowIndex, int columnIndex)
        {
            var sb = new StringBuilder();
            do
            {
                columnIndex--;
                var mod = columnIndex % 26;
                sb.Insert(0, (char)('A' + mod));
                columnIndex /= 26;
            } while (columnIndex > 0);
            sb.Append(rowIndex + 1);
            return sb.ToString();
        }

        private static string EncodeCell(string cellData)
        {
            if (cellData == null)
                cellData = "";
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

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        // Given a document name, a worksheet name, and the names of two adjacent cells, merges the two cells.
        // When two cells are merged, only the content from one cell is preserved:
        // the upper-left cell for left-to-right languages or the upper-right cell for right-to-left languages.
        private static void MergeCells(Worksheet worksheet, string cell1Name, string cell2Name)
        {
            // Open the document for editing.
            // Verify if the specified cells exist, and if they do not exist, create them.
            MergeCells mergeCells;
            if (worksheet.Elements<MergeCells>().Count() > 0)
            {
                mergeCells = worksheet.Elements<MergeCells>().First();
            }
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                }
                else if (worksheet.Elements<DataConsolidate>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<DataConsolidate>().First());
                }
                else if (worksheet.Elements<SortState>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SortState>().First());
                }
                else if (worksheet.Elements<AutoFilter>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<AutoFilter>().First());
                }
                else if (worksheet.Elements<Scenarios>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<Scenarios>().First());
                }
                else if (worksheet.Elements<ProtectedRanges>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<ProtectedRanges>().First());
                }
                else if (worksheet.Elements<SheetProtection>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetProtection>().First());
                }
                else if (worksheet.Elements<SheetCalculationProperties>().Count() > 0)
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetCalculationProperties>().First());
                }
                else
                {
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
                }
            }

            // Create the merged cell and append it to the MergeCells collection.

            string s1 = cell1Name + ":" + cell2Name;
            MergeCell mergeCell = new MergeCell() { Reference = s1 };
            mergeCells.Append(mergeCell);
            worksheet.Save();
        }
    }
    /// <summary>
    /// Excel sheet 设置
    /// </summary>
    public class ExcelSheet
    {
        /// <summary>
        /// sheet 名称。
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// sheet 所有内容
        /// </summary>
        public List<List<string>> Table { get; set; }
        /// <summary>
        /// 合并单元格列表
        /// </summary>
        public List<(string startCell, string endCell)> MergeCells { get; set; }
        public bool AddFilter { get; set; } = false;
        public Columns Columns { get; set; }
    }

}
