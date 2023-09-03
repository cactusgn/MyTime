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
            CreateTestData();
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
        #region "todoDto"
        private ObservableCollection<ToDoDto> toDoDtos;
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        #endregion

        #region "MemoDto"
        private ObservableCollection<MemoDto> memoDtos;
        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        #endregion

        void CreateTestData()
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            for(int i = 0;i<10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Title = "待办" + i , Content="正在处理中"});
                MemoDtos.Add(new MemoDto() { Title = "备忘" + i, Content = "我的密码" });
            }
        }
    }
}
