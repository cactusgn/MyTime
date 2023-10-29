using MaterialDesignColors.Recommended;
using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using ScottPlot;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Summary.Models
{
    public class QueryTaskModel:ViewModelBase
    {
		private DateTime startTime = DateTime.Today.AddDays(-6);
        private List<ToDoObj> allTasks = new List<ToDoObj>();
        public DateTime StartTime
        {
			get { return startTime; }
			set { startTime = value;OnPropertyChanged(); }
		}

		private DateTime endTime = DateTime.Today;

		public DateTime EndTime
        {
			get { return endTime; }
			set { endTime = value; OnPropertyChanged(); }
		}
        private string category;

        public string Category
        {
            get { return category; }
            set { category = value;
                AfterClickCategory(category);
            }
        }

        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set { _IsDialogOpen = value; OnPropertyChanged(); }
        }
        private TimeSpan totalCost;
        private string totalCostString;
        public string TotalCostString
        {
            get { return totalCostString; }
            set { totalCostString = value; OnPropertyChanged(); }
        }
        private TimeSpan averageCost;

        public TimeSpan AverageCost
        {
            get { return averageCost; }
            set { averageCost = value; OnPropertyChanged(); }
        }
        private int totalBonus;

        public int TotalBonus
        {
            get { return totalBonus; }
            set { totalBonus = value; OnPropertyChanged(); }
        }
        public System.Windows.Controls.MenuItem UpdateCategoryMenuItem;
        public DataGrid CategoryDatagrid;
        public MyCommand ClickOkButtonCommand { get; set; }
        public MyCommand UpdateCategoryCommand { get; set; }
        private ObservableCollection<ToDoObj> categoryDataGridSource = new ObservableCollection<ToDoObj>();
        public List<RadioButton> radioButtons { get; set; } = new List<RadioButton>();
        public ObservableCollection<ToDoObj> CategoryDataGridSource
        {
            get
            {
                return categoryDataGridSource;
            }
            set
            {
                categoryDataGridSource = value;
                OnPropertyChanged();
            }
        }
        private List<Category> categories;
        public List<Category> Categories
        {
            get { return categories; }
            set { categories = value; OnPropertyChanged(); }
        }
        public MyCommand CellEditEndingCommand { get; set; }
        public MyCommand SummaryRBChangedCommand { get; set; }
        public MyCommand UpdateViewModeCommand { get; set; }
        public ISQLCommands SQLCommands { get; set; }
        private int SelectedContextMenuCategoryId { get; set; }
        private string SelectedContextMenuType { get; set; }
        public Dictionary<string, int> TypesDic { get; set; } = new Dictionary<string, int>();
        public WrapPanel RBWrapPanel { get; internal set; }
        public WpfPlot CategoryPlot { get; internal set; }
        public Dictionary<string, string> colorDic = new Dictionary<string, string>();
        private string tableViewVisible = "Visible";

        public string TableViewVisible
        {
            get { return tableViewVisible; }
            set { tableViewVisible = value; OnPropertyChanged();}
        }
        private string squareViewVisible= "Collapsed";

        public string SquareViewVisible
        {
            get { return squareViewVisible; }
            set { squareViewVisible = value; OnPropertyChanged();}
        }
        private ObservableCollection<TimeSumView>  wrapDataSource;
        
        public ObservableCollection<TimeSumView> WrapDataSource
        {
            get { return wrapDataSource; }
            set { wrapDataSource = value; OnPropertyChanged();}
        }
        List<MyTime> SummaryAllTimeObjs = new List<MyTime>();
        private bool HasChoosenCategory = false;
        public QueryTaskModel(string category, DateTime startTime, DateTime endTime, ISQLCommands sqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            UpdateCategoryCommand = new MyCommand(UpdateCategory);
            CellEditEndingCommand = new MyCommand(CellEditEnding);
            UpdateViewModeCommand = new MyCommand(UpdateViewMode);
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
            SQLCommands = sqlCommands;
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
        }

        private void UpdateViewMode(object obj)
        {
            if(TableViewVisible=="Visible"){
                TableViewVisible = "Collapsed";
                SquareViewVisible = "Visible";
            }else{
                TableViewVisible = "Visible";
                SquareViewVisible = "Collapsed";
            }
        }

        private async void CellEditEnding(object obj)
        {
            //update note or type
            ToDoObj curr = (ToDoObj)obj;
            var updateNoteItem = CategoryDataGridSource.First(x => x.Id == curr.Id);
            updateNoteItem.Type = curr.Type;
            await SQLCommands.UpdateTodo(curr);
        }
        private void updateSubContextMenu(List<Category> Categories, System.Windows.Controls.MenuItem currentNode, int id)
        {
            var subCategories = Categories.Where(x => x.ParentCategoryId == id).ToList();
            if(subCategories.Count == 0) {
                currentNode.Click += updateCurrentSelectedCategory;
            }
            
            foreach (var category in subCategories)
            {
                if (category.Visible)
                {
                    System.Windows.Controls.MenuItem child = new System.Windows.Controls.MenuItem();
                    if (category.ParentCategoryId==0)
                    {
                        child.Tag = category.Name;
                        child.Click += updateCurrentSelectedType;
                        TypesDic.Add(category.Name, category.Id);
                    }
                    else
                    {
                        child.Tag = category.Id;
                    }
                    child.Header = category.Name;
                    child.Command = UpdateCategoryCommand;
                    child.CommandParameter = CategoryDatagrid.SelectedItems;
                    updateSubContextMenu(Categories, child,category.Id);
                    currentNode.Items.Add(child);
                }
            }
        }

        private void updateCurrentSelectedType(object sender, RoutedEventArgs e)
        {
            //each time will change type, but not every time will task change category id
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
            SelectedContextMenuType = (string)item.Tag;
            if(!HasChoosenCategory){
                if (TypesDic.ContainsKey(item.Tag.ToString())) {
                    SelectedContextMenuCategoryId = TypesDic[item.Tag.ToString()];
                }
                else
                {
                    SelectedContextMenuCategoryId = 0;
                }
                HasChoosenCategory = true;
            }
        }

        private void updateCurrentSelectedCategory(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
            HasChoosenCategory = true;
            if (TypesDic.ContainsKey(item.Tag.ToString())) {
                SelectedContextMenuCategoryId = TypesDic[item.Tag.ToString()];
            }
            else
            {
                SelectedContextMenuCategoryId = (int)item.Tag;
            }
        }

        public async void UpdateContextMenu()
        {
            Helper.allcategories = await SQLCommands.GetAllCategories();
            UpdateCategoryMenuItem.Items.Clear();
            TypesDic.Clear();
            updateSubContextMenu(Helper.allcategories, UpdateCategoryMenuItem,0);
        }
        private async void UpdateCategory(object obj)
        {
            System.Collections.IList items = (System.Collections.IList)obj;
            var collection = items.Cast<ToDoObj>();
            List<ToDoObj> selectedTasks = collection.ToList();
            Helper.allcategories = await SQLCommands.GetAllCategories();
            foreach (var task in selectedTasks) {
                task.CategoryId = SelectedContextMenuCategoryId;
                task.Category = Helper.allcategories.First(x=>x.Id ==SelectedContextMenuCategoryId).Name;
                task.Type = SelectedContextMenuType;
                await SQLCommands.UpdateTodo(task);
                List<MyTime> allTimeObjs =  SQLCommands.GetTimeObjsByName(task.Note);
                //更新所有该note的timeObj的type
                foreach (MyTime timeObj in allTimeObjs)
                {
                    timeObj.type =task.Type.ToString();
                    await SQLCommands.UpdateObj(timeObj);
                }
            }
            AfterClickCategory(Category);
        }

        public async void GetSummaryDate()
        {
            if (SQLCommands!=null)
            {
                SummaryAllTimeObjs = await SQLCommands.GetAllTimeObjs(StartTime, EndTime);
                Helper.allcategories = await SQLCommands.GetAllCategories();
                List<GeneratedToDoTask> AllTasksFromDatabase = SQLCommands.GetTasks(new DateTime(1900,1,1), EndTime);
                int findCategoryId = Helper.allcategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
                if (Category=="") {
                    findCategoryId=0;
                }
                allTasks = SummaryAllTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type!= null&&x.type!="none"&&x.note!=null).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => (x.endTime-x.startTime).Ticks)), Id = x.First().taskId, Type = x.First().type }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                foreach (ToDoObj task in allTasks)
                {
                    if (task.Id == 0||!AllTasksFromDatabase.Any(x=>x.Id==task.Id)||task.CategoryId==0)
                    {
                       await updateTaskIndex(task);
                    }
                   
                    //await updateTaskCategory(task, AllTasksFromDatabase, findCategoryId);
                    
                    if (task.Category == "" || task.Category == null)
                    {
                        task.Category = task.Type.ToString();
                    }
                }
                //check again if some time objs are created on second day while the task is created on previous day
                foreach (MyTime TimeObj in SummaryAllTimeObjs)
                {
                    if (TimeObj.taskId==0||!AllTasksFromDatabase.Any(x => x.Id==TimeObj.taskId))
                    {
                        GeneratedToDoTask findTask = SQLCommands.QueryTodo(TimeObj.note);

                        int findIndex = 0;
                        if (findTask != null)
                        {
                            findIndex = findTask.Id;
                        }
                        //找不到task，就新建一个task
                        if (findIndex==0)
                        {
                            ToDoObj taskToDo = new ToDoObj()
                            {
                                CreatedDate = TimeObj.createDate,
                                Note = TimeObj.note,
                                Type = TimeObj.type,
                                CategoryId = Helper.categoryDic.ContainsKey(TimeObj.type) ? Helper.categoryDic[TimeObj.type] : 0
                            };
                            findIndex =  await SQLCommands.AddTodo(taskToDo);
                            taskToDo.Id = findIndex;
                            List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(taskToDo.Note);
                            if (timeObjs!=null)
                            {
                                //新建完了更新所有该note的timeObj的taskId
                                foreach (MyTime timeObj in timeObjs)
                                {
                                    timeObj.taskId = findIndex;
                                    await SQLCommands.UpdateObj(timeObj);
                                }
                            }
                            AllTasksFromDatabase.Add(new GeneratedToDoTask() { Id = taskToDo.Id});
                        }
                        else
                        {
                            List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(TimeObj.note);
                            foreach (MyTime timeObj in timeObjs)
                            {
                                timeObj.taskId = findIndex;
                                timeObj.type = timeObj.type.Trim();
                                await SQLCommands.UpdateObj(timeObj);
                            }
                        }
                    }
                }
                 AfterClickCategory(Category);
            }
        }
        private void AfterClickCategory(string Category){
            if(RBWrapPanel!=null){
                int findCategoryId = Helper.allcategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
                if (RBWrapPanel != null)
                {
                    RBWrapPanel.Dispatcher.Invoke(new Action(delegate
                    {
                        RefreshRadioButtons(findCategoryId);
                    }));
                }
                var tempTasks = allTasks.ToList();
                if (findCategoryId != 0)
                {
                    tempTasks = allTasks.Where(x => x.CategoryId == findCategoryId).ToList();
                }
                List<ToDoObj> allSubTasks = new List<ToDoObj>();
                GetAllSubTask(findCategoryId, allSubTasks, allTasks);
                PaletteHelper _paletteHelper = new PaletteHelper();
                ITheme theme = _paletteHelper.GetTheme();
                var palette =  _paletteHelper.GetTheme().PrimaryMid;
                Color mainColor = Color.FromRgb((byte)Math.Max(palette.Color.R-50,0), (byte)Math.Max(palette.Color.G-50, 0), (byte)Math.Max(palette.Color.B-50, 0));
                WrapDataSource = new ObservableCollection<TimeSumView>(SummaryAllTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type!= null&&x.type!="none"&&x.note!=null&&allSubTasks.Any(t=>t.Id==x.taskId&&x.note==t.Note)).GroupBy(x=>new{x.createDate}).Select(x=>new TimeSumView(){ Date=x.Key.createDate, Hour=new TimeSpan(x.Sum(y=>(y.endTime-y.startTime).Ticks))}).OrderBy(x=>x.Date));
                DateTime createDate = startTime;
                TimeSpan maxTimeSpanInDS = new TimeSpan(9, 0, 0);
                if (WrapDataSource.Count>0)
                {
                    maxTimeSpanInDS = WrapDataSource.Max(x => x.Hour);
                }
                while(createDate<=endTime){
                    if(!WrapDataSource.Any(x=>x.Date==createDate)){
                         RBWrapPanel.Dispatcher.Invoke(new Action(delegate
                        {
                            WrapDataSource.Add(new TimeSumView(){ Date=createDate,Hour=new TimeSpan(0), Color=Color.FromArgb(1,mainColor.R,mainColor.G, mainColor.B).ToString()});
                        }));
                    }else{
                        RBWrapPanel.Dispatcher.Invoke(new Action(delegate
                        {
                            var item = WrapDataSource.First(x=>x.Date==createDate);
                            item.Color = Color.FromArgb((byte)(item.Hour.TotalSeconds/maxTimeSpanInDS.TotalSeconds*255),mainColor.R,mainColor.G, mainColor.B).ToString();
                        }));
                    }
                    createDate = createDate.AddDays(1);
                }
                RBWrapPanel.Dispatcher.Invoke(new Action(delegate
                {
                    WrapDataSource = new ObservableCollection<TimeSumView>(WrapDataSource.OrderBy(x=>x.Date));
                }));
                CalculateBonus(allSubTasks);
                CategoryDataGridSource = new ObservableCollection<ToDoObj>(tempTasks);
                totalCost = new TimeSpan(allSubTasks.Sum(x => x.LastTime.Ticks));
                TotalCostString = $"{totalCost.TotalHours.ToString("0.00")}h";
                AverageCost = (totalCost / ((EndTime - StartTime).Days+1));
                AverageCost = TimeSpan.FromMinutes((int)AverageCost.TotalMinutes);
                if (radioButtons.Count>0)
                {
                    RBWrapPanel.Dispatcher.Invoke(new Action(delegate
                    {
                        radioButtons[0].IsChecked = true;
                    }));
                    SummaryRBChanged(1);
                }
            }
        }

        private void GetAllSubTask(int findCategoryId, List<ToDoObj> allSubTasks, List<ToDoObj> allTasks)
        {
            allSubTasks.AddRange(allTasks.Where(x => x.CategoryId == findCategoryId).ToList());
            var subCategories = Helper.allcategories.Where(x => x.ParentCategoryId == findCategoryId);
            if (subCategories.Count() > 0)
            {
                foreach(Category cate in subCategories)
                {
                    GetAllSubTask(cate.Id, allSubTasks, allTasks);
                }
            }
        }

        private void RefreshRadioButtons(int findCategoryId)
        {
            if(RBWrapPanel!=null){
                int maxDepth = Helper.getMaxDepth(1,findCategoryId);
                radioButtons.Clear();
                RBWrapPanel.Children.Clear();
                for (int i = 0; i < maxDepth; i++)
                {
                    RadioButton AllRadioButton = new RadioButton();
                    Binding BindingObj = new Binding();
                    BindingObj.Path = new PropertyPath("RadioButtonEnabled");
                    AllRadioButton.SetBinding(RadioButton.IsEnabledProperty, BindingObj);
                    AllRadioButton.FontSize = 14;
                    AllRadioButton.Name = "RB" + i.ToString();
                    AllRadioButton.GroupName = "QueryType";
                    AllRadioButton.Margin = new Thickness(5, 5, 5, 5);
                    AllRadioButton.Command = SummaryRBChangedCommand;
                    AllRadioButton.CommandParameter = (i+1).ToString();
                    AllRadioButton.Content = "层级" + (i + 1).ToString();
                    //if (i == 0) AllRadioButton.IsChecked = true;
                    RBWrapPanel.Children.Add(AllRadioButton);
                    radioButtons.Add(AllRadioButton);
                }
            }
        }
        
        private async void SummaryRBChanged(object obj)
        {
           await Helper.RBChanged(obj, CategoryPlot, SQLCommands, category, allTasks);
        }
      
        private void CalculateBonus(List<ToDoObj> allTasks)
        {
            TotalBonus = 0;
            foreach (var task in allTasks)
            {
                int bonus = 0;
                var categoryItem = Helper.allcategories.FirstOrDefault(x => x.Name == task.Category);
                if (categoryItem != null)
                {
                    bonus = categoryItem.BonusPerHour;
                }
                if (bonus==0)
                {
                    categoryItem = Helper.allcategories.FirstOrDefault(x => x.Name == task.Type.ToString());
                    if (categoryItem!= null)
                    {
                        bonus = categoryItem.BonusPerHour;
                    }
                }
                task.Bonus = (int)(task.LastTime.TotalSeconds* bonus/3600);
                TotalBonus = TotalBonus + task.Bonus;
            }
        }
        private async Task updateTaskCategory(ToDoObj task, List<GeneratedToDoTask> AllTasks, int findCategoryId)
        {
                var findTask = AllTasks.Where(x => x.Note == task.Note).FirstOrDefault();
                if (findTask != null)
                {
                    task.CategoryId = findTask.CategoryId;
                    if (task.CategoryId == 0)
                    {
                        task.CategoryId = findCategoryId;
                        task.Category = Category;
                        await SQLCommands.UpdateTodo(task);
                    }
                    else
                    {
                        var findCategory = Helper.allcategories.FirstOrDefault(x => x.Id == findTask.CategoryId);
                        if (findCategory != null)
                        {
                            task.Category = findCategory.Name;
                        }
                    }
                }
                else
                {
                    findCategoryId = Helper.allcategories.FirstOrDefault(x => x.Name==task.Type.ToString(), new Data.Category()).Id;
                    task.CategoryId = findCategoryId;
                    task.Category = task.Type.ToString();
                    await SQLCommands.UpdateTodo(task);
                }
        }
        private async Task updateTaskIndex(ToDoObj task)
        {

            //找到当前note对应的taskID
            GeneratedToDoTask findTask = SQLCommands.QueryTodo(task.Note);
            
            int findIndex =0;
            int findTaskCategoryId = 0;
            if (findTask != null)
            {
                findIndex = findTask.Id;
                findTaskCategoryId = findTask.CategoryId;
                task.Finished = findTask.Finished;
            }
            //找不到task，就新建一个task
            if (findIndex==0)
            {
                findIndex =  await SQLCommands.AddTodo(task);
                task.Id = findIndex;
                List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(task.Note);
                if (timeObjs!=null)
                {
                    //新建完了更新所有该note的timeObj的taskId
                    foreach (MyTime timeObj in timeObjs)
                    {
                        timeObj.type = timeObj.type==null? task.Type.ToString(): timeObj.type;
                        timeObj.taskId = findIndex;
                        await SQLCommands.UpdateObj(timeObj);
                    }
                }
            }
            else
            {
                task.Id = findIndex;
                task.CategoryId = findTaskCategoryId;
                List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(task.Note);
                foreach (MyTime timeObj in timeObjs)
                {
                    timeObj.taskId = findIndex;
                    timeObj.type = timeObj.type.Trim();
                    await SQLCommands.UpdateObj(timeObj);
                }
            } 
            //task的categoryId还是0，就改成type的ID
            if (task.CategoryId == 0)
            {
                int categoryId = Helper.allcategories.FirstOrDefault(x => x.Name == task.Type.ToString(), new Data.Category()).Id;
                task.CategoryId = categoryId;
                task.Category = task.Type.ToString();
                await SQLCommands.UpdateTodo(task);
            }
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
            if (a != null && a.ToString() == "ThisWeek")
            {
                DayOfWeek dayOfWeek = DateTime.Today.DayOfWeek;
                if (dayOfWeek != DayOfWeek.Sunday)
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
            await Task.Run(() => { openDialog(); }).ContinueWith(delegate { GetSummaryDate(); closeDialog(); });
        }
        private void closeDialog()
        {
            IsDialogOpen=false;

        }
        private void openDialog()
        {
            IsDialogOpen=true;
        }

        internal async void RestoreDeleteCategoryToParentCategory(int id)
        {
            Category curr = Helper.allcategories.Where(x=>x.Id == id).FirstOrDefault();
           
            List<GeneratedToDoTask> AllTasksFromDatabase = SQLCommands.GetTasks(new DateTime(1900, 1, 1), DateTime.Today);
            var AllTasksOfDeleteCategory = AllTasksFromDatabase.Where(x => x.CategoryId == id);
            foreach(var task in AllTasksOfDeleteCategory) {
                task.CategoryId = curr.ParentCategoryId;
                await SQLCommands.UpdateTodo(task);
            }
        }
    }

    public class TimeSumView{
        public TimeSpan Hour { get; set; }
        public string Color { get; set; }
        public DateTime Date{ get;set;}

        public string DateString
        {
            get { return Date.ToShortDateString(); }
        }

    }
}
 