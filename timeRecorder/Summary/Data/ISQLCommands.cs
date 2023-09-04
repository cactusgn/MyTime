using Summary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public interface ISQLCommands
    {
        public Task<List<MyTime>> GetAllTimeObjs(DateTime startTime, DateTime endTime);
        public  Task<int> UpdateObj(TimeViewObj obj);
        public Task<int> AddObj(TimeViewObj obj);
        public Task<int> DeleteObj(TimeViewObj obj);
        public Task<int> DeleteObjByDate(DateTime date);
        public List<GeneratedToDoTask> GetTasks(DateTime date);
        public Task<int> AddTodo(ToDoObj obj);
        public Task<int> UpdateTodo(ToDoObj obj);
        public Task<int> DeleteTodo(ToDoObj obj);
    }
}
