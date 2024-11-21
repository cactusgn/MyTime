using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Common
{
    public class TaskView
    {
        public int taskId { get; set; }
        public string taskName { get; set; }
        public TimeSpan lastTime { get; set; }
        public DateTime taskDate { get; set; }
        public string DayOfWeek
        {
            get { return taskDate.DayOfWeek.ToString(); }
        }
    }
}
