using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeRecorder
{
    public class TimeRecordObj : IComparer<TimeRecordObj>
    {
        public DateTime mStartTime = DateTime.Parse("1994-11-11");
        public DateTime mEndTime = DateTime.Parse("1994-11-11");
        public TimeSpan interval;
        public TimeType timeType;
        public Label rightLabel;
        public TextBox textBox = new TextBox();
        public int index;
        public string comment;
        public bool selected = false;
        //public delegate void ClickTimeRecordObj(object sender, EventArgs e);
        //public ClickTimeRecordObj setCurrentObj;

        public TimeRecordObj()
        {
        }
        public TimeRecordObj(DateTime startTime, DateTime endTime)
        {
            mStartTime = startTime;
            mEndTime = endTime;
            timeType = TimeType.none;
        }

        public int Compare(TimeRecordObj x, TimeRecordObj y)
        {
            return x.mStartTime.CompareTo(y.mStartTime);//升序
        }

        public enum TimeType
        {
            none,
            waste,
            work,
            rest,
            study
        };
    }
}
