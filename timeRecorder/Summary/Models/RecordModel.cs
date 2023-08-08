using Summary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Models
{
    public class RecordModel : ViewModelBase
    {
        private List<ToDoObj> todayList = new List<ToDoObj>();
        public List<ToDoObj> TodayList{
        get { 
                return todayList; 
            }
        set{
                todayList = value;
                OnPropertyChanged();
            }
        }
        public RecordModel() {
            todayList.Add(new ToDoObj(){ Note="test1", Finished=false });
            todayList.Add(new ToDoObj() { Note = "test2", Finished = false });
        }
    }
}
