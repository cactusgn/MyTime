using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Renderable;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

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
        public double height;
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
        public List<RadioButton> radioButtons { get; set; } = new List<RadioButton>();

        public RadioButton ThirdLevelRB { get; set; }
        public RadioButton FirstLevelRB { get; set; }
        public SampleDialogViewModel sampleDialogViewModel { get; set; }
        public MyCommand TextBoxLostFocusCommand { get; set; }
        public MyCommand MergeCommand { get; set; }
        public List<RadioButton> SingleDayRadioButtons = new List<RadioButton>();
        
        public bool RadioButtonEnabled
        {
            get
            {
                return AllTimeViewObjs != null && AllTimeViewObjs.Count() > 0;
            }
        }
        private DateTime CurrentDate { get; set; } = DateTime.Today.AddDays(1);

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
        public int SelectedIndex = 0;
        public SummaryModel(ISQLCommands SqlCommands, SampleDialogViewModel SVM)
        {
            height = LeftPanelHeight;
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
            SingleDayRBChangedCommand = new MyCommand(SingleDayRBChanged);
            TimeObjType_SelectionChangedCommand = new MyCommand(TimeObjType_SelectionChanged);
            TimeObjType_NoteChangedCommand = new MyCommand(TimeObjType_NoteChanged);
            SplitButtonClickCommand = new MyCommand(SplitButtonClick);
            TextBoxLostFocusCommand = new MyCommand(TextBoxLostFocus);
            MergeCommand = new MyCommand(Merge);
            EndTime = DateTime.Today;
            StartTime = DateTime.Today.AddDays(-6);
            SQLCommands = SqlCommands;
            sampleDialogViewModel = SVM;
            SelectedCommand = new MyCommand(Selected);
            ResizeCommand = new MyCommand(resizeHeight);
            Helper.initColor(SqlCommands);
            updateOldItems();
        }
        public void RefreshSingleDayRadioButtons()
        {
            if (SingleDayTypeRadioGroupPanel!=null)
            {
                SingleDayTypeRadioGroupPanel.Children.Clear();
                Label label = new Label();
                label.Margin = new Thickness(10, 0, 10, 0);
                label.Content = "Type:";
                label.FontSize=14;
                SingleDayTypeRadioGroupPanel.Children.Add(label);
                int maxDepth = Helper.getMaxDepth(1, 0);
                SingleDayRadioButtons.Clear();
                for (int i = 0; i < maxDepth; i++)
                {
                    RadioButton AllRadioButton = new RadioButton();
                    Binding BindingObj = new Binding();
                    BindingObj.Path = new PropertyPath("SelectedTimeObj.IsEnabled");
                    AllRadioButton.SetBinding(RadioButton.IsEnabledProperty, BindingObj);
                    AllRadioButton.FontSize = 14;
                    AllRadioButton.Name = "RB" + i.ToString();
                    AllRadioButton.GroupName = "SingleDayType";
                    AllRadioButton.Margin = new Thickness(5, 5, 5, 5);
                    AllRadioButton.Command = SingleDayRBChangedCommand;
                    AllRadioButton.CommandParameter = (i+1).ToString();
                    AllRadioButton.Content = "层级" + (i + 1).ToString();
                    //if (i == 0) AllRadioButton.IsChecked = true;
                    SingleDayTypeRadioGroupPanel.Children.Add(AllRadioButton);
                    SingleDayRadioButtons.Add(AllRadioButton);
                }
                if (SingleDayRadioButtons.Count()>0)
                {
                    SingleDayRadioButtons[0].IsChecked = true;
                }
            }
        }
        public void initTypeCombobox()
        {
            TypeComboBox.Items.Clear();
            Helper.SummaryCategoryDic.Clear();
            TypeRadioGroupPanel.Children.Clear();
            radioButtons.Clear();
            int index = 0;
             List<Category> mainCategoriesWithNone = new List<Category>();
            mainCategoriesWithNone.Add(new Category(){ Name="none", Color="#F3F3F3"});
            mainCategoriesWithNone.AddRange(Helper.mainCategories);

            Label label = new Label();
            label.Margin = new Thickness(10,0,10,0);
            label.Content = "Type:";
            label.FontSize=14;
            TypeRadioGroupPanel.Children.Add(label);

            RadioButton AllRadioButton = new RadioButton();
            Binding BindingObj = new Binding();
            BindingObj.Path = new PropertyPath("RadioButtonEnabled");
            AllRadioButton.SetBinding(RadioButton.IsEnabledProperty, BindingObj);
            AllRadioButton.FontSize= 14;
            AllRadioButton.Name = "AllRB";
            AllRadioButton.GroupName = "Type";
            AllRadioButton.Margin = new Thickness(5, 0, 5, 0);
            AllRadioButton.Command=SummaryRBChangedCommand;
            AllRadioButton.CommandParameter = "All";
            AllRadioButton.Content = "All";
            AllRadioButton.IsChecked = true;
            TypeRadioGroupPanel.Children.Add(AllRadioButton);
            radioButtons.Add(AllRadioButton);
            foreach (var category in mainCategoriesWithNone)
            {
                Helper.SummaryCategoryDic.Add(category.Name, index++);
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                stackPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

                TextBlock textBlock = new TextBlock();
                textBlock.Width = 10;
                textBlock.Height = 10;
                BrushConverter brushConverter = new BrushConverter();
                textBlock.Background = (System.Windows.Media.Brush)brushConverter.ConvertFromString(category.Color);
                textBlock.Margin = new Thickness(5, 0, 10, 0);
                stackPanel.Children.Add(textBlock);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                textBlock2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                textBlock2.TextAlignment = System.Windows.TextAlignment.Center; 
                textBlock2.Text = category.Name;

                stackPanel.Children.Add(textBlock2);

                ComboBoxItem item = new ComboBoxItem();
                
                item.Content = stackPanel;
                item.Tag = category.Name;
                item.PreviewMouseLeftButtonUp += Type_PreviewMouseLeftButtonUp;

                TypeComboBox.Items.Add(item);

                if (category.Name=="none") continue;
                RadioButton radioButton = new RadioButton();
                BindingObj = new Binding();
                BindingObj.Path = new PropertyPath("RadioButtonEnabled");
                radioButton.SetBinding(RadioButton.IsEnabledProperty, BindingObj);
                radioButton.FontSize= 14;
                radioButton.Name = "sum" +category.Name+"RB";
                radioButton.GroupName = "Type";
                radioButton.Margin = new Thickness(5, 0, 5, 0);
                radioButton.Command=SummaryRBChangedCommand;
                radioButton.CommandParameter = category.Name;
                radioButton.Content = category.Name;
                TypeRadioGroupPanel.Children.Add(radioButton);
                radioButtons.Add(radioButton);
            }
        }

        private async void Type_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string a = ((ComboBoxItem)sender).Tag.ToString();

            if (SelectedTimeObj.Type!=a)
            {
                GeneratedToDoTask findTask = SQLCommands.QueryTodo(SelectedTimeObj.Note);
                var id = 0;
                if (findTask != null)
                {
                    id = findTask.Id;
                    findTask.TypeId = Helper.NameIdDic[a.ToString()];
                    findTask.CategoryId = 0;
                    await SQLCommands.UpdateTodo(findTask);
                }
                else
                {
                    ToDoObj newObj = new ToDoObj() { CreatedDate = SelectedTimeObj.CreatedDate, Note = SelectedTimeObj.Note, Finished = true, Type = a, CategoryId = Helper.categoryDic[a] };
                    id = await SQLCommands.AddTodo(newObj);
                }
                foreach (var dailyViewObjs in AllTimeViewObjs)
                {
                    var TodayAllObjectWithSameNote = dailyViewObjs.DailyObjs.Where(x => x.Note==selectedTimeObj.Note);
                    foreach (var obj in TodayAllObjectWithSameNote)
                    {
                        obj.Type = a.ToString();
                        obj.TaskId = id;
                        Helper.UpdateColor(obj, a.ToString());
                        await SQLCommands.UpdateObj(obj);
                    }
                }
                refreshSingleDayPlot();
                refreshSummaryPlot(currentSummaryRBType);
            }
        }

        private async void Merge(object obj)
        {
            var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs;
            if (obj.ToString() == "up")
            {
                var aboveItemList = currentDailyObj.Where(x => x.EndTime == selectedTimeObj.StartTime);
                if (aboveItemList.Count() > 0)
                {
                    var aboveItem = aboveItemList.First();
                    YESNOWindow dialog = new YESNOWindow("提示", $"确定合并时间块 {selectedTimeObj.Note} 和 {aboveItem.Note} 为 {selectedTimeObj.Note} 吗", "确定", "取消");
                    if (dialog.ShowDialog() == true)
                    {
                        var updatedStartTime = aboveItem.StartTime;
                        SelectedTimeObj.StartTime = updatedStartTime;
                        SelectedTimeObj.LastTime = SelectedTimeObj.EndTime - SelectedTimeObj.StartTime;
                        SelectedTimeObj.Height = Helper.CalculateHeight(SelectedTimeObj.LastTime, height);
                        await SQLCommands.DeleteObj(aboveItem);
                        await SQLCommands.UpdateObj(SelectedTimeObj);
                        currentDailyObj.Remove(aboveItem);
                    }
                }
            }
            else
            {
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
                        SelectedTimeObj.Height = Helper.CalculateHeight(SelectedTimeObj.LastTime, height);
                        await SQLCommands.DeleteObj(downItem);
                        await SQLCommands.UpdateObj(SelectedTimeObj);
                        currentDailyObj.Remove(downItem);
                    }
                }
            }
            AllTimeViewObjs.Single(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
            refreshSingleDayPlot();
            refreshSummaryPlot();
        }
        private async void updateOldItems()
        {
            List<GeneratedToDoTask> allTasks = SQLCommands.GetTasks(new DateTime(1900,1,1), DateTime.Today);
            //foreach (GeneratedToDoTask task in allTasks) {
            //    if(task.TypeId==0){
            //        task.TypeId = getTaskId(task.CategoryId);
            //        await SQLCommands.UpdateTodo(task);
            //    }
            //}
            List<MyTime> AllTimeObjs = await SQLCommands.GetAllTimeObjs(new DateTime(1900,1,1),DateTime.Today);
            if (AllTimeObjs != null)
            {
                foreach (MyTime timeObj in AllTimeObjs)
                {
                    if (timeObj.type==null||timeObj.type.Trim()==""|| !Helper.categoryDic.ContainsKey(timeObj.type.Trim()))
                    {
                        if(timeObj.taskId!=0&& SQLCommands.QueryTodo(timeObj.taskId)!=null)
                        {
                            timeObj.type = Helper.mainCategories.FirstOrDefault(x => x.Id == SQLCommands.QueryTodo(timeObj.taskId).TypeId, new Category(){ Name="none"}).Name;
                        }else{
                            timeObj.type = "none";
                            timeObj.taskId = 0;
                        }
                        await SQLCommands.UpdateObj(timeObj);
                    }
                }
            }
        }

     

        private async void TextBoxLostFocus(object obj)
        {
            var updateTimeViewObj = (TimeViewObj)obj;
            await SQLCommands.UpdateObj(updateTimeViewObj);
            updateTaskColor(updateTimeViewObj);
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
       
        public void refreshSingleDayPlot()
        {
            var TodayDailyObj = allTimeViewObjs.First(x => x.createdDate == SelectedTimeObj.CreatedDate).DailyObjs;

           foreach (RadioButton radioButton in SingleDayRadioButtons)
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
                            if (findTask != null&&findTask.CategoryId!=0&&Helper.IdCategoryDic.ContainsKey(findTask.CategoryId))
                            {
                                task.Category =  Helper.IdCategoryDic[findTask.CategoryId];
                                task.CategoryId= findTask.CategoryId;
                            }
                            else if(Helper.categoryDic.ContainsKey(task.Type))
                            {
                                task.CategoryId = Helper.categoryDic[task.Type];
                            }else{
                                task.CategoryId = 0;
                            }
                        }
                        await Helper.RBChanged(radioButton.CommandParameter, SingleDayPlot, SQLCommands, "", allTasks);
                    }
                      
                }));
            }
        }
        public void refreshSummaryPlot(string type="All")
        {
            var AllObjs = new ObservableCollection<TimeViewObj>();
            currentSummaryRBType = type;
            foreach (var GridTemplate in allTimeViewObjs)
            {
                var dailyObjs = GridTemplate.DailyObjs;
                if (type != "All"&&dailyObjs!=null)
                {
                    var filteredObjs = dailyObjs.Where(x => (x.Type!=null && x.Type==type)).OrderBy(e => e.LastTime);
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
                    AllObjs= new ObservableCollection<TimeViewObj>(AllObjs.GroupBy(x => x.Type).Select(x => new TimeViewObj() { LastTime = new TimeSpan(x.Sum(x => x.LastTime.Ticks)), Type = x.Key, CreatedDate = DateTime.Today, Note = x.Key }));
                }
            }
            SummaryPlot.Dispatcher.Invoke(new Action(delegate
            {
                Helper.refreshPlot(AllObjs, SummaryPlot);
            }));
        }
        
        public MyCommand SelectedCommand { get; set; }
        public ComboBox TypeComboBox { get; internal set; }
        public System.Windows.Style ComboBoxItemStyle { get; internal set; }
        public WrapPanel TypeRadioGroupPanel { get; internal set; }
        public WrapPanel SingleDayTypeRadioGroupPanel { get; internal set; }
        public Canvas LeftSchedule { get; internal set; }
        public Canvas RightSchedule { get; internal set; }

        private  void closeDialog()
        {
            IsDialogOpen=false;

        }
        private  void openDialog()
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
        public async void clickOkButton(object a = null)
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
           
            //Thread[] threads = new Thread[2];  
            //threads[0] = new Thread(openDialog);
            //threads[0].Start();
            //threads[1] = new Thread(test);
            //threads[1].Start();
           await Task.Run( () => {  openDialog(); })
                            .ContinueWith(async delegate { 
                                await Task.Delay(500); 
                                showTimeView(); 
                                closeDialog(); 
                             });
        }
        private async void test(){
            //延迟500ms，让它可以完整地先显示出进度条，否则进度条显示动画也会卡住, 此多线程效果和上面的task.run的效果相同
            //await Task.Delay(500);  
            //showTimeView(); 
            //closeDialog();
        }
        private async void TimeObjType_SelectionChanged(object a)
        {
            if (SelectedTimeObj.Type!=a.ToString())
            {
                GeneratedToDoTask findTask = SQLCommands.QueryTodo(SelectedTimeObj.Note);
                if (findTask != null)
                {
                    findTask.TypeId = Helper.NameIdDic[a.ToString()];
                    findTask.CategoryId = 0;
                    await SQLCommands.UpdateTodo(findTask);
                }
                foreach(var dailyViewObjs in AllTimeViewObjs)
                {
                    var TodayAllObjectWithSameNote = dailyViewObjs.DailyObjs.Where(x => x.Note==selectedTimeObj.Note);
                    foreach (var obj in TodayAllObjectWithSameNote)
                    {
                        obj.Type = a.ToString();
                        Helper.UpdateColor(obj, a.ToString());
                        //await SQLCommands.UpdateObj(obj);
                    }
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
            updateTaskColor(currObj);
            await SQLCommands.UpdateObj(SelectedTimeObj);
            refreshSingleDayPlot();
        }
        private void updateTaskColor(TimeViewObj currObj)
        {
            GeneratedToDoTask findTask = SQLCommands.QueryTodo(currObj.Note);
            int typeId = findTask != null ? findTask.TypeId : 0;
            string type = Helper.IdCategoryDic.ContainsKey(typeId) ? Helper.IdCategoryDic[typeId] : "none";
            if(findTask!=null){
                currObj.TaskId = findTask.Id;
            }
            Helper.UpdateColor(currObj, type.ToString());
        }
        public async void showTimeView()
        {
            AllTimeViewObjs = await Helper.BuildTimeViewObj(startTime, endTime,SQLCommands,height);
            SelectedTimeObj = new TimeViewObj() { Type="" };
            foreach(RadioButton radioButton in radioButtons) { 
                radioButton.Dispatcher.Invoke(new Action(delegate
                {
                    if (radioButton.IsChecked==true) refreshSummaryPlot(radioButton.Content.ToString());
                }));
            }
            updateCanvas();
        }

        public void updateCanvas(){
            if(LeftSchedule!=null && LeftPanelHeight > 100){
             LeftSchedule.Dispatcher.Invoke(new Action(delegate
                {
                    LeftSchedule.Children.Clear();
                    RightSchedule.Children.Clear();

                    System.Windows.Shapes.Rectangle r = new System.Windows.Shapes.Rectangle();
                    r.Fill = new SolidColorBrush(Colors.LightGray);
                    r.Stroke = new SolidColorBrush(Colors.LightGray);
                    r.Width = 2;
                    r.Height = height-100;
                    r.SetValue(Canvas.LeftProperty, (double)65);
                    r.SetValue(Canvas.TopProperty, (double)75);
                   
                    LeftSchedule.Children.Add(r);

                    System.Windows.Shapes.Rectangle r1 = new System.Windows.Shapes.Rectangle();
                    r1.Fill = new SolidColorBrush(Colors.LightGray);
                    r1.Stroke = new SolidColorBrush(Colors.LightGray);
                    r1.Width = 2;
                    r1.Height = height-100;
                    r1.SetValue(Canvas.LeftProperty, (double)7);
                    r1.SetValue(Canvas.TopProperty, (double)75);
                   
                    RightSchedule.Children.Add(r1);

                    int allhours = Helper.GlobalEndTimeSpan.Hours - Helper.GlobalStartTimeSpan.Hours;
                    PaletteHelper _paletteHelper = new PaletteHelper();
                    ITheme theme = _paletteHelper.GetTheme();
                    //bool IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;
                    var paletteColor =  _paletteHelper.GetTheme().PrimaryMid;
                    for(int i = Helper.GlobalStartTimeSpan.Hours+1;i<=Helper.GlobalEndTimeSpan.Hours;i++){
                        if(i<=Helper.GlobalEndTimeSpan.Hours){
                            Ellipse el = new Ellipse();
                            el.Fill = new SolidColorBrush(paletteColor.Color);
                            el.Stroke = new SolidColorBrush(paletteColor.Color);
                            el.Width = 10;
                            el.Height = 10;
                            el.SetValue(Canvas.ZIndexProperty, 1);
                            el.SetValue(Canvas.LeftProperty, (double)61);
                            el.SetValue(Canvas.TopProperty, (double)((new TimeSpan(i,0,0)) - Helper.GlobalStartTimeSpan).TotalSeconds/(Helper.GlobalEndTimeSpan - Helper.GlobalStartTimeSpan).TotalSeconds*r.Height+75);
                            LeftSchedule.Children.Add(el);

                            Ellipse el2 = new Ellipse();
                            el2.Fill = new SolidColorBrush(paletteColor.Color);
                            el2.Stroke = new SolidColorBrush(paletteColor.Color);
                            el2.Width = 10;
                            el2.Height = 10;
                            el2.SetValue(Canvas.ZIndexProperty, 1);
                            el2.SetValue(Canvas.LeftProperty, (double)3);
                            el2.SetValue(Canvas.TopProperty, (double)((new TimeSpan(i,0,0)) - Helper.GlobalStartTimeSpan).TotalSeconds/(Helper.GlobalEndTimeSpan - Helper.GlobalStartTimeSpan).TotalSeconds*r.Height+75);
                            RightSchedule.Children.Add(el2);

                            TextBlock a = new TextBlock();
                            a.Text =  i.ToString("00")+":00:00";
                            a.FontSize = 12;
                            a.SetValue(Canvas.LeftProperty, (double)7);
                            a.SetValue(Canvas.TopProperty, (double)((new TimeSpan(i,0,0)) - Helper.GlobalStartTimeSpan).TotalSeconds/(Helper.GlobalEndTimeSpan - Helper.GlobalStartTimeSpan).TotalSeconds*r.Height+72);
                            LeftSchedule.Children.Add(a);

                            TextBlock a2 = new TextBlock();
                            a2.Text =  i.ToString("00")+":00:00";
                            a2.FontSize = 12;
                            a2.SetValue(Canvas.LeftProperty, (double)15);
                            a2.SetValue(Canvas.TopProperty, (double)((new TimeSpan(i,0,0)) - Helper.GlobalStartTimeSpan).TotalSeconds/(Helper.GlobalEndTimeSpan - Helper.GlobalStartTimeSpan).TotalSeconds*r.Height+72);
                            RightSchedule.Children.Add(a2);
                        }
                    }
                }));
               
            }
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
                SummaryPlot.Height = LeftPanelHeight-250>0?LeftPanelHeight-250:100;
                SingleDayPlot.Height = LeftPanelHeight-250>0?LeftPanelHeight-250:100;
                updateCanvas();
                SummaryPlot.Refresh();
                SingleDayPlot.Refresh();
            }
        }
        
        public async void SplitTimeBlock(TimeSpan SplitTime, string content1, string content2){
            if(selectedTimeObj!=null){
                var currentDailyObj = AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs;
                var lastIndex = currentDailyObj.Max(x=>x.Id) +1;
                string type = "none";
                GeneratedToDoTask findTask = SQLCommands.QueryTodo(content1);
                int taskId = findTask==null ? 0 : findTask.Id;
               
                if (taskId==0)
                {
                    ToDoObj newObj = new ToDoObj() { CreatedDate = SelectedTimeObj.CreatedDate, Note = content1, Finished = false, Type = type, CategoryId = 0 };
                    taskId = await SQLCommands.AddTodo(newObj);
                }
                else
                {
                    if (Helper.IdCategoryDic.ContainsKey(findTask.TypeId))
                        type = Helper.IdCategoryDic[findTask.TypeId];
                }
                var newTimeObj1 = Helper.CreateNewTimeObj(selectedTimeObj.StartTime, SplitTime, content1, selectedTimeObj.CreatedDate, type, lastIndex, height, taskId: taskId);
                Helper.UpdateColor(newTimeObj1, type);
                lastIndex++;
                taskId = 0;
                type = "none";
                if (content2!="")
                {
                    findTask = SQLCommands.QueryTodo(content2);
                    taskId =  findTask==null ? 0 : findTask.Id;
                    if (taskId==0)
                    {
                        ToDoObj newObj = new ToDoObj() { CreatedDate = SelectedTimeObj.CreatedDate, Note = content2, Finished = false, Type = type, CategoryId = 0 };
                        taskId = await SQLCommands.AddTodo(newObj);
                    }
                    else
                    {
                        if (Helper.IdCategoryDic.ContainsKey(findTask.TypeId))
                            type = Helper.IdCategoryDic[findTask.TypeId];
                    }
                }
                var newTimeObj2 = Helper.CreateNewTimeObj(SplitTime, selectedTimeObj.EndTime, content2, selectedTimeObj.CreatedDate, type, lastIndex,height, taskId: taskId);
                Helper.UpdateColor(newTimeObj2, type);
                await SQLCommands.DeleteObj(selectedTimeObj);
                await SQLCommands.AddObj(newTimeObj1);
                await SQLCommands.AddObj(newTimeObj2);
                
                currentDailyObj.Add(newTimeObj1);
                currentDailyObj.Add(newTimeObj2);
                currentDailyObj.Remove(selectedTimeObj);
                AllTimeViewObjs.Single(x => x.createdDate == selectedTimeObj.CreatedDate).DailyObjs = new ObservableCollection<TimeViewObj>(currentDailyObj.OrderBy(item => item.StartTime));
                SelectedTimeObj = newTimeObj1;
                TypeComboBox.SelectedItem = type;
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
