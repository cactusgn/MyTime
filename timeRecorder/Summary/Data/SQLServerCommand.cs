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
                MyTime newObj = new MyTime() {
                    currentIndex = obj.Id,
                    startTime = obj.StartTime,
                    endTime = obj.EndTime,
                    createDate = obj.CreatedDate,
                    note = obj.Note,
                    type = obj.Type
                };
                
                await context.MyTime.AddAsync(newObj);
                await context.SaveChangesAsync();

            }
            return 1;
        }
    }
    
}
