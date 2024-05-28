using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;
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
using System.Windows.Data;
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
        public ComboBox TodoToday { get; set; }
        public TextBox TodoTodayTextbox { get; set; }
        private double height;

        public decimal EstimateTime
        {
            get { return Helper.EstimateTime; }
            set { Helper.EstimateTime = value; OnPropertyChanged(); }
        }

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
        /// <summary>
        /// TodayList is left panel's todolist's data source
        /// </summary>
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
        public MyCommand TodayListBoxRightClickCommand { get; set; }
        public MyCommand CheckChangedCommand { get; set; }
        public MyCommand SelectedCommand { get; set; }
        public MyCommand UpdateTypeCommand { get; set; }
        public MyCommand ResizeCommand { get; set; }
        public MyCommand StartCommand { get; set; }
        public MyCommand EndCommand { get; set; }
        public MyCommand WorkContentChangeCommand { get; set; }
        public MyCommand ImportCommand { get; set; }
        public MyCommand ExportCommand { get; set; }
        public MyCommand MergeCommand { get; set; }

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
        /// <summary>
        /// todayDailyObj is right panel's data source
        /// </summary>
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
        public MyCommand TipTextChangeCommand { get; set; }
        public MyCommand TipTextPreviewMouseUpCommand { get; set; }
        public MyCommand TodoTodaySelectionChangeCommand { get; set; }
        public MyCommand SummaryRBChangedCommand { get; set; }
        public MyCommand EstimateContentChangeCommand { get; set; }
        public MyCommand DownKey_Command { get; set; }
        public MyCommand UpKey_Command { get; set; }
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
        public static ObservableCollection<string> TestCategory = new ObservableCollection<string>();
        private HashSet<string> hs = new HashSet<string>();
        public static ObservableCollection<string> TimeTypes = new ObservableCollection<string> { "none", "rest", "waste","play", "work", "invest", };
        
        private ObservableCollection<string> tipList;

        public ObservableCollection<string> TipList
        {
            get { return tipList; }
            set { tipList = value; OnPropertyChanged(); }
        }

        public StackPanel RightButtonPanel { get; internal set; }
        public System.Windows.Style ButtonStyle { get; internal set; }
        public WrapPanel TypeRadioGroupPanel { get; internal set; }

        public List<RadioButton> RadioButtons { get; internal set; } = new List<RadioButton>();
        public Dictionary<string,decimal> EstimateDic=new Dictionary<string,decimal>();
        public bool ClickUpOrDown =false;
        public RecordModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM) {
            Enter_ClickCommand = new MyCommand(Enter_Click);
            DownKey_Command = new MyCommand(DownKeySub);
            UpKey_Command = new MyCommand(UpKeySub);
            DeleteContextMenu_ClickCommand = new MyCommand(DeleteContextMenu);
            TodayListBoxSelectionChangeCommand = new MyCommand(TodayListBoxSelectionChange);
            TodayListBoxRightClickCommand = new MyCommand(TodayListBoxRightClick);
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
            MergeCommand = new MyCommand(Merge);
            TipTextChangeCommand = new MyCommand(TipTextChange);
            TipTextPreviewMouseUpCommand = new MyCommand(TipTextPreviewMouseUp);
            TodoTodaySelectionChangeCommand = new MyCommand(TodoTodaySelectionChange);
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
            EstimateContentChangeCommand = new MyCommand(EstimateContentChange);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            Interval = int.Parse(Helper.GetAppSetting("RemindTime"));
            AccumulateModeCheck = bool.Parse(Helper.GetAppSetting("AccuMode"));
            Slogan = Helper.GetAppSetting("Slogan");
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Elapsed += new ElapsedEventHandler(showTextBoxTimer_Tick);//到时所有执行的动作
            showTextBoxTimer.Start();//启动计时
        }

        private void TodayListBoxRightClick(object obj)
        {
            if (SelectedListItem != null)
            {
                currentObj = SelectedListItem;
            }
        }

        private void EstimateContentChange(object obj)
        {
            if (!string.IsNullOrEmpty(workContent))
            {
                if (EstimateDic.ContainsKey(workContent))
                {
                    EstimateDic[workContent] = decimal.Parse(obj.ToString());
                }
                else
                {
                    EstimateDic.Add(workContent, decimal.Parse(obj.ToString()));
                }
            }
        }

        private void UpKeySub(object obj)
        {   
            ClickUpOrDown = true;
            if(TodoToday!=null && TodoToday.IsDropDownOpen && TodoToday.Items.Count>0){
                if(TodoToday.SelectedIndex-1>=0){
                    TodoToday.SelectedIndex--;
                    TodoTodayTextbox.Text = TodoToday.SelectedItem.ToString();
                }
            }
        }

        private void DownKeySub(object obj)
        {
            ClickUpOrDown = true;
            if(TodoToday!=null && TodoToday.IsDropDownOpen  && TodoToday.Items.Count>0){
                if(TodoToday.SelectedIndex+1<TodoToday.Items.Count){
                    TodoToday.SelectedIndex++;
                    TodoTodayTextbox.Text = TodoToday.SelectedItem.ToString();
                }
            }
        }

        private  void SummaryRBChanged(object obj)
        {
            refreshSingleDayPlot();
        }

        public void RefreshRadioButtons()
        {
            if (TypeRadioGroupPanel!=null)
            {
                TypeRadioGroupPanel.Children.Clear();
                Label label = new Label();
                label.Margin = new Thickness(10, 0, 10, 0);
                label.Content = "Type:";
                label.FontSize=14;
                TypeRadioGroupPanel.Children.Add(label);
                int maxDepth = Helper.getMaxDepth(1, 0);
                RadioButtons.Clear();
                for (int i = 0; i < maxDepth; i++)
                {
                    RadioButton AllRadioButton = new RadioButton();
                    AllRadioButton.FontSize = 14;
                    AllRadioButton.Name = "RB" + i.ToString();
                    AllRadioButton.GroupName = "SingleDayType2";
                    AllRadioButton.Margin = new Thickness(5, 5, 5, 5);
                    AllRadioButton.Command = SummaryRBChangedCommand;
                    AllRadioButton.CommandParameter = (i+1).ToString();
                    AllRadioButton.Content = "层级" + (i + 1).ToString();
                    //if (i == 0) AllRadioButton.IsChecked = true;
                    TypeRadioGroupPanel.Children.Add(AllRadioButton);
                    RadioButtons.Add(AllRadioButton);
                }
                if (RadioButtons.Count()>0)
                {
                    RadioButtons[0].IsChecked = true;
                }
            }
        }
        private void TodoTodaySelectionChange(object obj)
        {
            if (TodoToday.SelectedValue!=null)
            {
                TodayText = TodoToday.SelectedValue.ToString();
            }
        }

        private void TipTextPreviewMouseUp(object obj)
        {
            List<GeneratedToDoTask> allTasks = SQLCommands.GetTasks(new DateTime(1900, 1, 1), DateTime.Today);
            var ValidTasks = Helper.mainCategories.Where(x => x.AutoAddTask).Select(x=>x.Id);
            if (TodoTodayTextbox.Text==null||TodoTodayTextbox.Text=="")
            {
                TipList = new ObservableCollection<string>(allTasks.Where(x=>ValidTasks.Contains(Helper.mainCategories.FirstOrDefault(y=>y.Id==x.TypeId,new Category(){ Id=0}).Id)).OrderByDescending(x => x.UpdatedDate).Take(10).Select(x => x.Note).Distinct().ToList());
            }
            else
            {
                TipList = new ObservableCollection<string>(allTasks.Where(x => ValidTasks.Contains((Helper.mainCategories.FirstOrDefault(y => y.Id == x.TypeId, new Category() { Id = 0 }).Id)) && x.Note.Contains(TodoTodayTextbox.Text)).OrderByDescending(x => x.UpdatedDate).Select(x => x.Note).Distinct().ToList());
            }
            TodoToday.ItemsSource = TipList;
            TodoToday.IsDropDownOpen=true;
            TodoTodayTextbox.Focus();
        }

        private void TipTextChange(object obj)
        {
            if (TodoTodayTextbox.Text!=null && !ClickUpOrDown)
            {
                List<GeneratedToDoTask> allTasks = SQLCommands.GetTasks(new DateTime(1900, 1, 1), DateTime.Today);
                if (TodoTodayTextbox.Text=="")
                {
                    TipList = new ObservableCollection<string>(allTasks.Where(x =>Helper.mainCategories.FirstOrDefault(y => y.Id == x.TypeId, new Category() { AutoAddTask = false }).AutoAddTask==true).OrderByDescending(x => x.CreateDate).Take(10).Select(x => x.Note).Distinct().ToList());
                }
                else
                {
                    TipList = new ObservableCollection<string>(allTasks.Where(x => ((Helper.mainCategories.FirstOrDefault(y => y.Id == x.TypeId, new Category() { AutoAddTask = false }).AutoAddTask == true) || x.TypeId== 0) &&x.Note.Contains(TodoTodayTextbox.Text)).OrderByDescending(x => x.CreateDate).Select(x => x.Note).Distinct().ToList());
                }
                TodoToday.ItemsSource = TipList;
                TodoToday.IsDropDownOpen=true;
            }
            if(ClickUpOrDown) {
                ClickUpOrDown = false;
            }
        }
        public void initCategoryDic()
        {
            RightButtonPanel.Children.Clear();
            List<Category> categories = SQLCommands.GetAllCategories().Result.ToList();
            //categoryDic为了后续快速获取这几个主要任务的id
            List<Category> mainCategories = categories.Where(x => x.ParentCategoryId==0).ToList();
            TestCategory.Clear();
            TestCategory.Add("none");
            foreach (Category category in mainCategories)
            {
                if (!category.Visible) continue;
                TestCategory.Add(category.Name);
                Button button = new Button();
                button.Content = category.Name;
                BrushConverter brushConverter = new BrushConverter();
                button.Background = (Brush)brushConverter.ConvertFromString(category.Color);
                button.Style = ButtonStyle;
                button.Command = UpdateTypeCommand;
                button.CommandParameter = category.Name;
                RightButtonPanel.Children.Add(button);
            }
        }

        private async void Merge(object obj)
        {
            if(selectedTimeObj == null){
                await showMessageBox("请先选中左侧要改的时间块");
                return;
            }
            var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
            if (obj.ToString()=="up"){
                var aboveItemList = currentDailyObj.Where(x => x.EndTime == selectedTimeObj.StartTime);
                if (aboveItemList.Count() > 0)
                {
                    var aboveItem = aboveItemList.First();
                    YESNOWindow dialog = new YESNOWindow("提示", $"确定合并时间块 {selectedTimeObj.Note} 和 {aboveItem.Note} 为 {selectedTimeObj.Note} 吗", "确定", "取消");
                    if (dialog.ShowDialog() == true)
                    {
                        var updatedStartTime = aboveItem.StartTime;
                        SelectedTimeObj.StartTime = updatedStartTime;
                        SelectedTimeObj.LastTime = SelectedTimeObj.EndTime- SelectedTimeObj.StartTime;
                        await SQLCommands.DeleteObj(aboveItem);
                        await SQLCommands.UpdateObj(SelectedTimeObj);
                        currentDailyObj.Remove(aboveItem);
                    }
                }
            }else{
                var downItemList = currentDailyObj.Where(x => x.StartTime == selectedTimeObj.EndTime);
                if (downItemList.Count() > 0)
                {
                    var downItem = downItemList.First();
                    YESNOWindow dialog = new YESNOWindow("提示", $"确定合并时间块 {selectedTimeObj.Note} 和 {downItem.Note} 为 {selectedTimeObj.Note} 吗", "确定", "取消");
                    if (dialog.ShowDialog() == true)
                    {
                        var updatedEndTime = downItem.EndTime;
                        SelectedTimeObj.EndTime = updatedEndTime;
                        SelectedTimeObj.LastTime = SelectedTimeObj.EndTime - SelectedTimeObj.StartTime;
                        await SQLCommands.DeleteObj(downItem);
                        await SQLCommands.UpdateObj(SelectedTimeObj);
                        currentDailyObj.Remove(downItem);
                    }
                }
            }
            AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
            refreshAllObjs();
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
                try
                {
                    deleteFile(Helper.GetAppSetting("OutputDirectory"));
                }
                catch (Exception)
                {

                    await showMessageBox("删除旧记录时出错，请检查导出目录是否存在");
                    return;
                }
                
            }
            if (!Directory.Exists(Helper.GetAppSetting("OutputDirectory")))
            {
                try
                {
                    Directory.CreateDirectory(Helper.GetAppSetting("OutputDirectory"));
                }
                catch (Exception)
                {

                    await showMessageBox("创建导出目录时出错，请检查导出目录是否存在");
                    return;
                }
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
            string timeType = "none";
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
                    timeType = lines[i].Substring(3);
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
            refreshSingleDayPlot();
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
            if (!string.IsNullOrEmpty(workContent))
            {
                if (EstimateDic.ContainsKey(workContent))
                {
                    EstimateTime = EstimateDic[workContent];
                }
                else
                {
                    EstimateTime = 0;
                }
            }
        }

        private void SingleDayRBChanged(object obj)
        {
            refreshSingleDayPlot();
        }
        private async Task updateTodayListAfterChangeType(TimeViewObj curr, string changedType){
            var TodayAllObjectWithSameNote = AllTimeViewObjs.First(x => x.createdDate == curr.CreatedDate).DailyObjs.Where(x => x.Note == curr.Note);
            GeneratedToDoTask findTask = SQLCommands.QueryTodo(curr.Note);
            if(findTask != null)
            {
                findTask.TypeId = Helper.NameIdDic[changedType];
                findTask.CategoryId = 0;
                await SQLCommands.UpdateTodo(findTask);
            }
            foreach (var obj in TodayAllObjectWithSameNote)
            {
                obj.Type = changedType;
                Helper.UpdateColor(obj, changedType);
                await SQLCommands.UpdateObj(obj);
            }
            if (!hs.Contains(curr.Note) && Helper.mainCategories.FirstOrDefault(x => x.Name==changedType, new Category() { AutoAddTask=false }).AutoAddTask&& curr.Note != ""&&curr.Type!="none")
            {
                ToDoObj newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = curr.Note, Finished = false, Type = curr.Type, CategoryId= categoryDic[changedType] };
                var id = await SQLCommands.AddTodo(newObj);
                newObj.Id = id;
                TodayList.Add(newObj);
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                hs.Add(curr.Note);
            }
            else if (hs.Contains(curr.Note) && !(Helper.mainCategories.FirstOrDefault(x => x.Name == changedType, new Category() { AutoAddTask = false }).AutoAddTask))
            {
                var item = TodayList.Where(x => x.Note == curr.Note);
                if (item != null && item.Count() > 0)
                {
                    item.First().Type = changedType;
                    await SQLCommands.UpdateTodo(item.First());
                    hs.Remove(curr.Note);
                    await CheckAndDeleteToDo(item.First());
                    TodayList.Remove(item.First());
                }
            }else if(curr.Type!="none"){
                ToDoObj newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = curr.Note, Finished = false, Type = curr.Type, CategoryId = categoryDic[changedType] };
                var id = await SQLCommands.AddTodo(newObj);
            }
            refreshSingleDayPlot();
        }
        private async Task CheckAndDeleteToDo(ToDoObj objTobeDeleted)
        {
            var objs = SQLCommands.GetTimeObjsByName(objTobeDeleted.Note);
            if (objs.Count()==0||objTobeDeleted.Type=="none"||objTobeDeleted.Type=="")
            {
                await SQLCommands.DeleteTodo(objTobeDeleted);
            }
            else
            {
                objTobeDeleted.CreatedDate = objs.FirstOrDefault().createDate;
                await SQLCommands.UpdateTodo(objTobeDeleted);
            }
        }
        private async void UpdateType(object a)
        {
            if (SelectedTimeObj == null)
            {
                await showMessageBox("请先选中左侧要改的时间块");
                return;
            }
            await updateTodayListAfterChangeType(SelectedTimeObj, a.ToString());
        }
        private async void CellEditEnding(object obj){
            //update note or type
            TimeViewObj curr = (TimeViewObj)obj;
            var updateNoteItem = AllTimeViewObjs.First().DailyObjs.First(x => x.Id == curr.Id);
            updateNoteItem.TimeNote = curr.TimeNote;

            if (colorDic[updateNoteItem.Type]!=updateNoteItem.Color)
            {
                //Type updated
                await updateTodayListAfterChangeType(updateNoteItem, curr.Type);
            }
            else
            {
                //note updated
                await updateTodayListAfterChangeNote(updateNoteItem);
            }
        }
        private async Task updateTodayListAfterChangeNote(TimeViewObj curr)
        {
            GeneratedToDoTask findTask = SQLCommands.QueryTodo(curr.Note);
            if (findTask != null)
            {
                curr.Type =IdCategoryDic.ContainsKey(findTask.TypeId)?IdCategoryDic[findTask.TypeId]:"none";
                Helper.UpdateColor(curr, curr.Type);
                await SQLCommands.UpdateObj(curr);
            }
            else
            {
                await SQLCommands.UpdateObj(curr);
            }
            string changedType = curr.Type;
            if (!hs.Contains(curr.Note) && Helper.mainCategories.FirstOrDefault(x => x.Name==changedType, new Category() { AutoAddTask=false }).AutoAddTask&& curr.Note != ""&&curr.Type!="none")
            {
                ToDoObj newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = curr.Note, Finished = false, Type = curr.Type, CategoryId= categoryDic[changedType] };
                var id = await SQLCommands.AddTodo(newObj);
                newObj.Id = id;
                TodayList.Add(newObj);
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                hs.Add(curr.Note);
            }
            else if (hs.Contains(curr.Note) && !(Helper.mainCategories.FirstOrDefault(x => x.Name == changedType, new Category() { AutoAddTask = false }).AutoAddTask))
            {
                var item = TodayList.Where(x => x.Note == curr.Note);
                if (item != null && item.Count() > 0)
                {
                    await SQLCommands.UpdateTodo(item.First());
                    hs.Remove(curr.Note);
                    await CheckAndDeleteToDo(item.First());
                    TodayList.Remove(item.First());
                }
            }
            else if (curr.Type!="none")
            {
                ToDoObj newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = curr.Note, Finished = false, Type = curr.Type, CategoryId = categoryDic[changedType] };
                var id = await SQLCommands.AddTodo(newObj);
            }
            refreshSingleDayPlot();
        }
        private async void TextBoxLostFocus(object obj)
        {
            //update note
            var updateTimeViewObj = (TimeViewObj)obj;
            await updateTodayListAfterChangeNote(updateTimeViewObj);
            
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
            string type = findPreviousType(WorkContent);
            int taskId = 0;
            if(type == "none")
            {
                var item = TodayList.Where(x => x.Note==WorkContent);
                if (item!=null&&item.Count()>0)
                {
                    type = item.First().Type;
                    taskId = item.First().Id;
                }
                else
                {
                    var findTask = SQLCommands.QueryTodo(WorkContent);
                    int typeId = findTask!=null ? findTask.TypeId : 0;
                    type = IdCategoryDic.ContainsKey(typeId) ? IdCategoryDic[typeId] : "none";
                    taskId = findTask==null ? 0 : findTask.Id;
                }
            }
            var newObj = Helper.CreateNewTimeObj(WorkStartTime,Helper.getCurrentTime(), WorkContent, DateTime.Today, type, lastIndex, height, "record", taskId);
            await SQLCommands.AddObj(newObj);
            Helper.UpdateColor(newObj, type.ToString());
            currentDateTemplate.DailyObjs.Add(newObj);
            refreshAllObjs();
            //reset rest start time
            WorkStartTime = Helper.getCurrentTime();
            if(Helper.getCurrentTime()>=new TimeSpan(23, 59, 58))
            {
                WorkStartTime = new TimeSpan(0, 0, 0);
            }
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
            if (DateTime.Now.TimeOfDay <Helper.GlobalStartTimeSpan)
            {
                await showMessageBox("未到设定起始记录时间，请到设置页面重新设置起始记录时间");
                return false;
            }
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
                if ( WorkStartTime - lastViewObj.EndTime > Helper.intervalRemindTimeSpan)
                {
                    RemindWindow rw = new RemindWindow();
                    if(rw.ShowDialog()==true){
                        restCon = rw.InputTextBox.Text == "" ? restCon : rw.InputTextBox.Text;
                    }
                }
                
                GeneratedToDoTask findTask = SQLCommands.QueryTodo(restCon);
                int typeId = findTask!=null?findTask.TypeId:0;
                string type = IdCategoryDic.ContainsKey(typeId)?IdCategoryDic[typeId]:"none";
                int taskId = findTask==null? 0:findTask.Id;
                var newObj = Helper.CreateNewTimeObj(lastViewObj.EndTime, WorkStartTime, restCon, DateTime.Today, type, lastIndex, height, "record", taskId);
                await SQLCommands.AddObj(newObj);
                Helper.UpdateColor(newObj, type.ToString());
                AllTimeViewObjs[0].DailyObjs.Add(newObj);
            }
            else
            {
                var currentDateTemplate = initAllTimeViewObjs();
                if (Helper.getCurrentTime() > Helper.GlobalStartTimeSpan)
                {
                    GeneratedToDoTask findTask = SQLCommands.QueryTodo(Helper.RestContent);
                    int taskId = findTask==null ? 0 : findTask.Id;
                    string type = Helper.IdCategoryDic[findTask!=null ? findTask.TypeId : 0];
                    var newObj = Helper.CreateNewTimeObj(Helper.GlobalStartTimeSpan, WorkStartTime, Helper.RestContent, DateTime.Today, type, 1, height, "record", taskId);
                    await SQLCommands.AddObj(newObj);
                    Helper.UpdateColor(newObj, type);

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
            if(totalSpan.Ticks<0){
                //new day
                WorkStartTime = new TimeSpan();
                InitTodayData();
                refreshSingleDayPlot();
            }
        }

       
        public GridSourceTemplate initAllTimeViewObjs()
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
                GeneratedToDoTask findTask = SQLCommands.QueryTodo(content1);
                string type = "none";
                int taskId = findTask==null ? 0 : findTask.Id;
                if (taskId==0)
                {
                    ToDoObj newObj = new ToDoObj() { CreatedDate = SelectedTimeObj.CreatedDate, Note = content1, Finished = false, Type = "none", CategoryId = 0 };
                    taskId = await SQLCommands.AddTodo(newObj);
                }else{
                    if(Helper.IdCategoryDic.ContainsKey(findTask.TypeId))
                        type = Helper.IdCategoryDic[findTask.TypeId];
                }
                var newTimeObj1 = Helper.CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, type, lastIndex, height, taskId:taskId);
                Helper.UpdateColor(newTimeObj1, type);
                lastIndex++;
                taskId = 0;
                type = "none";
                if (content2!="")
                {
                    findTask = SQLCommands.QueryTodo(content2);
                    taskId = findTask==null ? 0 : findTask.Id;
                    if (taskId==0)
                    {
                        ToDoObj newObj = new ToDoObj() { CreatedDate = SelectedTimeObj.CreatedDate, Note = content2, Finished = false, Type = "none", CategoryId = 0 };
                        taskId = await SQLCommands.AddTodo(newObj);
                    }else{
                        if(Helper.IdCategoryDic.ContainsKey(findTask.TypeId))
                            type = Helper.IdCategoryDic[findTask.TypeId];
                    }
                }
                var newTimeObj2 = Helper.CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, type, lastIndex, height, taskId: taskId);
                
                Helper.UpdateColor(newTimeObj2, type);
                
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
        public async void InitTodayData()
        {
            AllTimeViewObjs = await Helper.BuildTimeViewObj(DateTime.Today, DateTime.Today, SQLCommands, height,"record");
            UpdateGridData();
            var AllTodayTasks = SQLCommands.GetTasks(DateTime.Today, DateTime.Today);
            //加载todayTaks里的work,invest,play时间块
            if (AllTodayTasks.Count() > 0)
            {
                foreach (var obj in AllTodayTasks)
                {
                    if (Helper.mainCategories.FirstOrDefault(x=>x.Id==obj.TypeId, new Category() { AutoAddTask=false}).AutoAddTask||obj.TypeId==0)
                    {
                        if (!hs.Contains(obj.Note)&&obj.Note!="")
                        {
                            ToDoObj newObj = new ToDoObj() { 
                                CreatedDate = DateTime.Today, 
                                Note = obj.Note, 
                                Finished = obj.Finished, 
                                Type = Helper.mainCategories.FirstOrDefault(x => x.Id == obj.TypeId, new Category() { Name = "" }).Name,
                                Id = obj.Id };
                            TodayList.Add(newObj);
                            hs.Add(obj.Note);
                        }
                    }
                }
            }
            //把DailyObj里的work，invest，play时间块加入到todaylist
            if (AllTimeViewObjs.Count() > 0 && AllTimeViewObjs[0].DailyObjs != null)
            {
                foreach(var obj in AllTimeViewObjs[0].DailyObjs)
                {
                    if(Helper.mainCategories.FirstOrDefault(x => x.Name==obj.Type, new Category() { AutoAddTask=false }).AutoAddTask)
                    {
                        if (!hs.Contains(obj.Note)&& obj.Note != ""&&obj.Type!="none")
                        {
                            ToDoObj newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = obj.Note, Finished = false, Type=obj.Type, CategoryId = categoryDic[obj.Type] };
                            var id = await SQLCommands.AddTodo(newObj);
                            newObj.Id = id;
                            TodayList.Add(newObj);
                            hs.Add(obj.Note);
                        }
                    }
                }
            }
            
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
            List<GeneratedToDoTask> allTasks = SQLCommands.GetTasks(new DateTime(1900, 1, 1), DateTime.Today);
            TipList = new ObservableCollection<string>(allTasks.Where(x => Helper.mainCategories.FirstOrDefault(y => y.Id == x.TypeId, new Category() { AutoAddTask = false }).AutoAddTask == true).OrderByDescending(x=>x.CreateDate).Take(10).Select(x => x.Note).ToList());
            resizeHeight();
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
            foreach (RadioButton radioButton in RadioButtons)
            {
                radioButton.Dispatcher.Invoke(new Action(async delegate
                {
                    if (radioButton.IsChecked==true)
                    {
                        List<ToDoObj> allTasks = new List<ToDoObj>();
                        allTasks = TodayDailyObj.Where(x=>x.Type!="none").GroupBy(x => new { x.Note }).Select(x => new ToDoObj() { CreatedDate = x.First().CreatedDate, Note = x.Key.Note, LastTime = new TimeSpan(x.Sum(x => x.LastTime.Ticks)), Id = x.First().Id, Type = x.First().Type, Category=x.First().Type }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                        //update Category and Task
                        foreach (ToDoObj task in allTasks)
                        {
                            var findTask = SQLCommands.QueryTodo(task.Note);
                            if (findTask != null&&IdCategoryDic.ContainsKey(findTask.CategoryId))
                            {
                                task.Category =  IdCategoryDic[findTask.CategoryId];
                                task.CategoryId= findTask.CategoryId;
                            }
                            else if (categoryDic.ContainsKey(task.Type))
                            {
                                task.CategoryId = categoryDic[task.Type];
                            }else{
                                task.CategoryId = 0;
                            }
                        }
                        await Helper.RBChanged(radioButton.CommandParameter, SingleDayPlot, SQLCommands, "", allTasks);
                    }
                      
                }));
            }
        }
        private async void Enter_Click(object obj)
        {
            if(obj.ToString() == ""){
                return;
            }
            if(!hs.Contains(obj.ToString())){
                var index = 0;
                ToDoObj newObj = null;
                var task = SQLCommands.QueryTodo(obj.ToString());
                if (task != null){
                    newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = obj.ToString(), Finished = false, Type = Helper.IdCategoryDic.ContainsKey(task.TypeId)? Helper.IdCategoryDic[task.TypeId]:"none", CategoryId = task.CategoryId };
                    index = await SQLCommands.UpdateTodo(newObj);
                }
                else{
                    newObj = new ToDoObj() { CreatedDate = DateTime.Today, Note = obj.ToString(), Finished = false, Type = "none", CategoryId = categoryDic["none"] };
                    index = await SQLCommands.AddTodo(newObj);
                }
                newObj.Id = index;
                hs.Add(obj.ToString());
                TodayList.Add(newObj);
                TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
                TodayText = "";
                TodoToday.IsDropDownOpen = false;
            }
            else{
                TodayText = "";
                await showMessageBox("已存在这个任务");
            }
        }
        private async void DeleteContextMenu(object obj)
        {
            if(currentObj != null)
            {
                TodayList.Remove(currentObj);
                hs.Remove(currentObj.Note);
                await CheckAndDeleteToDo(currentObj);
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
                    GridHeight = RightPanelHeight -284;
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
                TimeSpan allTimeSpan = AllTimeViewObjs[0].DailyObjs.OrderBy(x=>x.EndTime).Last().EndTime - Helper.GlobalStartTimeSpan;
                return lastTime/allTimeSpan*(height-95);
            }
            else
            {
                return 0;
            }
        }
        private async void CheckChanged(object obj) {
            TodayList = new ObservableCollection<ToDoObj>(todayList.OrderBy(x => x.Finished));
            GeneratedToDoTask updatedObj = SQLCommands.QueryTodo(((ToDoObj)obj).Note);
            updatedObj.Finished = ((ToDoObj)obj).Finished;
            await SQLCommands.UpdateTodo(updatedObj);
        }
        
    }
    
}
