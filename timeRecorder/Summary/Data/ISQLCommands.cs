using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public interface ISQLCommands
    {
        public List<MyTime> GetAllTimeObjs(DateTime startTime, DateTime endTime);
    }
}
