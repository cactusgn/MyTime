﻿using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Summary.Models
{
    public class SummaryModel : ViewModelBase
    {
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged(); }
        }
        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; OnPropertyChanged(); }
        }
        private double leftPanelHeight;
        public double LeftPanelHeight
        {
            get { return leftPanelHeight; }
            set { leftPanelHeight = value; OnPropertyChanged(); resizeHeight(); }
        }
        private double height;
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }

        private ObservableCollection<GridSourceTemplate> allTimeViewObjs;

        public ObservableCollection<GridSourceTemplate> AllTimeViewObjs
        {
            get { return allTimeViewObjs; }
            set { allTimeViewObjs = value; OnPropertyChanged(); }
        }

        public MyCommand ClickOkButtonCommand { get; set; }
        public MyCommand TimeObjType_SelectionChangedCommand { get; set; }
        public MyCommand TimeObjType_NoteChangedCommand { get; set; }
        public MyCommand SummaryRBChangedCommand { get; set; }
        public MyCommand SingleDayRBChangedCommand { get; set; }
        public MyCommand ResizeCommand { get; set; }
        public MyCommand SplitButtonClickCommand { get; set; }
        public ISQLCommands SQLCommands { get; set; }
        public WpfPlot SingleDayPlot { get; set; }
        public WpfPlot SummaryPlot { get; set; }
        public RadioButton WasteRB { get; set; }
        public RadioButton WorkRB { get; set; }
        public RadioButton PlayRB { get; set; }
        public RadioButton StudyRB { get; set; }
        public RadioButton RestRB { get; set; }
        public RadioButton AllRB { get; set; }
        public RadioButton ThirdLevelRB { get; set; }
        public RadioButton FirstLevelRB { get; set; }
        public SampleDialogViewModel sampleDialogViewModel { get; set; }
        public MyCommand TextBoxLostFocusCommand { get; set; }
        public bool RadioButtonEnabled
        {
            get
            {
                return AllTimeViewObjs != null && AllTimeViewObjs.Count() > 0;
            }
        }
        private DateTime CurrentDate { get; set; } = DateTime.Today.AddDays(1);

        private Dictionary<TimeType, string> colorDic = new Dictionary<TimeType, string>();
        private string currentSummaryRBType = "all";
        private TimeViewObj selectedTimeObj;

        public TimeViewObj SelectedTimeObj
        {
            get { return selectedTimeObj; }
            set
            {
                selectedTimeObj = value;
                OnPropertyChanged();
            }
        }
        public SummaryModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM)
        {
            InitVariables();
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
            SingleDayRBChangedCommand = new MyCommand(SingleDayRBChanged);
            TimeObjType_SelectionChangedCommand = new MyCommand(TimeObjType_SelectionChanged);
            TimeObjType_NoteChangedCommand = new MyCommand(TimeObjType_NoteChanged);
            SplitButtonClickCommand = new MyCommand(SplitButtonClick);
            TextBoxLostFocusCommand = new MyCommand(TextBoxLostFocus);
            EndTime = DateTime.Today;
            StartTime = DateTime.Today.AddDays(-6);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            SelectedCommand = new MyCommand(Selected);
            ResizeCommand = new MyCommand(resizeHeight);
        }

       
        private void InitVariables()
        {
            colorDic.Add(TimeType.none, "#F3F3F3");
            colorDic.Add(TimeType.invest, "#FFB6C1");
            colorDic.Add(TimeType.waste, "#F08080");
            colorDic.Add(TimeType.rest, "#98FB98");
            colorDic.Add(TimeType.work, "#FFD700");
            colorDic.Add(TimeType.play, "#ADD8E6");
            height = LeftPanelHeight;
        }

        
        private async void TextBoxLostFocus(object obj)
        {
            var updateTimeViewObj = (TimeViewObj)obj;
            await SQLCommands.UpdateObj(updateTimeViewObj);
            refreshSingleDayPlot();
        }
        private void SingleDayRBChanged(object obj)
        {
            refreshSingleDayPlot();
        }
        private void SummaryRBChanged(object obj)
        {
            refreshSummaryPlot(obj.ToString());
        }
        private void Selected(object obj)
        {
            TimeViewObj myTimeView = (TimeViewObj)obj;
            SelectedTimeObj = myTimeView;
            if (myTimeView.CreatedDate!=CurrentDate)
            {
                CurrentDate = myTimeView.CreatedDate;
                refreshSingleDayPlot();
            }
        }
       
        private void refreshSingleDayPlot()
        {
            var AllObj = AllTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs;
            if (FirstLevelRB.IsChecked==true)
            {
                var AllObj2 = (AllObj.GroupBy(x => x.Type).Select(x => new TimeViewObj() { LastTime=new TimeSpan(x.Sum(x => x.LastTime.Ticks)),Type =x.Key,CreatedDate=SelectedTimeObj.CreatedDate, Note=x.Key }));
                FirstLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    Helper.refreshPlot(AllObj2, SingleDayPlot);
                }));
            }
            if (ThirdLevelRB.IsChecked==true)
            {
                ThirdLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    Helper.refreshPlot(AllObj, SingleDayPlot);
                }));
            }
        }
        private void refreshSummaryPlot(string type="all")
        {
            var AllObjs = new ObservableCollection<TimeViewObj>();
            currentSummaryRBType = type;
            foreach (var GridTemplate in allTimeViewObjs)
            {
                var dailyObjs = GridTemplate.DailyObjs;
                if (type.ToLower() != "all"&&dailyObjs!=null)
                {
                    var filteredObjs = dailyObjs.Where(x => (x.Type!=null && x.Type.ToLower()==type.ToLower())).OrderBy(e => e.LastTime);
                    if (filteredObjs!=null)
                    {
                        foreach (var item in filteredObjs)
                        {
                            AllObjs.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in dailyObjs)
                    {
                        AllObjs.Add(item);
                    }
                }
            }
            SummaryPlot.Dispatcher.Invoke(new Action(delegate
            {
                Helper.refreshPlot(AllObjs, SummaryPlot);
            }));
        }
        
        public MyCommand SelectedCommand { get; set; }

        private  void closeDialog()
        {
            IsDialogOpen=false;

        }
        private void openDialog()
        {
            IsDialogOpen=true;
        }
        private async void openSplitDialog(){
            var view = new SampleDialog(SelectedTimeObj, sampleDialogViewModel);
            await DialogHost.Show(view, "SubRootDialog");
        }
        private void SplitButtonClick(object a = null){
            openSplitDialog();
        }
        private async void clickOkButton(object a = null)
        {
            if (a!=null && a.ToString() == "LastWeek")
            {
                StartTime = StartTime.AddDays(-7);
                EndTime = StartTime.AddDays(6);
            }
            if (a!=null &&a.ToString() == "NextWeek")
            {
                StartTime = EndTime.AddDays(1);
                EndTime = StartTime.AddDays(6);
            }
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
            if (a != null && a.ToString() == "ThisWeek"){
                DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;
                if(dayOfWeek != DayOfWeek.Sunday)
                {
                    StartTime = DateTime.Today.AddDays(DayOfWeek.Monday - dayOfWeek);
                    EndTime = DateTime.Today.AddDays(DayOfWeek.Saturday - dayOfWeek+1);
                }
                else
                {
                    StartTime = DateTime.Today.AddDays(-6);
                    EndTime = DateTime.Today;
                }
            }
                await Task.Run(() => { openDialog(); }).ContinueWith(delegate { showTimeView(); closeDialog(); });
            //await Task.Run(() => { openDialog(); }).ContinueWith(delegate { showTimeView(); closeDialog(); }).ContinueWith(delegate { closeDialog(); });
        }
        
        private async void TimeObjType_SelectionChanged(object a)
        {
            if (SelectedTimeObj.Type!=a.ToString())
            {
                var TodayAllObjectWithSameNote = AllTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs.Where(x => x.Note==selectedTimeObj.Note);
                foreach (var obj in TodayAllObjectWithSameNote)
                {
                    obj.Type = a.ToString();
                    Helper.UpdateColor(obj, a.ToString());
                    await SQLCommands.UpdateObj(obj);
                }
                refreshSingleDayPlot();
                refreshSummaryPlot(currentSummaryRBType);
            }
        }
        private async void TimeObjType_NoteChanged(object a)
        {
            var currObj = AllTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs.First(x => x.Id==selectedTimeObj.Id);
            currObj.Note = a.ToString();
            currObj.TimeNote = currObj.LastTime + "\n" + currObj.Note;
            await SQLCommands.UpdateObj(SelectedTimeObj);
            refreshSingleDayPlot();
        }
        public async void showTimeView()
        {
            AllTimeViewObjs = await Helper.BuildTimeViewObj(startTime, endTime,SQLCommands,height);
            SelectedTimeObj = new TimeViewObj() { Type="" };
            AllRB.Dispatcher.Invoke(new Action(delegate
            {
                if (AllRB.IsChecked==true) refreshSummaryPlot("all");
            }));
            WorkRB.Dispatcher.Invoke(new Action(delegate
            {
                if (WorkRB.IsChecked==true) refreshSummaryPlot("work");
            }));
            StudyRB.Dispatcher.Invoke(new Action(delegate
            {
                if (StudyRB.IsChecked==true) refreshSummaryPlot("invest");
            }));
            WasteRB.Dispatcher.Invoke(new Action(delegate
            {
                if (WasteRB.IsChecked==true) refreshSummaryPlot("waste");
            }));
            RestRB.Dispatcher.Invoke(new Action(delegate
            {
                if (RestRB.IsChecked==true) refreshSummaryPlot("rest");
            }));
            PlayRB.Dispatcher.Invoke(new Action(delegate
            {
                if (PlayRB.IsChecked==true) refreshSummaryPlot("play");
            }));
        }
        public void resizeHeight(object a = null)
        {
            if (AllTimeViewObjs != null)
            {
                if (a!=null)
                {
                    if (a.ToString()=="amplify")
                    {
                        height = height*1.5;
                    }
                    else if(height>LeftPanelHeight)
                    {
                        height = height/1.5;
                    }
                }
                else
                {
                    height = LeftPanelHeight;
                }
                foreach (var gridSource in AllTimeViewObjs)
                {
                    foreach (var obj in gridSource.DailyObjs)
                    {
                        obj.Height = Helper.CalculateHeight(obj.LastTime,height);
                    }
                }
                SummaryPlot.Height = LeftPanelHeight-250;
                SingleDayPlot.Height = LeftPanelHeight-250;
                SummaryPlot.Refresh();
                SingleDayPlot.Refresh();
            }
        }
        
        public async void SplitTimeBlock(TimeSpan SplitTime, string content1, string content2){
            if(selectedTimeObj!=null){
                var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
                var lastIndex = currentDailyObj.Max(x=>x.Id) +1;
                int taskId = SQLCommands.QueryTodo(content1);
                var newTimeObj1 = Helper.CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, TimeType.none, lastIndex, height, taskId: taskId);
                lastIndex++;
                taskId = 0;
                if (content2!="")
                {
                    taskId =  SQLCommands.QueryTodo(content2);
                }
                var newTimeObj2 = Helper.CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, TimeType.none, lastIndex,height, taskId: taskId);
                Helper.UpdateColor(newTimeObj1, TimeType.none.ToString());
                Helper.UpdateColor(newTimeObj2, TimeType.none.ToString());
                await SQLCommands.DeleteObj(selectedTimeObj);
                await SQLCommands.AddObj(newTimeObj1);
                await SQLCommands.AddObj(newTimeObj2);
                
                currentDailyObj.Add(newTimeObj1);
                currentDailyObj.Add(newTimeObj2);
                currentDailyObj.Remove(selectedTimeObj);
                AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
                SelectedTimeObj = newTimeObj1;
                refreshSingleDayPlot();
                refreshSummaryPlot();
            }
        }
       
    }
    //表格视图的单天Template
    public class GridSourceTemplate : ViewModelBase
    {
        private ObservableCollection<TimeViewObj> dailyObjs;
        public DateTime createdDate { get; set; }
        public ObservableCollection<TimeViewObj> DailyObjs
        {
            get { return dailyObjs; }
            set { dailyObjs = value; OnPropertyChanged(); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }
        private string week;
        public string Week
        {
            get { return week; }
            set { week = value; OnPropertyChanged(); }
        }
        private string color;
        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }
        public GridSourceTemplate(DateTime createdDate)
        {
            DailyObjs = new ObservableCollection<TimeViewObj>();
            //DailyObjs.CollectionChanged += DailyObjs_OnCollectionChanged;
            this.createdDate = createdDate;
        }

      
    }
    public class ChartBar
    {
        public string Note { get; set; }
        public string Type { get; set; }
        public TimeSpan Time { get; set; }

    }
    
}
