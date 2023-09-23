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
        public ISQLCommands SQLCommands { get; set; }
        private int SelectedContextMenuCategoryId { get; set; }
        private string SelectedContextMenuType { get; set; }
        public Dictionary<string, int> TypesDic { get; set; } = new Dictionary<string, int>();
        public WrapPanel RBWrapPanel { get; internal set; }
        public WpfPlot CategoryPlot { get; internal set; }
        public Dictionary<string, string> colorDic = new Dictionary<string, string>();
        public bool displayInvisibleItems { get; set; } = false;
        List<Category> AllCategories = new List<Category>();
        Dictionary<string, int> NameIdDic = new Dictionary<string, int>();
        Dictionary<int, string> IdNameDic = new Dictionary<int, string>();
        public QueryTaskModel(string category, DateTime startTime, DateTime endTime, ISQLCommands sqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            UpdateCategoryCommand = new MyCommand(UpdateCategory);
            CellEditEndingCommand = new MyCommand(CellEditEnding);
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
            SQLCommands = sqlCommands;
            SummaryRBChangedCommand = new MyCommand(SummaryRBChanged);
           
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
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
            SelectedContextMenuType = (string)item.Tag;
        }

        private void updateCurrentSelectedCategory(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem item = (System.Windows.Controls.MenuItem)sender;
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
            }
            AfterClickCategory(Category);
        }

        public async void GetSummaryDate()
        {
            if (SQLCommands!=null)
            {
                List<MyTime> allTimeObjs = await SQLCommands.GetAllTimeObjs(StartTime, EndTime);
                List<Category> AllCategories = await SQLCommands.GetAllCategories();
                List<GeneratedToDoTask> AllTasksFromDatabase = SQLCommands.GetTasks(new DateTime(1900,1,1), EndTime);
                int findCategoryId = AllCategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
                if (Category=="") findCategoryId=0;
                allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type!= null&&x.type!="none"&&x.note!=null).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = x.First().type }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                foreach (ToDoObj task in allTasks)
                {
                    //taskid为0，或者type不等于当前category
                    if (task.Id == 0||AllTasksFromDatabase.Where(x => x.Note==task.Note&& AllCategories.FirstOrDefault(y => y.Id==x.TypeId, new Data.Category() { Name="" }).Name== Category).Count()==0)
                    {
                       await updateTaskIndex(task, AllCategories);
                    }
                   
                    await updateTaskCategory(task, AllTasksFromDatabase, findCategoryId, AllCategories);
                    
                    if (task.Category == "" || task.Category == null)
                    {
                        task.Category = task.Type.ToString();
                    }
                }
                 AfterClickCategory(Category);
            }
        }
        private void AfterClickCategory(string Category){
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
            CalculateBonus(allSubTasks, Helper.allcategories);
            CategoryDataGridSource = new ObservableCollection<ToDoObj>(tempTasks);
            totalCost = new TimeSpan(allSubTasks.Sum(x => x.LastTime.Ticks));
            TotalCostString = $"{totalCost.TotalHours.ToString("0.00")}h";
            AverageCost = (totalCost / (EndTime - StartTime).Days);
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

        private void GetAllSubTask(int findCategoryId, List<ToDoObj> allSubTasks, List<ToDoObj> allTasks)
        {
            allSubTasks.AddRange(allTasks.Where(x => x.CategoryId == findCategoryId).ToList());
            var subCategories = AllCategories.Where(x => x.ParentCategoryId == findCategoryId);
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
                int maxDepth = getMaxDepth(1,findCategoryId);
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
        private long GetAllSubTime(List<ToDoObj> plotData, string currcategory)
        {
            if(!NameIdDic.ContainsKey(currcategory)) return 0;
            var tempSubCategories = AllCategories.Where(x => x.ParentCategoryId == NameIdDic[currcategory] ).ToList();
            long res = 0;
            if(tempSubCategories.Count > 0) { 
                foreach(var category in tempSubCategories) {
                    res+=GetAllSubTime(plotData, category.Name);
                }
            }else{
                return plotData.Where(x => x.CategoryId == NameIdDic[currcategory]).Sum(x => x.LastTime.Ticks);
            }
            res += plotData.Where(x => x.CategoryId == NameIdDic[currcategory]).Sum(x=>x.LastTime.Ticks);
            return res;
        }
        private async void SummaryRBChanged(object obj)
        {
            int param = int.Parse(obj.ToString());
            var index = 0;
            CategoryPlot.Plot.Clear();
            var plt = CategoryPlot.Plot;
            Dictionary<string, int> typelevelDic = new Dictionary<string, int>();
            colorDic.Clear();
            AllCategories = await SQLCommands.GetAllCategories();
            int findCategoryId = Helper.allcategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
            NameIdDic.Clear();
            IdNameDic.Clear();
            IdNameDic.Add(0, "none");
            NameIdDic.Add("none", 0);
            colorDic.Add("none", "#F3F3F3");
            foreach (Category category in AllCategories)
            {
                if(!category.Visible) { continue; }
                NameIdDic.Add(category.Name, category.Id);
                IdNameDic.Add(category.Id, category.Name);
                colorDic.Add(category.Name, category.Color);
                Category tempCate = category;
                int level = 0;
                if (tempCate.Id == findCategoryId) {
                    typelevelDic.Add(category.Name, level);
                }
                while (AllCategories.FirstOrDefault(x=>x.Id == tempCate.Id, new Data.Category() { Id= findCategoryId }).Id!=findCategoryId|| AllCategories.FirstOrDefault(x => x.Id == tempCate.Id, new Data.Category() { Id = 0 }).Id != 0)
                {
                    level++;
                    tempCate = AllCategories.FirstOrDefault(x => x.Id == tempCate.ParentCategoryId, new Data.Category() { ParentCategoryId= findCategoryId });
                    if(tempCate.Id==findCategoryId) 
                    typelevelDic.Add(category.Name, level);
                }
            }
            List<ToDoObj> plotData = allTasks.Where(x => typelevelDic.ContainsKey(x.Category)).ToList();
            int maxDepth = getMaxDepth(1, findCategoryId);
            foreach (ToDoObj task in plotData)
            {
                typelevelDic.Add("task:" + task.Note, maxDepth);
                NameIdDic.Add("task:" + task.Note, 0);
                colorDic.Add("task:" + task.Note, colorDic[IdNameDic[task.CategoryId]]);
            }
            typelevelDic = typelevelDic.OrderByDescending(x => x.Value).ToDictionary(x=>x.Key,x=>x.Value);
            Dictionary<string, long> plotDataFinal = new Dictionary<string, long>();

            foreach (var item in typelevelDic) {
                if (item.Value>=param)
                {
                    //parentCate=invest tempCategories=[invest, Timerecorder, learn...]
                    var tempSubCategories = AllCategories.Where(x => x.ParentCategoryId==NameIdDic[item.Key]||x.Id== NameIdDic[item.Key]).Select(x=> new { x.Id,x.Name}).ToList();
                    long sumLastTime = 0;
                    if (item.Value==maxDepth){
                        sumLastTime = plotData.First(x => ("task:" + x.Note) == item.Key).LastTime.Ticks;
                    }
                    else{
                        //sumLastTime = plotData.Where(x => tempSubCategories.Any(y => y.Id == x.CategoryId)).Sum(x => x.LastTime.Ticks) ;
                        sumLastTime = GetAllSubTime(plotData, item.Key);
                    }
                    
                    plotDataFinal.Add(item.Key, sumLastTime);
                }
            }
            List<int> allTypes = typelevelDic.Where(x => x.Value==param).Select(x => NameIdDic[x.Key]).ToList();
            plotDataFinal = plotDataFinal.Where(x => allTypes.Contains(NameIdDic[x.Key])).ToDictionary(x=>x.Key,x=>x.Value);
            var items = plotDataFinal.Select(x => new ChartBar { Note = x.Key, Type = x.Key, Time = new TimeSpan(x.Value) }).OrderBy(x => x.Type).ThenByDescending(x => x.Time);
            Dictionary<string, ChartBar[]> TypeItemList = new Dictionary<string, ChartBar[]>();
            var allItemCount = 0;
            foreach (string type in plotDataFinal.Keys)
            {
                //if (type == "none") continue;
                var timeItems = items.Where(x => x.Type == type).ToArray();
                TypeItemList.Add(type, timeItems);
                allItemCount += timeItems.Length;
            }
            string[] TimeLabels = new string[allItemCount];
            string[] YLabels = new string[allItemCount];
            double[] position = new double[allItemCount];
            foreach (var TypeItems in TypeItemList)
            {
                if (TypeItems.Key == "none") continue;
                addChartData(TypeItems.Value, TypeItems.Key, ref position, ref YLabels, ref TimeLabels, ref plt, ref index);
            }
            plt.YTicks(position, YLabels);
            plt.Legend(location: Alignment.UpperRight);
            Func<double, string> customFormatter = y => double.IsNaN(y)? "0" :  $"{TimeSpan.FromSeconds(y).ToString()}";
            plt.XAxis.TickLabelFormat(customFormatter);
            plt.YAxis.LabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            plt.XAxis.LabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");

            plt.YAxis.TickLabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            plt.XAxis.TickLabelStyle(fontSize: 14, fontName: Helper.CheckSysFontExisting() ? "微软雅黑" : "Microsoft YaHei");
            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
            CategoryPlot.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            RBWrapPanel.Dispatcher.Invoke(new Action(delegate
            {
                CategoryPlot.Render(lowQuality: false);
                CategoryPlot.Refresh();
            }));
        }
        private void addChartData(ChartBar[] Items, string type, ref double[] position, ref string[] YLabels, ref string[] TimeLabels, ref Plot plt, ref int index)
        {
            if (Items.Count() > 0)
            {
                double[] itemPostion = new double[Items.Count()];
                double[] itemValues = new double[Items.Count()];
                //initColor();
                for (int i = 0; i < Items.Count(); i++)
                {
                    itemPostion[i] = index + i + 1;
                    position[i + index] = index + i + 1;
                    YLabels[i + index] = Items[i].Note;
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
        private int getMaxDepth(int currDepth, int findCategoryId)
        {
            List<Category> allSubCategories = new List<Category>();
            if (displayInvisibleItems == true){
                 allSubCategories = Helper.allcategories.Where(x => x.ParentCategoryId == findCategoryId).ToList();
            }
            else{
                allSubCategories = Helper.allcategories.Where(x => x.ParentCategoryId == findCategoryId && x.Visible).ToList();
            }
            
            if (allSubCategories.Count == 0) return currDepth;
            int a = currDepth;
            foreach(var category in allSubCategories) {
                currDepth = Math.Max(getMaxDepth(a+1, category.Id), currDepth);
            }
            return currDepth;
        }

        private void CalculateBonus(List<ToDoObj> allTasks, List<Category> AllCategories)
        {
            TotalBonus = 0;
            foreach (var task in allTasks)
            {
                int bonus = 0;
                var categoryItem = AllCategories.FirstOrDefault(x => x.Name == task.Category);
                if (categoryItem != null)
                {
                    bonus = categoryItem.BonusPerHour;
                }
                if (bonus==0)
                {
                    categoryItem = AllCategories.FirstOrDefault(x => x.Name == task.Type.ToString());
                    if (categoryItem!= null)
                    {
                        bonus = categoryItem.BonusPerHour;
                    }
                }
                task.Bonus = (int)(task.LastTime.TotalSeconds* bonus/3600);
                TotalBonus = TotalBonus + task.Bonus;
            }
        }
        private async Task updateTaskCategory(ToDoObj task, List<GeneratedToDoTask> AllTasks, int findCategoryId, List<Category> AllCategories)
        {
            //if(findCategoryId == 0)
            //{
                
            //}
            //else
            //{
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
                        var findCategory = AllCategories.FirstOrDefault(x => x.Id == findTask.CategoryId);
                        if (findCategory != null)
                        {
                            task.Category = findCategory.Name;
                        }
                    }
                }
                else
                {
                    findCategoryId = AllCategories.FirstOrDefault(x => x.Name==task.Type.ToString(), new Data.Category()).Id;
                    task.CategoryId = findCategoryId;
                    task.Category = task.Type.ToString();
                    await SQLCommands.UpdateTodo(task);
                }
            //}
            
        }
        private async Task updateTaskIndex(ToDoObj task, List<Category> AllCategories)
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
                int categoryId = AllCategories.FirstOrDefault(x => x.Name == task.Type.ToString(), new Data.Category()).Id;
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

        internal async void RestoreDeleteCategoryToParentCategory(int id, List<Category> AllCategories)
        {
            Category curr = AllCategories.Where(x=>x.Id == id).FirstOrDefault();
            while (curr.ParentCategoryId!=0){
                curr = AllCategories.Where(x => x.Id == curr.ParentCategoryId).FirstOrDefault();
            }
            List<GeneratedToDoTask> AllTasksFromDatabase = SQLCommands.GetTasks(new DateTime(1900, 1, 1), DateTime.Today);
            var AllTasksOfDeleteCategory = AllTasksFromDatabase.Where(x => x.CategoryId == id);
            foreach(var task in AllTasksOfDeleteCategory) {
                task.CategoryId = curr.Id;
                await SQLCommands.UpdateTodo(task);
            }
        }
    }

   
}
 