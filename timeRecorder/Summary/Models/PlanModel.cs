using Summary.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Summary.Models
{
    public class PlanModel : ViewModelBase
    {
        public DataGrid TimeGrid { get; set; }
        private ObservableCollection<TaskView> timeObjs;

        public ObservableCollection<TaskView> TimeObjs
        {
            get { return timeObjs; }
            set { timeObjs = value; OnPropertyChanged(); }
        }
        public PlanModel()
        {
            TimeObjs = new ObservableCollection<TaskView>();
            var task = new TaskView()
            {
                taskId = 0,
                taskName = "123taskname",
                taskDate = DateTime.Now,
                lastTime = new TimeSpan(0, 20, 0)
            };
            TimeObjs.Add(task);
        }
        
       
    }

    public class TaskView
    {
        public int taskId { get; set; }
        public string taskName { get; set; }
        public TimeSpan lastTime { get; set; }
        public DateTime taskDate {  get; set; }

    }
}
