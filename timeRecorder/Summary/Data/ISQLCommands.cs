using Summary.Common;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public interface ISQLCommands
    {
        #region TimeObjs
        public Task<List<MyTime>> GetAllTimeObjs(DateTime startTime, DateTime endTime);
        public  Task<int> UpdateObj(TimeViewObj obj);
        public Task<int> AddObj(TimeViewObj obj);
        public Task<int> DeleteObj(TimeViewObj obj);
        public Task<int> DeleteObjByDate(DateTime date);
        #endregion

        #region GeneratedToDoTask
        public List<GeneratedToDoTask> GetTasks(DateTime date);
        public Task<int> AddTodo(ToDoObj obj);
        public Task<int> UpdateTodo(ToDoObj obj);
        public Task<int> DeleteTodo(ToDoObj obj);
        #endregion
        #region category
        public Task<List<Category>> GetAllCategories();
        public Task<int> UpdateCategory(AddCategoryModel category);
        public Task<int> AddCategory(AddCategoryModel category);
        public Task<int> DeleteCategory(int id);

        #endregion
    }
}
