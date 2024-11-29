using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Summary.Models
{
    public class PlanModel : ViewModelBase
    {
        public System.Windows.Controls.DataGrid TimeGrid { get; set; }
        public Dictionary<int, string> WorkThemes { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> SubWorkThemes { get; set; } = new Dictionary<int, string>();
        
        private int todayReward;
        private string todayRewardString;
        public string TodayRewardString
        {
            get { return todayRewardString; }
            set { todayRewardString = value; OnPropertyChanged(); }
        }
        private int todayMinus;
        private string todayMinusString;
        public string TodayMinusString
        {
            get { return todayMinusString; }
            set { todayMinusString = value; OnPropertyChanged(); }
        }
        private string todayTotalString;
        public string TodayTotalString
        {
            get { return todayTotalString; }
            set { todayTotalString = value; OnPropertyChanged(); }
        }
        private int weekReward;
        private string weekRewardString;
        public string WeekRewardString
        {
            get { return weekRewardString; }
            set { weekRewardString = value; OnPropertyChanged(); }
        }
        private int weekMinus;
        private string weekMinusString;
        public string WeekMinusString
        {
            get { return weekMinusString; }
            set { weekMinusString = value; OnPropertyChanged(); }
        }
        private string weekTotalString;
        public string WeekTotalString
        {
            get { return weekTotalString; }
            set { weekTotalString = value; OnPropertyChanged(); }
        }
        private int currentReward;
        private string currentRewardString;
        public string CurrentRewardString
        {
            get { return currentRewardString; }
            set { currentRewardString = value; OnPropertyChanged(); }
        }
        private int currentMinus;
        private string currentMinusString;
        public string CurrentMinusString
        {
            get { return currentMinusString; }
            set { currentMinusString = value; OnPropertyChanged(); }
        }
        private string currentTotalString;
        public string CurrentTotalString
        {
            get { return currentTotalString; }
            set { currentTotalString = value; OnPropertyChanged(); }
        }
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
        private bool themeOpen;
        public bool ThemeOpen
        {
            get => themeOpen;
            set { themeOpen = value; OnPropertyChanged(); }
        }
        private bool showHiddenItems;
        public bool ShowHiddenItems
        {
            get => showHiddenItems;
            set { showHiddenItems = value; OnPropertyChanged(); }
        }
        private bool useLightBGColor;
        public bool UseLightBGColor
        {
            get => useLightBGColor;
            set { useLightBGColor = value;
                Helper.SetAppSetting("UseLightBGColor", useLightBGColor.ToString());
                foreach (var item in TimeObjs)
                {
                    if (useLightBGColor && item.Background == 1)
                    {
                        item.Background = 3;
                    }
                    if (!useLightBGColor && item.Background == 3)
                    {
                        item.Background = 1;
                    }
                }
                OnPropertyChanged("TimeObjs");
                OnPropertyChanged(); 
            }
        }
        public MyCommand ClickOkButtonCommand { get; set; }
        public MyCommand ClickThemeButtonCommand { get; set; }
        private ObservableCollection<DayTaskView> timeObjs = new ObservableCollection<DayTaskView>();

        public ObservableCollection<DayTaskView> TimeObjs
        {
            get { return timeObjs; }
            set { timeObjs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CategoryTheme> themeItems = new ObservableCollection<CategoryTheme>();
        public ObservableCollection<CategoryTheme> ThemeItems
        {
            get { return themeItems; }
            set { themeItems = value; OnPropertyChanged();}
        }
        public ISQLCommands SQLCommands { get; set; }
        private DateTime firstDayOfWeek{get;set;}
        public MyCommand CheckChangedCommand { get; set; }
        public MyCommand ShowHiddenItemsCheckedCommand { get; set; }
        public MyCommand UseLightBGColorCheckedCommand { get; set; }
        public MyCommand ClickOkButtonInChoosingTheme { get; set; }
        public DialogType dialogType { get; set; }
        public PlanModel(ISQLCommands SqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            ClickThemeButtonCommand = new MyCommand(ClickThemeButton);
            CheckChangedCommand = new MyCommand(ThemeCheckChanged);
            ShowHiddenItemsCheckedCommand = new MyCommand(ShowHiddenItemsCheckChanged);
            UseLightBGColorCheckedCommand = new MyCommand(UseLightBGColorChecked);
            ClickOkButtonInChoosingTheme = new MyCommand(ConfirmThemes);
            SQLCommands = SqlCommands;
            WorkThemes = SQLCommands.getCheckedThemes();
            firstDayOfWeek = getFirstDayOfWeek();
            ShowHiddenItems = bool.Parse(Helper.GetAppSetting("ShowHiddenItems"));
            UseLightBGColor = bool.Parse(Helper.GetAppSetting("UseLightBGColor"));
        }

        private void UseLightBGColorChecked(object obj)
        {
            
        }

        public async Task showMessageBox(string message)
        {
            dialogType = DialogType.MessageDialog;
            var view = new SampleMessageDialog(message);
            await DialogHost.Show(view, "RootDialog3");
        }
        private async void ConfirmThemes(object obj)
        {
           if(SelectedMoreThan5Themes()){
                await showMessageBox("最多选择5个主题");
                return;
           }
           WorkThemes.Clear();
           foreach (var theme in ThemeItems)
            {
                if (theme.Checked)
                {
                    WorkThemes.Add(theme.Id, theme.Name);
                }
                SQLCommands.updateCheckedTheme(theme);
            }
            ThemeOpen = false;
            clickOkButton(null);
        }

        private bool SelectedMoreThan5Themes()
        {
            return ThemeItems.Where(x=>x.Checked).Count()>5;
        }

        private void ShowHiddenItemsCheckChanged(object obj)
        {
            Helper.SetAppSetting("ShowHiddenItems", ShowHiddenItems.ToString());
            ThemeItems.Clear();
            addThemes(0, 0);
        }

        private void ThemeCheckChanged(object obj)
        {
            //CategoryTheme ct = (CategoryTheme)obj;
            //SQLCommands.updateCheckedTheme(ct);
        }

        private void ClickThemeButton(object obj)
        {
            ThemeOpen = true;
            WorkThemes = SQLCommands.getCheckedThemes();
            ThemeItems.Clear();
            addThemes(0, 0);
        }
        private void addThemes(int parentKey, int level)
        {
            foreach(Category category in Helper.allcategories)
            {
                if(category.ParentCategoryId == parentKey)
                {
                    if(!category.Visible&&!showHiddenItems)
                    {
                        continue;
                    }
                    ThemeItems.Add(new CategoryTheme() { Id = category.Id, Level = level + 1, Checked= WorkThemes.ContainsKey(category.Id), Name = category.Name });
                    addThemes(category.Id, level+1);
                }
            }
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

        public async void clickOkButton(object a)
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
            string ThemeColor = Helper.GetAppSetting("ThemeColor");
            for (int i = 0; i<(endTime-startTime).Days+1; i++)
            {
                var dayTaskView = new DayTaskView() { TaskDate = startTime.AddDays(i) };
                foreach (var value in WorkThemes.Values)
                {
                    dayTaskView.Background = i % 2;
                    if(useLightBGColor && dayTaskView.Background == 1)
                    {
                        dayTaskView.Background = 3;
                    }
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
            if(startTime<= DateTime.Today && endTime >= DateTime.Today){
                todayReward = 0;
                todayMinus = 0;
            }
            if(startTime <= firstDayOfWeek)
            {
                weekReward = 0;
                weekMinus = 0;
            }
            currentMinus = 0;
            currentReward = 0;
            List<MyTime> AllTimeObjs = await SQLCommands.GetAllTimeObjs(startTime,endTime);
            List<ToDoObj> allToDoByDate = AllTimeObjs.Where(x=>x.type!= null&&x.type!="none"&&!string.IsNullOrEmpty(x.note)).OrderBy(x => x.createDate).GroupBy(x => new { x.note,x.createDate }).Select(x => new ToDoObj() { CreatedDate = x.Key.createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => (x.endTime-x.startTime).Ticks)), Id = x.First().taskId, Type = x.First().type }).ToList();
            bool calculateAllAtFirstTime = true;
            foreach (int i in WorkThemes.Keys)
            {
                SubWorkThemes.Clear();
                AddSubCategories(i);
                foreach (ToDoObj recordedTask in allToDoByDate)
                {
                    GeneratedToDoTask todo = SQLCommands.QueryTodo(recordedTask.Id);
                    Category a = Helper.allcategories.FirstOrDefault(x => x.Id == todo.CategoryId);
                    recordedTask.Type = a.Name;
                    recordedTask.Bonus = Convert.ToInt32(a.BonusPerHour * recordedTask.LastTime.TotalHours);
                    if(calculateAllAtFirstTime){
                        CalculateBonus(recordedTask.Bonus, recordedTask.CreatedDate);
                    }
                    if (SubWorkThemes.ContainsKey(a.Id))
                    {
                       recordedTask.Type = WorkThemes[i];
                       InsertTaskIntoDayTaskViews(recordedTask, TimeObjs);
                    }
                }
                calculateAllAtFirstTime = false;
            }
            TimeGrid.Dispatcher.Invoke(new Action(delegate
            {
                TodayRewardString = todayReward.ToString();
                TodayMinusString = todayMinus.ToString();
                TodayTotalString = (todayReward+ todayMinus).ToString();
                WeekRewardString = weekReward.ToString();
                WeekMinusString = weekMinus.ToString();
                WeekTotalString = (weekReward + weekMinus).ToString();
                CurrentRewardString = currentReward.ToString();
                CurrentMinusString = currentMinus.ToString();
                CurrentTotalString = (currentReward + currentMinus).ToString();
            }));
        }

        private void CalculateBonus(int bonus, DateTime taskDate)
        {
            if(bonus>0){
                currentReward += bonus;
                if(DateTime.Today==taskDate){
                    todayReward += bonus;
                }
                if(taskDate.Date >= firstDayOfWeek && taskDate.Date <= firstDayOfWeek.AddDays(6)){
                    weekReward += bonus;
                }
            }
            else{
                currentMinus += bonus;
                if (DateTime.Today == taskDate)
                {
                    todayMinus += bonus;
                }
                if (taskDate.Date >= firstDayOfWeek && taskDate.Date <= firstDayOfWeek.AddDays(6))
                {
                    weekMinus += bonus;
                }
            }
        }

        private DateTime getFirstDayOfWeek()
        {
            // 获取当前日期
            DateTime now = DateTime.Today;

            DateTime firstDayOfWeek = now.AddDays(1-(int)(now.DayOfWeek));
            return firstDayOfWeek;
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
                        Background = DayTaskViews.First().Background,
                    });
                }));
                
                InsertTaskIntoDayTaskViews(recordedTask, TimeObjs);
            }
        }

        public void AddColumnsToTable()
        {
            this.TimeGrid.Columns.Clear();
            this.TimeGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn()
            {
                Header = "日期",
                IsReadOnly = true,
                Binding = new Binding("TaskDate")
                {
                    StringFormat = "yyyy/MM/dd"
                }
            });
            this.TimeGrid.Columns.Add(new MaterialDesignThemes.Wpf.DataGridTextColumn()
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
