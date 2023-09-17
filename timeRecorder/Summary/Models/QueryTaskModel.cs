using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Summary.Models
{
    public class QueryTaskModel:ViewModelBase
    {
		private DateTime startTime = DateTime.Today.AddDays(-6);

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
            set { category = value;  GetSummaryDate(); }
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
        public ISQLCommands SQLCommands { get; set; }
        private int SelectedContextMenuCategoryId { get; set; }
        private string SelectedContextMenuType { get; set; }
        public Dictionary<string, int> TypesDic { get; set; } = new Dictionary<string, int>();

        public QueryTaskModel(string category, DateTime startTime, DateTime endTime, ISQLCommands sqlCommands)
        {
            ClickOkButtonCommand = new MyCommand(clickOkButton);
            UpdateCategoryCommand = new MyCommand(UpdateCategory);
            CellEditEndingCommand = new MyCommand(CellEditEnding);
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
            SQLCommands = sqlCommands;
            GetSummaryDate();
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
            List<Category> AllCategories = await SQLCommands.GetAllCategories();
            UpdateCategoryMenuItem.Items.Clear();
            TypesDic.Clear();
            updateSubContextMenu(AllCategories, UpdateCategoryMenuItem,0);
        }
        private async void UpdateCategory(object obj)
        {
            System.Collections.IList items = (System.Collections.IList)obj;
            var collection = items.Cast<ToDoObj>();
            List<ToDoObj> selectedTasks = collection.ToList();
            List<Category> allCategories = await SQLCommands.GetAllCategories();
            foreach (var task in selectedTasks) {
                task.CategoryId = SelectedContextMenuCategoryId;
                task.Category = allCategories.First(x=>x.Id ==SelectedContextMenuCategoryId).Name;
                task.Type = SelectedContextMenuType;
                await SQLCommands.UpdateTodo(task);
            }
            GetSummaryDate();
        }

        private async void GetSummaryDate()
        {
            if (SQLCommands!=null)
            {
                List<MyTime> allTimeObjs = await SQLCommands.GetAllTimeObjs(StartTime, EndTime);
                List<ToDoObj> allTasks = new List<ToDoObj>();
                List<Category> AllCategories = await SQLCommands.GetAllCategories();
                List<GeneratedToDoTask> AllTasksFromDatabase = SQLCommands.GetTasks(new DateTime(1900,1,1), EndTime);
                int findCategoryId = AllCategories.FirstOrDefault(x => x.Name == Category, new Data.Category()).Id;
                if (Category=="") findCategoryId=0;
                List<Category> BasicCategories = AllCategories.Where(x => x.ParentCategoryId==0).ToList();
                if (BasicCategories.Where(x=>x.Name==Category).Count()>0)
                {
                    allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type==Category&&x.note!=null).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = x.First().type}).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                }
                else
                {
                    allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type!= null&&x.type!="none"&&x.note!=null).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = x.First().type }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                }
                foreach (ToDoObj task in allTasks)
                {
                    if (task.Id == 0||AllTasksFromDatabase.Where(x=>x.Note==task.Note&&x.Type==Category).Count()==0)
                    {
                       await updateTaskIndex(task, AllCategories);
                    }
                    //if (Category!="")
                    //{
                        await updateTaskCategory(task, AllTasksFromDatabase, findCategoryId, AllCategories);
                    //}
                    if (task.Category == "" || task.Category == null)
                    {
                        task.Category = task.Type.ToString();
                    }
                }
                if (findCategoryId!=0)
                {
                    allTasks = allTasks.Where(x => x.CategoryId == findCategoryId).ToList();
                    
                }
                CalculateBonus(allTasks, AllCategories);
                CategoryDataGridSource = new ObservableCollection<ToDoObj>(allTasks);
                totalCost = new TimeSpan(allTasks.Sum(x => x.LastTime.Ticks));
                TotalCostString = $"{totalCost.TotalHours.ToString("0.00")}h";
                AverageCost = (totalCost/(EndTime-StartTime).Days);
                AverageCost = TimeSpan.FromMinutes((int)AverageCost.TotalMinutes);
            }
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
 