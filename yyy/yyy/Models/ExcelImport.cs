using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace yyy.Models
{
    public class ExcelImport
    {
        Stylet.IWindowManager windowManager;
        public string FilePath { get; set; }
        public string Head { get; set; }
        public string FristRow { get; set; }

        public int HeadIndex { get; set; } = 0;

        public int FristRowIndex { get; set; } = 1;

        public DataTable table { get; set; }

        IWorkbook wb;

        public ExcelImport(Stylet.IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public DataTable GetDataTable()
        {
            return table;
        }

        public void SetHeadIndexAndFristIndex(int h, int F)
        {
            HeadIndex = h;
            FristRowIndex = F;
        }

        public void LoadFile()
        {
            FilePath = "data.xlsx";
            try
            {
                wb = WorkbookFactory.Create(FilePath);
            }
            catch
            {
                var warning = String.Format("这个文件：{0}，已被占用", FilePath);
                System.Windows.MessageBox.Show(warning);
                return;
            }
            var sheet = wb.GetSheetAt(0);
            Head = RowToString(sheet.GetRow(HeadIndex));
            FristRow = RowToString(sheet.GetRow(FristRowIndex));
        }

        private string RowToString(IRow row)
        {
            string rowString = "";
            if (row != null)
            {
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    string value;
                    try
                    {
                        var varItem = row.GetCell(j);
                        if (varItem != null)
                            value = varItem.ToString();
                        else
                            value = "";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        value = "";
                    }
                    rowString += value ?? "";
                    rowString += "|";
                }
            }
            return rowString;
        }

        public void ReduceHeadIndex()
        {
            try
            {
                if (HeadIndex == 0)
                    return;
                HeadIndex--;
                var sheet = wb.GetSheetAt(0);
                Head = RowToString(sheet.GetRow(HeadIndex));
            }
            catch (Exception ex)
            {
                windowManager.ShowMessageBox(ex.Message);
            }

        }

        public void UpHeadIndex()
        {
            try
            {
                var sheet = wb.GetSheetAt(0);
                HeadIndex++;
                Head = RowToString(sheet.GetRow(HeadIndex));
            }
            catch (Exception ex)
            {
                windowManager.ShowMessageBox(ex.Message);
            }
        }

        public void ReduceFristRowIndex()
        {
            try
            {
                if (FristRowIndex == 0)
                    return;
                FristRowIndex--;
                var sheet = wb.GetSheetAt(0);
                FristRow = RowToString(sheet.GetRow(FristRowIndex));
            }
            catch (Exception ex)
            {
                windowManager.ShowMessageBox(ex.Message);
            }


        }

        public void UpFristRowIndex()
        {
            try
            {
                var sheet = wb.GetSheetAt(0);
                FristRowIndex++;
                FristRow = RowToString(sheet.GetRow(FristRowIndex));
            }
            catch (Exception ex)
            {
                windowManager.ShowMessageBox(ex.Message);
            }
        }

        public void BuildDataTable()
        {
            try
            {
                table = new DataTable();
                var sheet = wb.GetSheetAt(0);
                BuildHead(sheet, table);
                BuildBody(sheet, table);
            }
            catch (Exception ex)
            {
                windowManager.ShowMessageBox("未导入文件");
            }
        }

        void BuildHead(ISheet sheet, DataTable dataTable)
        {
            var row = sheet.GetRow(HeadIndex);
            if (row != null)
            {
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    string value;
                    try
                    {
                        var varItem = row.GetCell(j);
                        if (varItem != null)
                            value = varItem.ToString();
                        else
                            value = "";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        value = "";
                    }
                    var col = new DataColumn(value);
                    dataTable.Columns.Add(col);
                }
            }
        }

        void BuildBody(ISheet sheet, DataTable dataTable)
        {
            for (int i = FristRowIndex; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row != null)
                {
                    var dtRow = dataTable.NewRow();
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        string value;
                        try
                        {
                            var varItem = row.GetCell(j);
                            if (varItem != null)
                                value = varItem.ToString();
                            else
                                value = "";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            value = "";
                        }

                        dtRow[j] = value;
                    }
                    dataTable.Rows.Add(dtRow);
                }
            }
        }
    }
}
