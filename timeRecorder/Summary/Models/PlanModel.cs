using Summary.Common;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Summary.Models
{
    public class PlanModel : ViewModelBase
    {
        public DataGrid TimeGrid { get; set; }
        public DateTime startTime {  get; set; } = new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged(); }
        }
        
        public DateTime endTime { get; set; } = DateTime.ParseExact(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1);
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; OnPropertyChanged(); }
        }
        
        public MyCommand ClickOkButtonCommand { get; set; }
        private ObservableCollection<TaskView> timeObjs = new ObservableCollection<TaskView>();

        public ObservableCollection<TaskView> TimeObjs
        {
            get { return timeObjs; }
            set { timeObjs = value; OnPropertyChanged(); }
        }
        public ISQLCommands SQLCommands { get; set; }
        public PlanModel(ISQLCommands SqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            SQLCommands = SqlCommands;
            //TimeObjs = new ObservableCollection<TaskView>();
            //var task = new TaskView()
            //{
            //    taskId = 0,
            //    taskName = "123taskname",
            //    taskDate = DateTime.Now,
            //    lastTime = new TimeSpan(0, 20, 0)
            //};
            //TimeObjs.Add(task);
        }

        private async void clickOkButton(object a)
        {
            if (a!=null &&a.ToString() == "LastMonth")
            {
                StartTime = DateTime.ParseExact(EndTime.Year.ToString() + EndTime.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(-1);
                EndTime = StartTime.AddMonths(1).AddDays(-1);
            }
            if (a!=null &&a.ToString() == "NextMonth")
            {
                StartTime = DateTime.ParseExact(EndTime.Year.ToString() + EndTime.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(1);
                EndTime = DateTime.ParseExact(EndTime.Year.ToString() + EndTime.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(2).AddDays(-1);
            }
            if (a != null && a.ToString() == "ThisMonth")
            {
                StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                EndTime = DateTime.ParseExact(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(1).AddDays(-1);
            }
            await BuildData(startTime, endTime);

        }

        private async Task BuildData(DateTime startTime, DateTime endTime)
        {
            List<TaskView> taskViews = await SQLCommands.GetTask(startTime,endTime);
            foreach(TaskView taskView in taskViews)
            {
                TimeObjs.Add(taskView);
            }
        }

        public void init()
        {
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "日期",
                IsReadOnly = true,
                Binding = new Binding("taskDate")
                {
                    StringFormat = "yyyy/MM/dd"
                }
            });
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "星期",
                IsReadOnly = true,
                Binding = new Binding("DayOfWeek")
                {
                    StringFormat = "yyyy/MM/dd HH:mm:ss"
                }
            });
        }

    }

}
