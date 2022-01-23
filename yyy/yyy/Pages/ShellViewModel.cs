using System;
using System.Collections.ObjectModel;
using System.Data;
using Stylet;
using yyy.Models;
using yyy.Pages.ExeclExport;

namespace yyy.Pages
{
    public class ShellViewModel : Conductor<IScreen>
    {
        ExcelImport excelImport;
        public ExeclExportViewModel execlExportViewModel { get; set; }
        int index = 0;
        int Max = 101;
        bool isStart = true;
        public String StartLabel { get; set; } = "开始计时";

        public String OilLabel { get; set; } = "0 %";

        int testTime = 56;
        public ObservableCollection<PressItem> PressList { get; set; } = new ObservableCollection<PressItem>();
        public int TestTime
        {
            get => testTime; set
            {
                testTime = value;
            }
        }

        DataTable ans;
        public ShellViewModel(IWindowManager windowManager, ExcelImport _excelImport, ExeclExportViewModel _execlExportViewModel)
        {
            excelImport = _excelImport;
            execlExportViewModel = _execlExportViewModel;
            excelImport.LoadFile();
            excelImport.BuildDataTable();

            ans = excelImport.GetDataTable().Clone();


        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer testTimer = new System.Windows.Threading.DispatcherTimer();


        public void Go()
        {
            if (isStart)
            {
                Start();
                StartLabel = "停止";
                isStart = false;
            }
            else
            {
                Stop();
                StartLabel = "开始计时";
                isStart = true;
            }
        }
        void Stop()
        {
            dispatcherTimer.Stop();
            testTimer.Stop();
            execlExportViewModel.SetDataTable(ans);
        }
        void Start()
        {
            // Tick 超过计时器间隔时发生。
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            // Interval 获取或设置计时器刻度之间的时间段
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            dispatcherTimer.Start();

            testTimer.Tick += new EventHandler(TestTick);
            testTimer.Interval = new TimeSpan(0, 0, testTime);
            testTimer.Start();
        }

        void TestTick(object sender, EventArgs e)
        {
            Random rd = new Random();
            OilLabel = String.Format("{0} %", rd.Next(2, 5));
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)//计时执行的程序
        {
            DataTable table = excelImport.GetDataTable();
            TurnPress(table.Rows[index % Max]);
            DataRow row = ans.NewRow();
            for (int i = 1; i < table.Rows[index % Max].ItemArray.Length; i++)
            {
                row[i] = table.Rows[index % Max][i];
            }
            row[0] = index * 15;
            ans.Rows.Add(row);
            index += 1;

        }

        public void TurnPress(DataRow row)
        {
            PressList.Clear();
            for (int i = 1; i < row.ItemArray.Length; i++)
            {
                PressItem item = new PressItem();
                item.Value = row.ItemArray[i].ToString();
                item.Name = String.Format("传感器 {0}", i);
                PressList.Add(item);
            }
        }
    }

    public class PressItem
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
