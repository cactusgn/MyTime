using Microsoft.EntityFrameworkCore;
using Summary.Common;
using System;
using System.Collections.Generic;
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
        public async Task<int> UpdateObj(TimeViewObj obj)
        {
            using (var context = new MytimeContext())
            {
                var objToUpdate = context.MyTime.FirstOrDefault(x=>x.currentIndex==obj.Id && x.createDate == obj.CreatedDate);
                if(objToUpdate != null && (objToUpdate.note!=obj.Note || objToUpdate.type!=obj.Type))
                {
                    objToUpdate.type = obj.Type;
                    objToUpdate.note = obj.Note;

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
                    type = obj.Type
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
                await context.ToDos.AddAsync(new GeneratedToDoTask() { CreateDate=DateTime.Today, UpdatedDate = DateTime.Today, Note=obj.Note, Finished=obj.Finished, Type=obj.Type.ToString() });
                await context.SaveChangesAsync();
                index = context.ToDos.First(x => x.UpdatedDate==DateTime.Today&&x.Note == obj.Note).Id;
            }
            return index;
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
        public List<GeneratedToDoTask> GetTasks(DateTime date)
        {
            var list = new List<GeneratedToDoTask>();
            using (var context = new MytimeContext())
            {
                list = context.ToDos.Where(x=>x.UpdatedDate == date).ToList();
            }
            return list;
        }
    }
    
}
