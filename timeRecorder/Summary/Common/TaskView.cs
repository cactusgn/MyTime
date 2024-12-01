using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Summary.Common
{
    public class TaskView
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public TimeSpan LastTime { get; set; }
        public DateTime TaskDate { get; set; }
        
        public int Reward { get; set; }
    }
    public class TaskTheme
    {
        public string ThemeName { get; set; }
        public List<TaskView> taskViewList { get; set; }
    }
    public class DayTaskData
    {
        public DateTime TaskDate { get; set; }

        public List<TaskTheme> TaskThemeList { get; set; }
    }
    public class DayTaskView : ViewModelBase
    {
        public bool IsReadOnly
        { 
            get {
                return TaskDate <= DateTime.Today;
            } 
        }
        public Visibility AddButtonVisible
        {
            get
            {
                if (TaskDate > DateTime.Today){
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public DateTime TaskDate { get; set; }
        
        public string DayOfWeek
        {
            get { return convertDayOfWeek(TaskDate.DayOfWeek); }
        }
        private int background = 1;
        public int Background {
            get { return background; }
            set { background = value; OnPropertyChanged(); }
        }
        public string Theme1 { get; set; } = "";
        public string theme1TaskName { get; set; } = "";
        public string Theme1TaskName
        {
            get { return theme1TaskName; }
            set { theme1TaskName = value; OnPropertyChanged(); }
        }
        public string Theme1LastTime { get; set; } = "";
        public string Theme1Reward { get; set; } = "";
        public string Theme2 { get; set; } = "";
        public string theme2TaskName { get; set; } = "";
        public string Theme2TaskName
        {
            get { return theme2TaskName; }
            set { theme2TaskName = value; OnPropertyChanged(); }
        }
        public string Theme2LastTime { get; set; } = "";
        public string Theme2Reward { get; set; } = "";
        public string Theme3 { get; set; } = "";
        public string theme3TaskName { get; set; } = "";
        public string Theme3TaskName
        {
            get { return theme3TaskName; }
            set { theme3TaskName = value; OnPropertyChanged(); }
        }
        public string Theme3LastTime { get; set; } = "";
        public string Theme3Reward { get; set; } = "";
        public string Theme4 { get; set; } = "";
        public string theme4TaskName { get; set; } = "";
        public string Theme4TaskName
        {
            get { return theme4TaskName; }
            set { theme4TaskName = value; OnPropertyChanged(); }
        }
        public string Theme4LastTime { get; set; } = "";
        public string Theme4Reward { get; set; } = "";
        public string Theme5 { get; set; } = "";
        public string theme5TaskName { get; set; } = "";
        public string Theme5TaskName
        {
            get { return theme5TaskName; }
            set { theme5TaskName = value; OnPropertyChanged(); }
        }
        public string Theme5LastTime { get; set; } = "";
        public string Theme5Reward { get; set; } = "";
        private string convertDayOfWeek(DayOfWeek dayOfWeek)
        {
            var res = "";
            switch (dayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    res = "周一";
                    break;
                case System.DayOfWeek.Tuesday:
                    res ="周二";
                    break;
                case System.DayOfWeek.Wednesday:
                    res = "周三";
                    break;
                case System.DayOfWeek.Thursday:
                    res = "周四";
                    break;
                case System.DayOfWeek.Friday:
                    res =  "周五";
                    break;
                case System.DayOfWeek.Saturday:
                    res =  "周六";
                    break;
                case System.DayOfWeek.Sunday:
                    res =  "周日";
                    break;
            }
            return res;
        }
    }
}
