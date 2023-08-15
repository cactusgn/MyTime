using Microsoft.Data.SqlClient;
using ScottPlot;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;

namespace Summary.Models
{
    public class RecordModel : ViewModelBase
    {
        private double height;
        private double rightPanelHeight;
        public double RightPanelHeight
        {
            get { return rightPanelHeight; }
            set { rightPanelHeight = value; }
        }

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
        private ToDoObj currentObj { get; set; }
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
        public MyCommand SelectedCommand { get; set; }
        public MyCommand UpdateTypeCommand { get; set; }
        public MyCommand ResizeCommand { get; set; }

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
        private ObservableCollection<GridSourceTemplate> allTimeViewObjs;

        public ObservableCollection<GridSourceTemplate> AllTimeViewObjs
        {
            get { return allTimeViewObjs; }
            set { allTimeViewObjs = value; OnPropertyChanged(); }
        }
        public ISQLCommands SQLCommands { get; set; }
        public ObservableCollection<TimeViewObj> DailyObj { get; set; } = new ObservableCollection<TimeViewObj>();
        private TimeViewObj selectedTimeObj;
        public WpfPlot SingleDayPlot { get; set; }
        public RadioButton AllRB { get; set; }
        public RadioButton ThirdLevelRB { get; set; }
        public RadioButton FirstLevelRB { get; set; }
        public TimeViewObj SelectedTimeObj
        {
            get { return selectedTimeObj; }
            set
            {
                selectedTimeObj = value;
                OnPropertyChanged();
            }
        }
        public RecordModel(ISQLCommands SqlCommands) {
            Enter_ClickCommand = new MyCommand(Enter_Click);
            DeleteContextMenu_ClickCommand = new MyCommand(DeleteContextMenu);
            TodayListBoxSelectionChangeCommand = new MyCommand(TodayListBoxSelectionChange);
            CheckChangedCommand = new MyCommand(CheckChanged);
            SelectedCommand = new MyCommand(Selected);
            UpdateTypeCommand = new MyCommand(UpdateType);
            ResizeCommand = new MyCommand(resizeHeight);
            SQLCommands = SqlCommands;
            InitTodayData();
        }
        private async void UpdateType(object a){
    
            var TodayAllObjectWithSameNote = AllTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs.Where(x => x.Note == selectedTimeObj.Note);
            foreach (var obj in TodayAllObjectWithSameNote)
            {
                obj.Type = a.ToString();
                Helper.UpdateColor(obj, a.ToString());
                await SQLCommands.UpdateObj(obj);
            }
        }
        private void Selected(object obj)
        {
            TimeViewObj myTimeView = (TimeViewObj)obj;
            SelectedTimeObj = myTimeView;
        }
        private async void InitTodayData()
        {
            AllTimeViewObjs = await Helper.BuildTimeViewObj(DateTime.Today, DateTime.Today, SQLCommands, height,"record");
        }
        
        private void Enter_Click(object obj)
        {
            if(obj.ToString()!=""){
                TodayList.Add(new ToDoObj() { Note = obj.ToString(), Finished = false });
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                TodayText = "";
            }
        }
        private void DeleteContextMenu(object obj)
        {
            if(currentObj != null)
            {
                TodayList.Remove(currentObj);
                currentObj = null;
            }
            
        }
        private void TodayListBoxSelectionChange(object obj)
        {
            if(SelectedListItem != null)
            {
                currentObj= SelectedListItem;
                WorkContent = SelectedListItem.Note;
                SelectedListItem = null;
            }
        }

        public void resizeHeight(object a = null)
        {
            if (AllTimeViewObjs != null)
            {
                if (a != null)
                {
                    if (a.ToString() == "amplify")
                    {
                        height = height * 1.5;
                    }
                    else if (height > RightPanelHeight)
                    {
                        height = height / 1.5;
                    }
                }
                else
                {
                    height = RightPanelHeight;
                }
                
                foreach (var gridSource in AllTimeViewObjs)
                {
                    foreach (var obj in gridSource.DailyObjs)
                    {
                        obj.Height = Helper.CalculateHeight(obj.LastTime, height,"record");
                    }
                }
            }
        }

        private void CheckChanged(object obj) {
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
        }
       
    }
}
