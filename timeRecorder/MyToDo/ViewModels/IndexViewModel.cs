using MyToDo.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            createTaskBars();
        }
        private ObservableCollection<TaskBar> taskBars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value;RaisePropertyChanged(); }
        }
        void createTaskBars()
        {
            TaskBars.Add(new TaskBar() { Icon="ClockFast",Title="汇总", Content="9",Target="",Color="#FF0CA0FF"});
            TaskBars.Add(new TaskBar() { Icon="ClockCheckOutline",Title="已完成", Content="9",Target="",Color="#FF1ECA3A"});
            TaskBars.Add(new TaskBar() { Icon="ChartLineVariant",Title="完成率", Content="100%",Target="",Color="#FF02C6DC"});
            TaskBars.Add(new TaskBar() { Icon="PlaylistStar",Title="备忘录", Content="19",Target="",Color="#FFFFA000"});
        }
    
    }
}
