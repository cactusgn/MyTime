﻿using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using Summary.Domain;
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
using System.Windows.Media.Animation;
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
        public MyCommand StartCommand { get; set; }
        public MyCommand EndCommand { get; set; }

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
        private bool startbtnEnabled=true;

        public bool StartbtnEnabled
        {
            get { return startbtnEnabled; }
            set { startbtnEnabled = value; OnPropertyChanged(); }
        }
        private bool endbtnEnabled = false;

        public bool EndbtnEnabled
        {
            get { return endbtnEnabled; }
            set { endbtnEnabled = value; OnPropertyChanged(); }
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
        public SampleDialogViewModel sampleDialogViewModel { get; set; }
        public MyCommand SplitButtonClickCommand { get; set; }
        public TimeViewObj SelectedTimeObj
        {
            get { return selectedTimeObj; }
            set
            {
                selectedTimeObj = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan WorkStartTime;
        public RecordModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM) {
            Enter_ClickCommand = new MyCommand(Enter_Click);
            DeleteContextMenu_ClickCommand = new MyCommand(DeleteContextMenu);
            TodayListBoxSelectionChangeCommand = new MyCommand(TodayListBoxSelectionChange);
            CheckChangedCommand = new MyCommand(CheckChanged);
            SelectedCommand = new MyCommand(Selected);
            UpdateTypeCommand = new MyCommand(UpdateType);
            ResizeCommand = new MyCommand(resizeHeight);
            SplitButtonClickCommand = new MyCommand(SplitButtonClick);
            StartCommand = new MyCommand(StartClick);
            EndCommand = new MyCommand(EndClick);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            InitTodayData();
        }

        private async void EndClick(object obj)
        {
            StartbtnEnabled = true;
            EndbtnEnabled = false;
            GridSourceTemplate currentDateTemplate;
            if (AllTimeViewObjs==null)
            {
                currentDateTemplate = initAllTimeViewObjs();
            }
            else
            {
                currentDateTemplate = AllTimeViewObjs[0];
            }
            int lastIndex = AllTimeViewObjs[0].DailyObjs.Max(x => x.Id)+1;
            var newObj = Helper.CreateNewTimeObj(WorkStartTime,Helper.getCurrentTime(), WorkContent, DateTime.Today, TimeType.None, lastIndex, height, "record");
            await SQLCommands.AddObj(newObj);
            Helper.UpdateColor(newObj, "none");
            currentDateTemplate.DailyObjs.Add(newObj);
        }
        
        private async void StartClick(object obj)
        {
            if (WorkContent==null)
            {
                await showMessageBox("请先填写工作内容");
                return;
            }
            WorkStartTime = Helper.getCurrentTime();
            StartbtnEnabled = false;
            EndbtnEnabled = true;
            if (AllTimeViewObjs!=null && AllTimeViewObjs.Count>0&&AllTimeViewObjs[0].DailyObjs!=null&&AllTimeViewObjs[0].DailyObjs.Count>0)
            {
                var lastViewObj = AllTimeViewObjs[0].DailyObjs.OrderBy(x=>x.StartTime).LastOrDefault();
                int lastIndex = AllTimeViewObjs[0].DailyObjs.Max(x => x.Id)+1;
                var newObj = Helper.CreateNewTimeObj(lastViewObj.EndTime, WorkStartTime, Helper.RestContent, DateTime.Today, TimeType.Rest, lastIndex, height, "record");
                await SQLCommands.AddObj(newObj);
                Helper.UpdateColor(newObj, "none");
                AllTimeViewObjs[0].DailyObjs.Add(newObj);
            }
            else
            {
                var currentDateTemplate = initAllTimeViewObjs();
                if (Helper.getCurrentTime() > Helper.GlobalStartTimeSpan)
                {
                    var newObj = Helper.CreateNewTimeObj(Helper.GlobalStartTimeSpan, WorkStartTime, Helper.RestContent, DateTime.Today, TimeType.Rest, 1, height, "record");
                    await SQLCommands.AddObj(newObj);
                    Helper.UpdateColor(newObj, "none");
                    currentDateTemplate.DailyObjs.Add(newObj);
                }
                else
                {
                    Helper.GlobalStartTimeSpan = Helper.getCurrentTime();
                }
            }
        }
        //private void outputText(bool showMessage = true)
        //{
        //    deleteFile();
        //    for (int i = 0; i < timelist.Count; i++)
        //    {
        //        addText("开始时间：" + format_date(timelist[i].mStartTime));
        //        if (timelist[i].mEndTime == DateTime.Parse("1994-11-11"))
        //        {
        //            timelist[i].mEndTime = DateTime.Now;
        //            timelist[i].interval = timelist[i].mEndTime - timelist[i].mStartTime;
        //        }
        //        addText("间隔时间：" + format_date(timelist[i].interval));
        //        addText("结束时间：" + format_date(timelist[i].mEndTime));
        //        addText("类型：" + timelist[i].timeType);
        //        addText("备注：" + timelist[i].comment);
        //        addText("");
        //    }
        //    if (showMessage)
        //        MessageBox.Show("导出成功！");
        //}
        private GridSourceTemplate initAllTimeViewObjs()
        {
            AllTimeViewObjs = new ObservableCollection<GridSourceTemplate>();
            var currentDateTemplate = new GridSourceTemplate(DateTime.Today);
            currentDateTemplate.Title = DateTime.Today.ToShortDateString();
            currentDateTemplate.Week = DateTime.Today.DayOfWeek.ToString();
            if (currentDateTemplate.Week.Equals("Saturday")||currentDateTemplate.Week.Equals("Sunday"))
            {
                currentDateTemplate.Color = "#32CD32";
            }
            else
            {
                currentDateTemplate.Color = "#008080";
            }
            AllTimeViewObjs.Add(currentDateTemplate);
            return currentDateTemplate;
        }
        private async Task showMessageBox(string message)
        {
            var view = new SampleMessageDialog(message);
            await DialogHost.Show(view, "SubRootDialog");
        }
        private void SplitButtonClick(object a = null)
        {
            openSplitDialog();
        }
        private async void openSplitDialog()
        {
            if(selectedTimeObj == null)
            {
                await showMessageBox("请先选中要分割的时间块");
                return;
            }
            var view = new SampleDialog(SelectedTimeObj, sampleDialogViewModel);
            await DialogHost.Show(view, "SubRootDialog");
        }
        public async void SplitTimeBlock(TimeSpan SplitTime, string content1, string content2)
        {
            if (selectedTimeObj!=null)
            {
                var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
                var lastIndex = currentDailyObj.Max(x => x.Id) +1;
                var newTimeObj1 = Helper.CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, TimeType.None, lastIndex, height);
                lastIndex++;
                var newTimeObj2 = Helper.CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, TimeType.None, lastIndex, height);
                Helper.UpdateColor(newTimeObj1, TimeType.None.ToString());
                Helper.UpdateColor(newTimeObj2, TimeType.None.ToString());
                await SQLCommands.DeleteObj(selectedTimeObj);
                await SQLCommands.AddObj(newTimeObj1);
                await SQLCommands.AddObj(newTimeObj2);

                currentDailyObj.Add(newTimeObj1);
                currentDailyObj.Add(newTimeObj2);
                currentDailyObj.Remove(selectedTimeObj);
                AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
                SelectedTimeObj = newTimeObj1;
                refreshSingleDayPlot();
            }
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
        public void refreshSingleDayPlot()
        {
            var AllObj = AllTimeViewObjs.First(x => x.createdDate == DateTime.Today).DailyObjs;
            if (FirstLevelRB.IsChecked == true)
            {
                var AllObj2 = (AllObj.GroupBy(x => x.Type).Select(x => new TimeViewObj() { LastTime = new TimeSpan(x.Sum(x => x.LastTime.Ticks)), Type = x.Key, CreatedDate = DateTime.Today, Note = x.Key }));
                FirstLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    Helper.refreshPlot(AllObj2, SingleDayPlot);
                }));
            }
            if (ThirdLevelRB.IsChecked == true)
            {
                ThirdLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    Helper.refreshPlot(AllObj, SingleDayPlot);
                }));
            }

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
