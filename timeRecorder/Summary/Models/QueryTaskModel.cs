using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string SelectedContextMenuCategory { get; set; }
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
            foreach (var category in subCategories)
            {
                if (category.Visible)
                {
                    System.Windows.Controls.MenuItem child = new System.Windows.Controls.MenuItem();
                    child.Header = category.Name;
                    child.Command = UpdateCategoryCommand;
                    updateSubContextMenu(Categories, child,category.Id);
                    currentNode.Items.Add(child);
                }
            }
        }

        public async void UpdateContextMenu()
        {
            List<Category> AllCategories = await SQLCommands.GetAllCategories();
            UpdateCategoryMenuItem.Items.Clear();
            updateSubContextMenu(AllCategories, UpdateCategoryMenuItem,0);
        }
        private void UpdateCategory(object obj)
        {
            Console.WriteLine(obj);
        }

        private async void GetSummaryDate()
        {
            if (SQLCommands!=null)
            {
                List<MyTime> allTimeObjs = await SQLCommands.GetAllTimeObjs(StartTime, EndTime);
                List<ToDoObj> allTasks = new List<ToDoObj>();
                if (Category=="invest"||Category=="rest"||Category=="work"||Category=="play")
                {
                    allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime&&x.type==Category).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = Helper.ConvertTimeType(x.First().type) }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                }
                else if (Category=="")
                {
                    allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = Helper.ConvertTimeType(x.First().type) }).ToList();
                }
                else
                {
                    allTasks = allTimeObjs.Where(x => x.createDate>=startTime&&x.createDate<=endTime).OrderBy(x => x.createDate).GroupBy(x => new { x.note }).Select(x => new ToDoObj() { CreatedDate = x.First().createDate, Note = x.Key.note, LastTime = new TimeSpan(x.Sum(x => x.lastTime.Ticks)), Id = x.First().taskId, Type = Helper.ConvertTimeType(x.First().type) }).OrderBy(x => x.LastTime).ThenByDescending(x => x.LastTime).ToList();
                    allTasks = allTasks.Where(x => x.Category==Category).ToList();
                }
                TotalBonus = 0;
                List<Category> AllCategories = await SQLCommands.GetAllCategories();
                foreach (ToDoObj task in allTasks)
                {
                    if (task.Id == 0)
                    {
                       await updateTaskIndex(task);
                    }
                    int bonus = 0;
                    var categoryItem = AllCategories.FirstOrDefault(x => x.Name == task.Category);
                    if(categoryItem != null) {
                        bonus = categoryItem.BonusPerHour;
                    }
                    if (bonus==0)
                    {
                        categoryItem = AllCategories.FirstOrDefault(x => x.Name == task.Type.ToString());
                        if(categoryItem!= null )
                        {
                            bonus = categoryItem.BonusPerHour;
                        }
                    }
                    task.Bonus = (int)(task.LastTime.TotalSeconds* bonus/3600);
                    TotalBonus = TotalBonus + task.Bonus;
                }
                CategoryDataGridSource = new ObservableCollection<ToDoObj>(allTasks);
                totalCost = new TimeSpan(allTasks.Sum(x => x.LastTime.Ticks));
                TotalCostString = $"{totalCost.Hours}h{totalCost.Minutes}m";
                AverageCost = (totalCost/(EndTime-StartTime).Days);
                AverageCost = TimeSpan.FromMinutes((int)AverageCost.TotalMinutes);
                
            }
            
        }
        private async Task updateTaskIndex(ToDoObj task)
        {
            int findIndex = SQLCommands.QueryTodo(task.Note);
            if (findIndex==0)
            {
                findIndex =  await SQLCommands.AddTodo(task);
                task.Id = findIndex;
                List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(task.Note);
                if (timeObjs!=null)
                {
                    foreach (MyTime timeObj in timeObjs)
                    {
                        timeObj.taskId = findIndex;
                        await SQLCommands.UpdateObj(timeObj);
                    }
                }
            }
            else
            {
                task.Id = findIndex;
                List<MyTime> timeObjs = SQLCommands.GetTimeObjsByName(task.Note);
                foreach (MyTime timeObj in timeObjs)
                {
                    timeObj.taskId = findIndex;
                    await SQLCommands.UpdateObj(timeObj);
                }
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
    }

   
}
