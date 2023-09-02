using MaterialDesignDemo.Domain;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Summary.Common.Utils.Helper;

namespace Summary.Models
{
    public class RecordModel : ViewModelBase
    {
        private double height;
        private double rightPanelHeight;
        public double RightPanelHeight
        {
            get { return rightPanelHeight; }
            set { rightPanelHeight = value; OnPropertyChanged(); }
        }
        private double gridHeight;
        public double GridHeight
        {
            get {
                return gridHeight;
            }
            set
            {
                gridHeight = value; OnPropertyChanged();
            }
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
        public TimeSpan CalculatedRemindTime = new TimeSpan();
        private bool accumulateModeCheck;

        public bool AccumulateModeCheck
        {
            get { return accumulateModeCheck; }
            set { accumulateModeCheck = value; OnPropertyChanged(); }
        }
        public DialogType dialogType { get; set; }
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
        public DataGrid TodayObjsGrid{ get; set; }
        public MyCommand Enter_ClickCommand { get; set; }
        public MyCommand DeleteContextMenu_ClickCommand { get; set; }
        public MyCommand TodayListBoxSelectionChangeCommand { get; set; }
        public MyCommand CheckChangedCommand { get; set; }
        public MyCommand SelectedCommand { get; set; }
        public MyCommand UpdateTypeCommand { get; set; }
        public MyCommand ResizeCommand { get; set; }
        public MyCommand StartCommand { get; set; }
        public MyCommand EndCommand { get; set; }
        public MyCommand WorkContentChangeCommand { get; set; }
        public MyCommand ImportCommand { get; set; }
        public MyCommand ExportCommand { get; set; }

        public int interval { get; set; }
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
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
        public MyCommand SingleDayRBChangedCommand { get; set; }
        private ObservableCollection<GridSourceTemplate> allTimeViewObjs;

        public ObservableCollection<GridSourceTemplate> AllTimeViewObjs
        {
            get { return allTimeViewObjs; }
            set { allTimeViewObjs = value; OnPropertyChanged(); }
        }
        private ObservableCollection<TimeViewObj> todayDailyObj;

        public ObservableCollection<TimeViewObj> TodayDailyObj
        {
            get { return todayDailyObj; }
            set { todayDailyObj = value; OnPropertyChanged(); }
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
        public MyCommand TextBoxLostFocusCommand { get; set; }
        public MyCommand CellEditEndingCommand { get; set; }
        public MyCommand IntervalTextBoxLostFocusCommand { get; set; }
        public MyCommand SloganTextBoxLostFocusCommand { get; set; }
        public MyCommand AccumulateModeCheckChangedCommand { get; set; }
        public MyCommand MinimizeCommand { get; set; }
        public MyCommand DeleteAllCommand { get; set; }
        public TimeViewObj SelectedTimeObj
        {
            get { return selectedTimeObj; }
            set
            {
                selectedTimeObj = value;
                OnPropertyChanged();
            }
        }
        private string slogan;

        public string Slogan
        {
            get { return slogan; }
            set { slogan = value; OnPropertyChanged(); }
        }

        private TimeSpan tickTime;

        public TimeSpan TickTime
        {
            get { return tickTime; }
            set { tickTime = value; OnPropertyChanged(); }
        }
        System.Timers.Timer showTextBoxTimer = new System.Timers.Timer(); //新建一个Timer对象
        private TimeSpan WorkStartTime = new TimeSpan();
        private TimeSpan CurrentWorkAccuTime = new TimeSpan();
        private bool DialogIsShown = false;

        private HashSet<string> hs = new HashSet<string>();
        public static ObservableCollection<string> TimeTypes = new ObservableCollection<string> { "none", "rest", "waste","play", "work", "study",};
      
        public RecordModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM) {
            Enter_ClickCommand = new MyCommand(Enter_Click);
            DeleteContextMenu_ClickCommand = new MyCommand(DeleteContextMenu);
            TodayListBoxSelectionChangeCommand = new MyCommand(TodayListBoxSelectionChange);
            CheckChangedCommand = new MyCommand(CheckChanged);
            SelectedCommand = new MyCommand(Selected);
            UpdateTypeCommand = new MyCommand(UpdateType);
            ResizeCommand = new MyCommand(resizeHeight);
            SplitButtonClickCommand = new MyCommand(SplitButtonClick);
            TextBoxLostFocusCommand = new MyCommand(TextBoxLostFocus);
            SingleDayRBChangedCommand = new MyCommand(SingleDayRBChanged);
            StartCommand = new MyCommand(StartClick);
            EndCommand = new MyCommand(EndClick);
            CellEditEndingCommand = new MyCommand(CellEditEnding);
            WorkContentChangeCommand = new MyCommand(WorkContentChange);
            IntervalTextBoxLostFocusCommand = new MyCommand(IntervalTextBoxLostFocus);
            SloganTextBoxLostFocusCommand = new MyCommand(SloganTextBoxLostFocus);
            AccumulateModeCheckChangedCommand = new MyCommand(AccumulateModeCheckChanged);
            DeleteAllCommand = new MyCommand(DeleteAll);
            ImportCommand = new MyCommand(ImportFile);
            ExportCommand = new MyCommand(ExportFile);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            InitTodayData();
            Interval = int.Parse(Helper.GetAppSetting("RemindTime"));
            AccumulateModeCheck = bool.Parse(Helper.GetAppSetting("AccuMode"));
            Slogan = Helper.GetAppSetting("Slogan");
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Elapsed += new ElapsedEventHandler(showTextBoxTimer_Tick);//到时所有执行的动作
            showTextBoxTimer.Start();//启动计时
        }
        public static void DeleteDirectory(string directoryPath, string fileName)
        {
            //删除文件
            for (int i = 0; i < Directory.GetFiles(directoryPath).ToList().Count; i++)
            {
                if (Directory.GetFiles(directoryPath)[i].Substring(directoryPath.Length+1) == fileName)
                {
                    File.Delete(Directory.GetFiles(directoryPath)[i]);
                }
            }
        }
        private void deleteFile(string filepath)
        {
            DeleteDirectory(filepath, "time_record.txt");
        }
       
        public static void addText(String name)
        {
            FileStream fs = new FileStream(Helper.GetAppSetting("OutputDirectory")+ "\\time_record.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(name);
            sw.Close();
        }
        private async void ExportFile(object obj)
        {
            if(File.Exists(Helper.GetAppSetting("OutputDirectory")+ "\\time_record.txt"))
            {
                deleteFile(Helper.GetAppSetting("OutputDirectory"));
            }
            if (!Directory.Exists(Helper.GetAppSetting("OutputDirectory")))
            {
                Directory.CreateDirectory(Helper.GetAppSetting("OutputDirectory"));
            }
            for (int i = 0; i < TodayDailyObj.Count; i++)
            {
                addText("开始时间：" + TodayDailyObj[i].StartTime);
                
                addText("间隔时间：" + (TodayDailyObj[i].EndTime - TodayDailyObj[i].StartTime));
                addText("结束时间：" + TodayDailyObj[i].EndTime);
                addText("类型：" + TodayDailyObj[i].Type);
                addText("备注：" + TodayDailyObj[i].Note);
                addText("");
            }
            await showMessageBox("导出成功！");
        }
        private async Task import()
        {
            var importDirectory = Helper.GetAppSetting("ImportDirectory");
            string text = System.IO.File.ReadAllText(importDirectory + "\\time_record.txt");
            string[] lines = text.Split(new char[2] { '\r', '\n' });
            TimeSpan startTime = new TimeSpan();
            TimeSpan stopTime = new TimeSpan();
            TimeType timeType = TimeType.none;
            string comment = "";
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("开始时间"))
                {
                    startTime = TimeSpan.Parse(lines[i].Substring(5, 8));
                }
                if (lines[i].StartsWith("结束时间"))
                {
                    stopTime = TimeSpan.Parse(lines[i].Substring(5, 8));
                }
                if (lines[i].StartsWith("类型"))
                {
                    timeType = Helper.ConvertTimeType(lines[i].Substring(3));
                }
                if (lines[i].StartsWith("备注"))
                {
                    comment = lines[i].Substring(3);
                    var newObj = Helper.CreateNewTimeObj(startTime, stopTime, comment, DateTime.Today, timeType, 1, height, "record");
                    await SQLCommands.AddObj(newObj);
                }
               
            }
            InitTodayData();
            resizeHeight();
        }
        private async void ImportFile(object obj)
        {
            var importDirectory = Helper.GetAppSetting("ImportDirectory");
            if (!File.Exists(importDirectory + "\\time_record.txt"))
            {
                await showMessageBox("未找到要导入的文件，请在设置中确认导入目录");
                return;
            }
            if (todayDailyObj!=null&&todayDailyObj.Count()>0)
            {
                YESNOWindow dialog = new YESNOWindow("提示","确定覆盖现在的时间块吗","确定","取消");
                if (dialog.ShowDialog() == true)
                {
                    if (Helper.WorkMode)
                    {
                        await showMessageBox("请先暂停工作");
                        return;
                    }
                    DeleteAllAfterCheck();
                    await import();
                }
            }
            else
            {
                await import();
            }
           
        }

        private async void DeleteAll(object obj)
        {
            if (Helper.WorkMode)
            {
                await showMessageBox("请先暂停工作");
                return;
            }
            showRemindDeleteDialog();
            
        }
        public async void DeleteAllAfterCheck()
        {
            await SQLCommands.DeleteObjByDate(DateTime.Today);
            initAllTimeViewObjs();
            refreshSingleDayPlot();
        }
        private void AccumulateModeCheckChanged(object obj)
        {
            Helper.SetAppSetting("AccuMode", AccumulateModeCheck.ToString());
        }

        private void SloganTextBoxLostFocus(object obj)
        {
            Helper.SetAppSetting("Slogan", Slogan);
        }

        private void IntervalTextBoxLostFocus(object obj)
        {
            Helper.SetAppSetting("RemindTime", Interval.ToString());
        }

        private void WorkContentChange(object obj)
        {
            calculateAccuTime();
            Helper.WorkContent = WorkContent;
        }

        private void SingleDayRBChanged(object obj)
        {
            refreshSingleDayPlot();
        }
        private async void updateTodayListAfterChangeType(TimeViewObj curr, string changedType){
            var TodayAllObjectWithSameNote = AllTimeViewObjs.First(x => x.createdDate == curr.CreatedDate).DailyObjs.Where(x => x.Note == curr.Note);
            foreach (var obj in TodayAllObjectWithSameNote)
            {
                obj.Type = changedType;
                Helper.UpdateColor(obj, changedType);
                await SQLCommands.UpdateObj(obj);
            }
            if (!hs.Contains(curr.Note) && (changedType == "work" || changedType == "study" || changedType == "play") && curr.Note != "")
            {
                ToDoObj newObj = new ToDoObj() { Note = curr.Note, Finished = false, Type = Helper.ConvertTimeType(curr.Type) };
                var id = await SQLCommands.AddTodo(newObj);
                newObj.Id = id;
                TodayList.Add(newObj);
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                hs.Add(curr.Note);
            }
            else if (hs.Contains(curr.Note) && !(changedType == "work" || changedType == "study" || changedType == "play"))
            {
                var item = TodayList.Where(x => x.Note == curr.Note);
                if (item != null && item.Count() > 0)
                {
                    hs.Remove(curr.Note);
                    await SQLCommands.DeleteTodo(item.First());
                    TodayList.Remove(item.First());
                }
            }
            refreshSingleDayPlot();
        }
        private async void UpdateType(object a)
        {
            if (SelectedTimeObj == null)
            {
                await showMessageBox("请先选中左侧要改的时间块");
                return;
            }
            updateTodayListAfterChangeType(SelectedTimeObj, a.ToString());
        }
        private  void CellEditEnding(object obj){
            //update note or type
            TimeViewObj curr = (TimeViewObj)obj;
            var updateNoteItem = AllTimeViewObjs.First().DailyObjs.First(x => x.Id == curr.Id);
            updateNoteItem.TimeNote = curr.TimeNote;
            updateTodayListAfterChangeType(updateNoteItem, curr.Type);
        }
        private async void TextBoxLostFocus(object obj)
        {
            //update note or type
            var updateTimeViewObj = (TimeViewObj)obj;
            await SQLCommands.UpdateObj(updateTimeViewObj);
            refreshSingleDayPlot();
        }
        private void refreshAllObjs(){
            UpdateGridData();
            refreshSingleDayPlot();
            resizeHeight();
        }
        public async void EndClick(object obj)
        {
            Helper.WorkMode = false;
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
            TimeType type = (TimeType)Enum.Parse(typeof(TimeType), findPreviousType(WorkContent));
            if(type == TimeType.none)
            {
                var item = TodayList.Where(x => x.Note==WorkContent);
                if (item!=null&&item.Count()>0)
                {
                    type = item.First().Type;
                }
            }
            var newObj = Helper.CreateNewTimeObj(WorkStartTime,Helper.getCurrentTime(), WorkContent, DateTime.Today, type, lastIndex, height, "record");
            await SQLCommands.AddObj(newObj);
            Helper.UpdateColor(newObj, type.ToString());
            currentDateTemplate.DailyObjs.Add(newObj);
            refreshAllObjs();
            //reset rest start time
            WorkStartTime = Helper.getCurrentTime();
        }
        private void calculateAccuTime()
        {
            CurrentWorkAccuTime = new TimeSpan(AllTimeViewObjs[0].DailyObjs.Where(x => x.Note == workContent).Sum(x => x.LastTime.Ticks));
        }
        private string findPreviousType(string note){
            var pre = AllTimeViewObjs[0].DailyObjs.Where(x => x.Note == note);
            if(pre.Count()>0){
                return pre.First().Type;
            }else{
                return "none";
            }
        }
        public async Task<bool> StartClickMethod()
        {
            Helper.WorkMode = true;
            if (WorkContent==null||WorkContent == "")
            {
                await showMessageBox("请先填写工作内容");
                return false;
            }
            
            WorkStartTime = Helper.getCurrentTime();
            calculateAccuTime();
            StartbtnEnabled = false;
            EndbtnEnabled = true;
            CalculatedRemindTime = new TimeSpan();
            if (AllTimeViewObjs!=null && AllTimeViewObjs.Count>0&&AllTimeViewObjs[0].DailyObjs!=null&&AllTimeViewObjs[0].DailyObjs.Count>0)
            {
                var lastViewObj = AllTimeViewObjs[0].DailyObjs.OrderBy(x => x.StartTime).LastOrDefault();
                int lastIndex = AllTimeViewObjs[0].DailyObjs.Max(x => x.Id)+1;
                var restCon = Helper.RestContent;
                if ( WorkStartTime - lastViewObj.EndTime > new TimeSpan(0,2,0)){
                    RemindWindow rw = new RemindWindow();
                    if(rw.ShowDialog()==true){
                        restCon = rw.InputTextBox.Text == "" ? restCon : rw.InputTextBox.Text;
                    }
                }
                TimeType type = (TimeType)Enum.Parse(typeof(TimeType), findPreviousType(restCon));
                var newObj = Helper.CreateNewTimeObj(lastViewObj.EndTime, WorkStartTime, restCon, DateTime.Today, type, lastIndex, height, "record");
                await SQLCommands.AddObj(newObj);
                Helper.UpdateColor(newObj, type.ToString());
                AllTimeViewObjs[0].DailyObjs.Add(newObj);
            }
            else
            {
                var currentDateTemplate = initAllTimeViewObjs();
                if (Helper.getCurrentTime() > Helper.GlobalStartTimeSpan)
                {
                    var newObj = Helper.CreateNewTimeObj(Helper.GlobalStartTimeSpan, WorkStartTime, Helper.RestContent, DateTime.Today, TimeType.rest, 1, height, "record");
                    await SQLCommands.AddObj(newObj);
                    Helper.UpdateColor(newObj, "rest");

                    currentDateTemplate.DailyObjs.Add(newObj);
                }
                else
                {
                    Helper.GlobalStartTimeSpan = Helper.getCurrentTime();
                }
            }
            refreshAllObjs();
            return true;
        }
        public async void StartClick(object obj)
        {
            await StartClickMethod();
        }
        
        private void showTextBoxTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan now = Helper.getCurrentTime();
            TimeSpan totalSpan = now - WorkStartTime;
            
            if (AccumulateModeCheck&&endbtnEnabled)
            {
                totalSpan = new TimeSpan(totalSpan.Ticks + CurrentWorkAccuTime.Ticks);
            }
            TickTime = totalSpan;
            Helper.TickTime = TickTime;
            if(TickTime > new TimeSpan(23,59,58) && Helper.WorkMode)
            {
                EndClick(null);
            }
            if(Helper.WorkMode)
            {
                CalculatedRemindTime = CalculatedRemindTime.Add(new TimeSpan(0, 0, 1));
                if (CalculatedRemindTime > new TimeSpan(0, Interval, 0) && !Helper.MiniWindowShow)
                {
                    CalculatedRemindTime = new TimeSpan();
                    if (!Helper.MiniWindowShow)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            showRemindDialog();
                        }));
                    }
                }
            }
        }

       
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
            UpdateGridData();
            return currentDateTemplate;
        }
        public async Task showMessageBox(string message)
        {
            if (!DialogIsShown)
            {
                DialogIsShown = true;
                dialogType = DialogType.MessageDialog;
                var view = new SampleMessageDialog(message);
                await DialogHost.Show(view, "SubRootDialog");
                DialogIsShown = false;
            }
        }
        public async void showRemindDialog()
        {
            if (!DialogIsShown)
            {
                DialogIsShown = true;
                dialogType = DialogType.OkCancelDialog;
                var view = new RemindDialog("心态好最重要呀", "已经工作好一会了，休息一下眼睛更好哦", "休息", "继续");
                await DialogHost.Show(view, "SubRootDialog");
                DialogIsShown = false;
            }
        }
        public async void showRemindDeleteDialog()
        {
            if (!DialogIsShown)
            {
                DialogIsShown = true;
                dialogType = DialogType.DeleteTodayTimeDialog;
                var view = new RemindDialog("提示", "确认删除今日所有时间块吗？","取消","确定");
                await DialogHost.Show(view, "SubRootDialog");
                DialogIsShown = false;
            }
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
            if (!DialogIsShown)
            {
                DialogIsShown = true;
                dialogType = DialogType.SplitDialog;
                var view = new SampleDialog(SelectedTimeObj, sampleDialogViewModel);
                await DialogHost.Show(view, "SubRootDialog");
                DialogIsShown = false;
            }
        }
        public async void SplitTimeBlock(TimeSpan SplitTime, string content1, string content2)
        {
            if (selectedTimeObj!=null)
            {
                var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
                var lastIndex = currentDailyObj.Max(x => x.Id) +1;
                var newTimeObj1 = Helper.CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, TimeType.none, lastIndex, height);
                lastIndex++;
                var newTimeObj2 = Helper.CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, TimeType.none, lastIndex, height);
                Helper.UpdateColor(newTimeObj1, TimeType.none.ToString());
                Helper.UpdateColor(newTimeObj2, TimeType.none.ToString());
                
                currentDailyObj.Add(newTimeObj1);
                currentDailyObj.Add(newTimeObj2);
                currentDailyObj.Remove(selectedTimeObj);
                AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
                UpdateGridData();
                refreshSingleDayPlot();
                resizeHeight();
                await SQLCommands.DeleteObj(selectedTimeObj);
                await SQLCommands.AddObj(newTimeObj1);
                await SQLCommands.AddObj(newTimeObj2);
                SelectedTimeObj = newTimeObj1;
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
            UpdateGridData();
            var AllTodayTasks = SQLCommands.GetTasks(DateTime.Today);
            //加载todayTaks里的work,study,play时间块
            if (AllTodayTasks.Count() > 0)
            {
                foreach (var obj in AllTodayTasks)
                {
                    if (obj.Type == "study" || obj.Type == "work" || obj.Type == "play")
                    {
                        if (!hs.Contains(obj.Note)&&obj.Note!="")
                        {
                            ToDoObj newObj = new ToDoObj() { Note = obj.Note, Finished = obj.Finished, Type = Helper.ConvertTimeType(obj.Type),Id = obj.Id };
                            TodayList.Add(newObj);
                            hs.Add(obj.Note);
                        }
                    }
                }
            }
            //把DailyObj里的work，study，play时间块加入到todaylist
            if (AllTimeViewObjs.Count() > 0 && AllTimeViewObjs[0].DailyObjs != null)
            {
                foreach(var obj in AllTimeViewObjs[0].DailyObjs)
                {
                    if(obj.Type == "study" || obj.Type == "work"||obj.Type=="play")
                    {
                        if (!hs.Contains(obj.Note)&& obj.Note != "")
                        {
                            ToDoObj newObj = new ToDoObj() { Note = obj.Note, Finished = false, Type=Helper.ConvertTimeType(obj.Type) };
                            var id = await SQLCommands.AddTodo(newObj);
                            newObj.Id = id;
                            TodayList.Add(newObj);
                            hs.Add(obj.Note);
                        }
                    }
                }
            }
            
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
        }
        private void UpdateGridData()
        {
            if (AllTimeViewObjs.Count() > 0 && AllTimeViewObjs[0].DailyObjs != null)
            {
                TodayDailyObj = AllTimeViewObjs[0].DailyObjs;
                
            }
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
        private async void Enter_Click(object obj)
        {
            if(obj.ToString() == ""){
                return;
            }
            if(!hs.Contains(obj.ToString())){
                ToDoObj newObj = new ToDoObj() { Note = obj.ToString(), Finished = false, Type=TimeType.work };
                var index = await SQLCommands.AddTodo(newObj);
                newObj.Id = index;
                hs.Add(obj.ToString());
                TodayList.Add(newObj);
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                TodayText = "";
            }else{
                TodayText = "";
                await showMessageBox("已存在这个任务");
            }
        }
        private void DeleteContextMenu(object obj)
        {
            if(currentObj != null)
            {
                TodayList.Remove(currentObj);
                SQLCommands.DeleteTodo(currentObj);
                hs.Remove(currentObj.Note);
                currentObj = null;
            }
            
        }
        private void TodayListBoxSelectionChange(object obj)
        {
            if(SelectedListItem != null)
            {
                currentObj= SelectedListItem;
                WorkContent = SelectedListItem.Note;
                Helper.WorkContent = WorkContent;
                SelectedListItem = null;
                WorkContentChange(null);
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
                    GridHeight = RightPanelHeight -280;
                }
                
                foreach (var gridSource in AllTimeViewObjs)
                {
                    foreach (var obj in gridSource.DailyObjs)
                    {
                        obj.Height = CalculateRecordHeight(obj.LastTime);
                    }
                }
            }
        }
        private double CalculateRecordHeight(TimeSpan lastTime)
        {
            if (AllTimeViewObjs != null && AllTimeViewObjs[0].DailyObjs.Count>0)
            {
                TimeSpan allTimeSpan = AllTimeViewObjs[0].DailyObjs.OrderBy(x=>x.EndTime).Last().EndTime - new TimeSpan(6, 0, 0);
                return lastTime/allTimeSpan*(height-95);
            }
            else
            {
                return 0;
            }
        }
        private async void CheckChanged(object obj) {
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
            await SQLCommands.UpdateTodo((ToDoObj)obj);
        }
        
    }
    
}
