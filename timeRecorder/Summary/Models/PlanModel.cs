using Summary.Common;
using Summary.Common.Utils;
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
        public Dictionary<int, string> WorkThemes { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> SubWorkThemes { get; set; } = new Dictionary<int, string>();
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
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }
        public MyCommand ClickOkButtonCommand { get; set; }
        private ObservableCollection<DayTaskView> timeObjs = new ObservableCollection<DayTaskView>();

        public ObservableCollection<DayTaskView> TimeObjs
        {
            get { return timeObjs; }
            set { timeObjs = value; OnPropertyChanged(); }
        }
        public ISQLCommands SQLCommands { get; set; }
        public PlanModel(ISQLCommands SqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            SQLCommands = SqlCommands;
            WorkThemes.Add(1, "想做");
            WorkThemes.Add(7, "TimeRecorder");
            WorkThemes.Add(63, "弹幕");
            WorkThemes.Add(3, "玩");
            WorkThemes.Add(22, "浪费");
        }
        private void closeDialog()
        {
            IsDialogOpen=false;

        }
        private void openDialog()
        {
            IsDialogOpen=true;
        }
        private void AddSubCategories(int id)
        {
            if (!SubWorkThemes.Keys.Contains(id))
            {
                SubWorkThemes.Add(id, Helper.allcategories.First(x=>x.Id == id).Name);
                List<Category> list = Helper.allcategories.Where(x => x.ParentCategoryId == id)?.ToList();
                foreach(Category item in list)
                {
                    AddSubCategories(item.Id);
                }
            }
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
          
            AddColumnsToTable();
            await Task.Run(() => { openDialog(); }).ContinueWith(delegate { BuildData(startTime, endTime); closeDialog(); });
            TimeObjs = new ObservableCollection<DayTaskView>( TimeObjs.OrderBy(x => x.TaskDate));
        }
        private void initTimeObjs(DateTime startTime, DateTime endTime)
        {
            TimeObjs.Clear();
            for (int i = 0; i<(endTime-startTime).Days+1; i++)
            {
                var dayTaskView = new DayTaskView() { TaskDate = startTime.AddDays(i) };
                foreach (var value in WorkThemes.Values)
                {
                    dayTaskView.Background = i % 2;
                    if (string.IsNullOrEmpty(dayTaskView.Theme1))
                    {
                        dayTaskView.Theme1 = value;
                        continue;
                    }
                    if (string.IsNullOrEmpty(dayTaskView.Theme2))
                    {
                        dayTaskView.Theme2 = value;
                        continue;
                    }
                    if (string.IsNullOrEmpty(dayTaskView.Theme3))
                    {
                        dayTaskView.Theme3 = value;
                        continue;
                    }
                    if (string.IsNullOrEmpty(dayTaskView.Theme4))
                    {
                        dayTaskView.Theme4 = value;
                        continue;
                    }
                    if (string.IsNullOrEmpty(dayTaskView.Theme5))
                    {
                        dayTaskView.Theme5 = value;
                        continue;
                    }
                }
                TimeObjs.Add(dayTaskView);
            }
        }
        private async void BuildData(DateTime startTime, DateTime endTime)
        {
            TimeGrid.Dispatcher.Invoke(new Action(delegate
            {
                initTimeObjs(startTime, endTime);
            }));
            List<MyTime> AllTimeObjs = await SQLCommands.GetAllTimeObjs(startTime,endTime);
            List<ToDoObj> allToDoByDate = AllTimeObjs.Where(x=>x.type!= null&&x.type!="none"&&!string.IsNullOrEmpty(x.note)).OrderBy(x => x.createDate).GroupBy(x => new { x.note,x.createDate }).Select(x => new ToDoObj() { CreatedDate = x.Key.createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => (x.endTime-x.startTime).Ticks)), Id = x.First().taskId, Type = x.First().type }).ToList();
            foreach (int i in WorkThemes.Keys)
            {
                SubWorkThemes.Clear();
                AddSubCategories(i);
                foreach (ToDoObj recordedTask in allToDoByDate)
                {
                    recordedTask.Type = Helper.allcategories.FirstOrDefault(x=>x.Id == SQLCommands.QueryTodo(recordedTask.Id).CategoryId).Name;
                    if (SubWorkThemes.Values.Contains(recordedTask.Type))
                    {
                        recordedTask.Type = WorkThemes[i];
                       InsertTaskIntoDayTaskViews(recordedTask, TimeObjs);
                    }
                }
            }
        }

        private void InsertTaskIntoDayTaskViews(ToDoObj recordedTask, ObservableCollection<DayTaskView> TimeObjs)
        {
            bool insertSuccess = false;
            var DayTaskViews = TimeObjs.Where(x => x.TaskDate==recordedTask.CreatedDate);

            foreach (var DayLine in DayTaskViews)
            {
                if (DayLine.Theme1 == recordedTask.Type)
                {
                    if (string.IsNullOrEmpty(DayLine.Theme1TaskName))
                    {
                        DayLine.Theme1TaskName = recordedTask.Note;
                        DayLine.Theme1LastTime = recordedTask.LastTime.TotalHours.ToString("F2") ;
                        DayLine.Theme1Reward = recordedTask.Bonus.ToString();
                        insertSuccess = true;
                        break;
                    }
                    continue;
                }
                if (DayLine.Theme2 == recordedTask.Type)
                {
                    if (string.IsNullOrEmpty(DayLine.Theme2TaskName))
                    {
                        DayLine.Theme2TaskName = recordedTask.Note;
                        DayLine.Theme2LastTime = recordedTask.LastTime.TotalHours.ToString("F2");
                        DayLine.Theme2Reward = recordedTask.Bonus.ToString();
                        insertSuccess = true;
                        break;
                    }
                    continue;
                }
                if (DayLine.Theme3 == recordedTask.Type)
                {
                    if (string.IsNullOrEmpty(DayLine.Theme3TaskName))
                    {
                        DayLine.Theme3TaskName = recordedTask.Note;
                        DayLine.Theme3LastTime = recordedTask.LastTime.TotalHours.ToString("F2");
                        DayLine.Theme3Reward = recordedTask.Bonus.ToString();
                        insertSuccess = true;
                        break;
                    }
                    continue;
                }
                if (DayLine.Theme4 == recordedTask.Type)
                {
                    if (string.IsNullOrEmpty(DayLine.Theme4TaskName))
                    {
                        DayLine.Theme4TaskName = recordedTask.Note;
                        DayLine.Theme4LastTime = recordedTask.LastTime.TotalHours.ToString("F2");
                        DayLine.Theme4Reward = recordedTask.Bonus.ToString();
                        insertSuccess = true;
                        break;
                    }
                    continue;
                }
                if (DayLine.Theme5 == recordedTask.Type)
                {
                    if (string.IsNullOrEmpty(DayLine.Theme5TaskName))
                    {
                        DayLine.Theme5TaskName = recordedTask.Note;
                        DayLine.Theme5LastTime = recordedTask.LastTime.TotalHours.ToString("F2");
                        DayLine.Theme5Reward = recordedTask.Bonus.ToString();
                        insertSuccess = true;
                        break;
                    }
                    continue;
                }
            }
            if (!insertSuccess&&DayTaskViews!=null&&TimeObjs!=null)
            {
                TimeGrid.Dispatcher.Invoke(new Action(delegate
                {
                    TimeObjs.Add(new DayTaskView()
                    {
                        TaskDate = recordedTask.CreatedDate,
                        Theme1 = DayTaskViews.First().Theme1,
                        Theme2 = DayTaskViews.First().Theme2,
                        Theme3 = DayTaskViews.First().Theme3,
                        Theme4 = DayTaskViews.First().Theme4,
                        Theme5 = DayTaskViews.First().Theme5,
                        Background = DayTaskViews.First().Background
                    });
                }));
                
                InsertTaskIntoDayTaskViews(recordedTask, TimeObjs);
            }
        }

        public void AddColumnsToTable()
        {
            this.TimeGrid.Columns.Clear();
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "日期",
                IsReadOnly = true,
                Binding = new Binding("TaskDate")
                {
                    StringFormat = "yyyy/MM/dd"
                }
            });
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "星期",
                IsReadOnly = true,
                Binding = new Binding("DayOfWeek")
            });
            int i =0;
            foreach (var theme in WorkThemes)
            {
                i++;
                this.TimeGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = theme.Value,
                    IsReadOnly = true,
                    Binding = new Binding($"Theme{i}TaskName"),
                    
                });
                this.TimeGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = "时间",
                    IsReadOnly = true,
                    Binding = new Binding($"Theme{i}LastTime")
                    
                });
                this.TimeGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = "获得",
                    IsReadOnly = true,
                    Binding = new Binding($"Theme{i}Reward")
                });
            }
            
        }

    }

}
