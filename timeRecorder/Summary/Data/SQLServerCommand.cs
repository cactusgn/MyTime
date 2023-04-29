using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public  class SQLServerCommand: ISQLCommands
    {
        public  List<MyTime> GetAllTimeObjs(DateTime startTime, DateTime endTime)
        {
            var list = new List<MyTime>();
            using (var context = new MytimeContext())
            {
                if (context.MyTime.ToList().Count == 0)
                {
                    var cate1 = new MyTime { currentIndex =1 };
                    var cate2 = new MyTime { currentIndex = 2 };
                    context.MyTime.AddRange(cate1, cate2);
                    context.SaveChanges();
                }
                // 查询
                list = context.MyTime.Where(x => x.createDate>=startTime && x.createDate<=endTime).ToList();
            }
            return list;
        }

        
    }
}
