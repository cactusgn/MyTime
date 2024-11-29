using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class DayTaskView:ViewModelBase
    {
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
        public string Theme1TaskName { get; set; } = "";
        public string Theme1LastTime { get; set; } = "";
        public string Theme1Reward { get; set; } = "";
        public string Theme2 { get; set; } = "";
        public string Theme2TaskName { get; set; } = "";
        public string Theme2LastTime { get; set; } = "";
        public string Theme2Reward { get; set; } = "";
        public string Theme3 { get; set; } = "";
        public string Theme3TaskName { get; set; } = "";
        public string Theme3LastTime { get; set; } = "";
        public string Theme3Reward { get; set; } = "";
        public string Theme4 { get; set; } = "";
        public string Theme4TaskName { get; set; } = "";
        public string Theme4LastTime { get; set; } = "";
        public string Theme4Reward { get; set; } = "";
        public string Theme5 { get; set; } = "";
        public string Theme5TaskName { get; set; } = "";
        public string Theme5LastTime { get; set; } = "";
        public string Theme5Reward { get; set; } = "";
    }
}
