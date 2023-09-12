using Microsoft.EntityFrameworkCore;
using Summary.Common;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public  class SQLServerCommand: ISQLCommands
    {
        public async Task<List<MyTime>> GetAllTimeObjs(DateTime startTime, DateTime endTime)
        {
            var list = new List<MyTime>();
            using (var context = new MytimeContext())
            {
                if (context.MyTime.ToList().Count == 0)
                {
                    var cate1 = new MyTime { currentIndex =1 };
                    var cate2 = new MyTime { currentIndex = 2 };
                    context.MyTime.AddRange(cate1, cate2);
                    await context.SaveChangesAsync();
                }
                // 查询
                list = context.MyTime.Where(x => x.createDate>=startTime && x.createDate<=endTime).ToList();
            }
            return list;
        }
        public List<MyTime> GetTimeObjsByName(string name)
        {
            List<MyTime> list = null;
            using (var context = new MytimeContext())
            {
                // 查询
                list = context.MyTime.Where(x => x.note==name).ToList();
            }
            return list;
        }
        public List<MyTime> GetTimeObjsByType(string type)
        {
            List<MyTime> list = null;
            using (var context = new MytimeContext())
            {
                // 查询
                list = context.MyTime.Where(x => x.type.Trim()==type).ToList();
            }
            return list;
        }
        public async Task<int> UpdateObj(TimeViewObj obj)
        {
            using (var context = new MytimeContext())
            {
                var objToUpdate = context.MyTime.FirstOrDefault(x=>x.currentIndex==obj.Id && x.createDate == obj.CreatedDate);
                if(objToUpdate != null && (objToUpdate.note!=obj.Note || objToUpdate.type!=obj.Type))
                {
                    objToUpdate.type = obj.Type;
                    objToUpdate.note = obj.Note;
                    objToUpdate.taskId = obj.TaskId;
                    var allNotes = context.MyTime.Where(x => x.note==obj.Note && x.createDate == obj.CreatedDate);
                    foreach (var note in allNotes)
                    {
                        note.type = obj.Type;
                    }
                    await context.SaveChangesAsync();
                }
                
            }
            return 1;
        }
        public async Task<int> UpdateObj(MyTime obj)
        {
            using (var context = new MytimeContext())
            {
                var objToUpdate = context.MyTime.FirstOrDefault(x => x.currentIndex==obj.currentIndex && x.createDate == obj.createDate);
                if (objToUpdate != null)
                {
                    objToUpdate.type = obj.type.Trim();
                    objToUpdate.note = obj.note;
                    objToUpdate.taskId = obj.taskId;
                    await context.SaveChangesAsync();
                }

            }
            return 1;
        }
        public async Task<int> AddObj(TimeViewObj obj)
        {
            using (var context = new MytimeContext())
            {
                var todayItems = context.MyTime.Where(x => x.createDate == obj.CreatedDate);
                var MaxIndex = 0;
                if (todayItems.Count() > 0) {
                    MaxIndex = todayItems.Max(x => x.currentIndex);
                }
                obj.Id = MaxIndex + 1;
                if(obj.Type == "rest")
                {
                    var oldTimeObj = todayItems.Where(x=>x.note== obj.Note&&x.type!="none");
                    if(oldTimeObj.Count()>0) {
                        obj.Type = oldTimeObj.First().type;
                    }
                }
                MyTime newObj = new MyTime() {
                    currentIndex = obj.Id,
                    startTime = obj.StartTime,
                    endTime = obj.EndTime,
                    lastTime = obj.LastTime,
                    createDate = obj.CreatedDate,
                    note = obj.Note,
                    type = obj.Type,
                    taskId = obj.TaskId
                };
                
                await context.MyTime.AddAsync(newObj);
                await context.SaveChangesAsync();

            }
            return 1;
        }
        public async Task<int> DeleteObj(TimeViewObj obj){
            using(var context = new MytimeContext()) {
                var item = context.MyTime.Where(x => x.currentIndex == obj.Id && x.createDate == obj.CreatedDate);
                await item.ExecuteDeleteAsync();
                return 1;
            }
        }
        public async Task<int> DeleteObjByDate(DateTime date)
        {
            using (var context = new MytimeContext())
            {
                var item = context.MyTime.Where(x => x.createDate == date);
                await item.ExecuteDeleteAsync();
                return 1;
            }
        }
        public async Task<int> AddTodo(ToDoObj obj)
        {
            int index = 1;
            using (var context = new MytimeContext())
            {
                await context.ToDos.AddAsync(new GeneratedToDoTask() { CreateDate=obj.CreatedDate, UpdatedDate = DateTime.Today, Note=obj.Note, Finished=obj.Finished, Type=obj.Type.ToString(), CategoryId=obj.CategoryId });
                await context.SaveChangesAsync();
                index = context.ToDos.First(x => x.UpdatedDate == DateTime.Today&&x.Note == obj.Note).Id;
            }
            return index;
        }
        public GeneratedToDoTask QueryTodo(string note)
        {
            using (var context = new MytimeContext())
            {
                var item = context.ToDos.Where(x => x.Note == note);
                if (item.Count()>0)
                {
                    return item.First();
                }
            }
            return null;
        }
        public async Task<int> UpdateTodo(ToDoObj obj)
        {
            using (var context = new MytimeContext())
            {
                var item = context.ToDos.Where(x=>x.Note == obj.Note);
                if (item!=null)
                {
                    var updateObj = item.First();
                    updateObj.UpdatedDate = DateTime.Today;
                    updateObj.Type = obj.Type.ToString();
                    updateObj.CategoryId = obj.CategoryId;
                    updateObj.Finished = obj.Finished;
                }
                await context.SaveChangesAsync();
            }
            return 1;
        }
       
        public async Task<int> DeleteTodo(ToDoObj obj)
        {
            using (var context = new MytimeContext())
            {
                var item = context.ToDos.Where(x => x.UpdatedDate==DateTime.Today&&x.Note == obj.Note);
                if(item!=null)
                    await item.ExecuteDeleteAsync();
            }
            return 1;
        }
        public List<GeneratedToDoTask> GetTasks(DateTime startDate, DateTime endDate)
        {
            var list = new List<GeneratedToDoTask>();
            using (var context = new MytimeContext())
            {
                if (startDate.Year==1900)
                {
                    list = context.ToDos.ToList();
                }
                else
                {
                    list = context.ToDos.Where(x => x.CreateDate >= startDate && x.CreateDate<=endDate).ToList();
                }
                
            }
            return list;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var list = new List<Category>();
            using (var context = new MytimeContext())
            {
                if (context.Categories.ToList().Count == 0)
                {
                    //Category root = new Category() { Name = "", Color = "", Visible = true, BonusPerHour = 0,ParentCategoryId=-1 };
                    //context.Categories.AddRange(root);
                    //await context.SaveChangesAsync();
                    Category invest = new Category() { Name = "invest", Color = "#FFB6C1", Visible = true, BonusPerHour = 20 };
                    Category work = new Category() { Name = "work", Color = "#FFD700", Visible = true, BonusPerHour = 0 };
                    Category play = new Category() { Name = "play", Color = "#ADD8E6", Visible = true, BonusPerHour = 0 };
                    Category rest = new Category() { Name = "rest", Color = "#98FB98", Visible = true, BonusPerHour = 0 };
                    context.Categories.AddRange(invest, work, play, rest);
                    await context.SaveChangesAsync();
                }
                list = context.Categories.ToList();
            }
            return list;
        }
        public async Task<int> UpdateCategory(AddCategoryModel category)
        {
            using (var context = new MytimeContext()){
                var oldCategory = context.Categories.Where(x => x.Id == category.Id);
                if (oldCategory.Count()>0){
                    var obj = oldCategory.First();
                    obj.Color = category.SelectedColor;
                    obj.Name = category.Category;
                    obj.ParentCategoryId = category.ParentId;
                    obj.BonusPerHour = category.Bonus;
                    obj.Visible = category.Visible;
                }
                await context.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<int> AddCategory(AddCategoryModel category)
        {
            using (var context = new MytimeContext())
            {
                Category cate = new Category() { 
                    Name = category.Category, 
                    Color = category.SelectedColor,
                    ParentCategoryId = category.ParentId,
                    BonusPerHour = category.Bonus,
                    Visible = category.Visible
                };
                context.Categories.Add(cate);
                await context.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<int> DeleteCategory(int id){
            using (var context = new MytimeContext()){
                var item = context.Categories.Where(x => x.Id == id);
                var subItems = context.Categories.Where(x=>x.ParentCategoryId == id).ToList();
                foreach (var category in subItems)
                {
                    await DeleteCategory(category.Id);
                }
                await item.ExecuteDeleteAsync();
            }
            return 1;
        }
    }
    
}
