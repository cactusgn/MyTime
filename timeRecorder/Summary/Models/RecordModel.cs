using Summary.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Summary.Models
{
    public class RecordModel : ViewModelBase
    {
        private ObservableCollection<ToDoObj> todayList = new ObservableCollection<ToDoObj>();
        public ObservableCollection<ToDoObj> TodayList{
        get { 
                return todayList; 
            }
        set{
                todayList = value;
                OnPropertyChanged();
            }
        }
        public ToDoObj selectedListItem { get; set; }
        public ToDoObj SelectedListItem
        {
            get { return selectedListItem; }
            set
            {
                selectedListItem = value;
                OnPropertyChanged();
            }
        }
                    
        public MyCommand Enter_ClickCommand { get; set; }
        public MyCommand DeleteContextMenu_ClickCommand { get; set; }
        public MyCommand TodayListBoxSelectionChangeCommand { get; set; }
        public MyCommand CheckChangedCommand { get; set; }

        public int interval { get; set; }
        public string Interval
        {
            get
            {
                return interval.ToString();
            }
            set
            {
                interval = int.Parse(value);
                OnPropertyChanged();
            }
        }
        private string todayText;

        public string TodayText
        {
            get { return todayText; }
            set { todayText = value; OnPropertyChanged(); }
        }
        private string workContent;

        public string WorkContent
        {
            get { return workContent; }
            set { workContent = value; OnPropertyChanged(); }
        }

        public RecordModel() {
            Enter_ClickCommand = new MyCommand(Enter_Click);
            DeleteContextMenu_ClickCommand = new MyCommand(DeleteContextMenu);
            TodayListBoxSelectionChangeCommand = new MyCommand(TodayListBoxSelectionChange);
            CheckChangedCommand = new MyCommand(CheckChanged);
        }
       
        private void Enter_Click(object obj)
        {
            TodayList.Add(new ToDoObj() { Note=obj.ToString(), Finished=false });
            TodayText = "";
        }
        private void DeleteContextMenu(object obj)
        {
            if(SelectedListItem != null)
            {
                TodayList.Remove(SelectedListItem);
            }
            
        }
        private void TodayListBoxSelectionChange(object obj)
        {
            if(SelectedListItem != null)
                WorkContent = SelectedListItem.Note;
        }
        private void CheckChanged(object obj) {
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
        }
    }
}
