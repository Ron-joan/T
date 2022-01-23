using System;
using System.Data;
using Stylet;
using www.Pages.ExeclExport;

namespace www.Pages
{
    public class ShellViewModel : Screen
    {
        public ExeclExportViewModel execlExportViewModel { get; set; }
        public String StartLabel { get; set; } = "开始计时";

        bool isStart = true;

        public string oneValue { get; set; }

        int oneIndex = 0;

        int twoIndex = 0;

        public int oneTime { get; set; } = 56;


        public string twoValue { get; set; }

        public int twoTime { get; set; } = 110;

        System.Windows.Threading.DispatcherTimer oneDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer twoDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        DataTable ans = new DataTable();

        public ShellViewModel(IWindowManager windowManager, ExeclExportViewModel _execlExportViewModel)
        {
            execlExportViewModel = _execlExportViewModel;
            ans.Columns.Add(new DataColumn("甲烷"));
            ans.Columns.Add(new DataColumn("硫化氢"));
        }

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
            oneDispatcherTimer.Stop();
            twoDispatcherTimer.Stop();
            execlExportViewModel.SetDataTable(ans);
        }

        void Start()
        {
            StartOne(oneDispatcherTimer, oneTime, new EventHandler(OneTick));
            StartOne(twoDispatcherTimer, twoTime, new EventHandler(TwoTick));
        }

        void OneTick(object sender, EventArgs e)
        {
            Random rd = new Random();
            oneValue = String.Format("{0} %", rd.Next(2, 15));
            int index = oneIndex;
            if(index >= ans.Rows.Count)
            {
                DataRow dataRow = ans.NewRow();
                dataRow[0] = oneValue;
                ans.Rows.Add(dataRow);
                
            }
            else
            {
                ans.Rows[index][0] = oneValue;
            }
            oneIndex += 1;
        }

        void TwoTick(object sender, EventArgs e)
        {
            Random rd = new Random();
            twoValue = String.Format("{0} %", rd.Next(2, 25));
            int index = twoIndex;
            if (index >= ans.Rows.Count)
            {
                DataRow dataRow = ans.NewRow();
                dataRow[1] = twoValue;
                ans.Rows.Add(dataRow);

            }
            else
            {
                ans.Rows[index][1] = twoValue;
            }
            twoIndex += 1;
        }

        void StartOne(System.Windows.Threading.DispatcherTimer testTimer,int testTime, EventHandler eventHandler)
        {
            testTimer.Tick += eventHandler;
            testTimer.Interval = new TimeSpan(0, 0, testTime);
            testTimer.Start();
        }
    }
}
