using NPOI.SS.UserModel;
using Stylet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace www.Pages.ExeclExport
{
    public class ExeclExportViewModel : Stylet.Screen
    {
        private OpenFileDialog openFileDialog;

        IWindowManager windowManager;
        public string FilePath { get; set; }

        public string FileName { get; set; } = "导出.xls";

        public DataTable table { get; set; }
        public ExeclExportViewModel(IWindowManager windowManager)
        {
            this.openFileDialog = new OpenFileDialog();
            this.windowManager = windowManager;
        }

        public void SetFileName(string fileName)
        {
            FileName = fileName;
        }

        public void SetDataTable(DataTable dataTable)
        {
            table = dataTable;
        }
        public void SaveFile()
        {
            if (table == null)
            {
                windowManager.ShowMessageBox("未导入文件");
                return;
            }

            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            string m_Dir = m_Dialog.SelectedPath.Trim();

            if (string.IsNullOrEmpty(m_Dir))
                return;

            FilePath = m_Dir + "\\" + FileName;
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            DateTableToSheet(book.CreateSheet(FileName), table);


            // 写入到客户端  
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                book.Write(ms);
                using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }

            windowManager.ShowMessageBox("生成成功");
        }

        public ISheet DateTableToSheet(ISheet sheet, DataTable dataTable)
        {
            BuildHead(sheet, dataTable);
            BuildBody(sheet, dataTable);
            return sheet;
        }
        void BuildHead(ISheet sheet, DataTable dataTable)
        {
            IRow row = sheet.CreateRow(0);
            int i = 0;
            foreach (DataColumn item in dataTable.Columns)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(item.ColumnName);
                i++;
            }
        }


        void BuildBody(ISheet sheet, DataTable dataTable)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(dataTable.Rows[i].ItemArray[j].ToString());
                }
            }
        }


    }
}
