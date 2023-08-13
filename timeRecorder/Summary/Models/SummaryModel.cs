using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using Summary.Common;
using Summary.Data;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        
        public SummaryModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM)
        {
            InitVariables();
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
            SingleDayRBChangedCommand = new MyCommand(SingleDayRBChanged);
            TimeObjType_SelectionChangedCommand = new MyCommand(TimeObjType_SelectionChanged);
            TimeObjType_NoteChangedCommand = new MyCommand(TimeObjType_NoteChanged);
            SplitButtonClickCommand = new MyCommand(SplitButtonClick);
            EndTime = DateTime.Today;
            StartTime = DateTime.Today.AddDays(-6);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            SelectedCommand = new MyCommand(Selected);
            ResizeCommand = new MyCommand(resizeHeight);
        }

       
        private void InitVariables()
        {
            colorDic.Add(TimeType.None, "#F3F3F3");
            colorDic.Add(TimeType.Study, "#FFB6C1");
            colorDic.Add(TimeType.Waste, "#F08080");
            colorDic.Add(TimeType.Rest, "#98FB98");
            colorDic.Add(TimeType.Work, "#FFD700");
            colorDic.Add(TimeType.Play, "#ADD8E6");
            height = LeftPanelHeight;
        }

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
        private void refreshPlot(IEnumerable<TimeViewObj> AllObj, WpfPlot plot)
        {
            var items = AllObj.GroupBy(x => new { x.Note, x.Type }).Select(x => new ChartBar { Note=x.Key.Note, Type=x.Key.Type, Time = new TimeSpan(x.Sum(x => x.LastTime.Ticks)) }).OrderBy(x => x.Type).ThenByDescending(x => x.Time);
            var studyItems = items.Where(x => x.Type=="study").ToArray();
            var wasteItems = items.Where(x => x.Type=="waste").ToArray();
            var workItems = items.Where(x => x.Type=="work").ToArray();
            var restItems = items.Where(x => x.Type=="rest").ToArray();
            var playItems = items.Where(x => x.Type=="play").ToArray();
            var index = 0;
            plot.Plot.Clear();
            var plt = plot.Plot;
            var allItemCount = studyItems.Length + wasteItems.Length+ workItems.Length+restItems.Length + playItems.Length;
            string[] TimeLabels = new string[allItemCount];
            string[] YLabels = new string[allItemCount];
            double[] position = new double[allItemCount];
            addChartData(studyItems, TimeType.Study, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(workItems, TimeType.Work, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(wasteItems, TimeType.Waste, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(restItems, TimeType.Rest, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            addChartData(playItems, TimeType.Play, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);

            plt.YTicks(position, YLabels);
            plt.Legend(location: Alignment.UpperRight);
            Func<double, string> customFormatter = y => $"{TimeSpan.FromSeconds(y).ToString()}";
            plt.XAxis.TickLabelFormat(customFormatter);
            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
            plot.Refresh();
        }
        private void refreshSingleDayPlot()
        {
            var AllObj = AllTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs;
            if (FirstLevelRB.IsChecked==true)
            {
                var AllObj2 = (AllObj.GroupBy(x => x.Type).Select(x => new TimeViewObj() { LastTime=new TimeSpan(x.Sum(x => x.LastTime.Ticks)),Type =x.Key,CreatedDate=SelectedTimeObj.CreatedDate, Note=x.Key }));
                FirstLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    refreshPlot(AllObj2, SingleDayPlot);
                }));
            }
            if (ThirdLevelRB.IsChecked==true)
            {
                ThirdLevelRB.Dispatcher.Invoke(new Action(delegate
                {
                    refreshPlot(AllObj, SingleDayPlot);
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
                refreshPlot(AllObjs, SummaryPlot);
            }));
        }
        private void addChartData(ChartBar[] Items, TimeType type, ref double[] position, ref string[] YLabels, ref string[] TimeLabels, ref Plot plt, ref int index)
        {
            if (Items.Count()>0)
            {
                double[] itemPostion = new double[Items.Count()];
                double[] itemValues = new double[Items.Count()];

                for (int i = 0; i<Items.Count(); i++)
                {
                    itemPostion[i] = index + i+1;
                    position[i+index] = index +i+1;
                    YLabels[i+index] = Items[i].Note;
                    itemValues[i] = Items[i].Time.TotalSeconds;
                    TimeLabels.Append(Items[i].Time.ToString());
                }
                var bar = plt.AddBar(itemValues, itemPostion, System.Drawing.ColorTranslator.FromHtml(colorDic[type]));
                bar.Orientation = ScottPlot.Orientation.Horizontal;
                bar.ShowValuesAboveBars = true;
                Func<double, string> customFormatter = y => $"{TimeSpan.FromSeconds(y).ToString()}";
                bar.ValueFormatter = customFormatter;
                index = index + Items.Count();
            }
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
            var tempDate = EndTime;
            if (a!=null && a.ToString() == "LastWeek")
            {
                StartTime = StartTime.AddDays(-7);
                EndTime = StartTime.AddDays(6);
            }
            if (a!=null &&a.ToString() == "NextWeek")
            {
                StartTime = StartTime.AddDays(7);
                EndTime = StartTime.AddDays(6);
            }
            if (a!=null &&a.ToString() == "LastMonth")
            {
                StartTime = DateTime.ParseExact(tempDate.Year.ToString() +tempDate.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(-1);
                EndTime = StartTime.AddMonths(1).AddDays(-1);
            }
            if (a!=null &&a.ToString() == "NextMonth")
            {
                StartTime = DateTime.ParseExact(tempDate.Year.ToString() +tempDate.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(1);
                EndTime = DateTime.ParseExact(tempDate.Year.ToString() +tempDate.Month.ToString("00") + "01", "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).AddMonths(2).AddDays(-1);
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
                    UpdateColor(obj, a.ToString());
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
            List<MyTime> allTimeData = await SQLCommands.GetAllTimeObjs(startTime, endTime);
            AllTimeViewObjs = await BuildTimeViewObj(allTimeData);
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
                if (StudyRB.IsChecked==true) refreshSummaryPlot("study");
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
                        obj.Height = CalculateHeight(obj.LastTime);
                    }
                }
                SummaryPlot.Height = LeftPanelHeight-250;
                SingleDayPlot.Height = LeftPanelHeight-250;
                SummaryPlot.Refresh();
                SingleDayPlot.Refresh();
            }
        }
        private async Task<ObservableCollection<GridSourceTemplate>> BuildTimeViewObj(List<MyTime> allTimeData)
        {
            DateTime currentDate = startTime;
            ObservableCollection<GridSourceTemplate> AllTimeViewObjs = new ObservableCollection<GridSourceTemplate>();
            
            while (currentDate<=endTime)
            {
                int lastIndex = 1;
                var currentDateTemplate = new GridSourceTemplate(currentDate);

                currentDateTemplate.Title = currentDate.ToShortDateString();
                currentDateTemplate.Week = currentDate.DayOfWeek.ToString();

                if (currentDateTemplate.Week.Equals("Saturday")||currentDateTemplate.Week.Equals("Sunday"))
                {
                    currentDateTemplate.Color = "#32CD32";
                }
                else
                {
                    currentDateTemplate.Color = "#008080";
                }
                TimeSpan endTime = new TimeSpan(6, 0, 0);
                List<MyTime> currentDateData = allTimeData.Where(x => x.createDate==currentDate&&x.startTime>=endTime).OrderBy(s => s.startTime).ToList<MyTime>();
                bool firstTimeObj = true;
                if(currentDateData.Count>0)
                {
                    lastIndex = currentDateData.Max(x => x.currentIndex)+1;
                }
                foreach (MyTime TimeObj in currentDateData)
                {
                    TimeViewObj timeViewObj = new TimeViewObj();
                    TimeSpan startTime = TimeSpan.Parse(TimeObj.startTime.ToString());
                    endTime = TimeSpan.Parse(TimeObj.endTime.ToString());
                    if (firstTimeObj)
                    {
                        firstTimeObj = false;
                        //Add first time object
                        TimeSpan tempStart = new TimeSpan(6, 0, 0);
                        if (startTime > tempStart)
                        {
                            TimeViewObj startTimeObj = CreateNewTimeObj(tempStart, TimeObj.startTime, "nothing", currentDate, TimeType.None, lastIndex);
                            lastIndex++;
                            await SQLCommands.AddObj(startTimeObj);
                            UpdateColor(startTimeObj, "none");
                            currentDateTemplate.DailyObjs.Add(startTimeObj);
                        }
                    }
                    if (startTime>=new TimeSpan(6, 0, 0))
                    {
                        timeViewObj.CreatedDate = currentDate;
                        timeViewObj.LastTime = TimeObj.endTime-TimeObj.startTime;
                        timeViewObj.Note = TimeObj.note;
                        timeViewObj.Height = CalculateHeight(endTime - startTime);
                        timeViewObj.StartTime = TimeObj.startTime;
                        timeViewObj.EndTime = TimeObj.endTime;
                        timeViewObj.Type = TimeObj.type.Trim()==""? "none": TimeObj.type.Trim();
                        timeViewObj.Id = TimeObj.currentIndex;
                        UpdateColor(timeViewObj, TimeObj.type.Trim().ToLower());

                    }
                    currentDateTemplate.DailyObjs.Add(timeViewObj);
                }
                //Add last obj
                //Add first time object
                TimeSpan tempEndTime = new TimeSpan(23, 59, 59);
                if (endTime < tempEndTime && currentDate<DateTime.Today)
                {
                    TimeViewObj startTimeObj = CreateNewTimeObj(endTime, tempEndTime, "nothing", currentDate, TimeType.None, lastIndex);
                    UpdateColor(startTimeObj, "none");
                    await SQLCommands.AddObj(startTimeObj);
                    currentDateTemplate.DailyObjs.Add(startTimeObj);
                }
                currentDate = currentDate.AddDays(1);
                AllTimeViewObjs.Add(currentDateTemplate);
            }
            return AllTimeViewObjs;
        }
        private void UpdateColor(TimeViewObj timeViewObj, string type)
        {
            switch (type.ToLower())
            {
                case "study":
                    timeViewObj.Color = "#FFB6C1";
                    break;
                case "waste":
                    timeViewObj.Color = "#F08080";
                    break;
                case "rest":
                    timeViewObj.Color = "#98FB98";
                    break;
                case "work":
                    timeViewObj.Color = "#FFD700";
                    break;
                case "none":
                    timeViewObj.Color = "#F3F3F3";
                    break;
                case "play":
                    timeViewObj.Color = "#ADD8E6";
                    break;
            }
        }
        private double CalculateHeight(TimeSpan lastTime)
        {
            TimeSpan allTimeSpan = new TimeSpan(18, 0, 0);
            return lastTime/allTimeSpan*(height-100);
        }
        public async void SplitTimeBlock(TimeSpan SplitTime, string content1, string content2){
            if(selectedTimeObj!=null){
                var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
                var lastIndex = currentDailyObj.Max(x=>x.Id) +1;
                var newTimeObj1 = CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, TimeType.None, lastIndex);
                lastIndex++;
                var newTimeObj2 = CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, TimeType.None, lastIndex);
                UpdateColor(newTimeObj1, TimeType.None.ToString());
                UpdateColor(newTimeObj2, TimeType.None.ToString());
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
        private TimeViewObj CreateNewTimeObj(TimeSpan startTime, TimeSpan endTime, string note, DateTime createDate, TimeType type, int index){
            TimeViewObj TimeObj = new TimeViewObj();
            TimeObj.CreatedDate = createDate;
            TimeObj.LastTime = endTime - startTime;
            TimeObj.Note = note;
            TimeObj.Height = CalculateHeight(TimeObj.LastTime);
            TimeObj.StartTime = startTime;
            TimeObj.EndTime = endTime;
            TimeObj.Type = type.ToString().ToLower();
            TimeObj.Id = index;
            return TimeObj;
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
