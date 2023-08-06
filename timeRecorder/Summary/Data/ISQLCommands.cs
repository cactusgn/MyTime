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
    }
}
