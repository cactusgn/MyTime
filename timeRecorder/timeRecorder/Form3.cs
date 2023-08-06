using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static timeRecorder.Form1;

namespace timeRecorder
{
    public partial class Form3 : Form
    {
        Label endLabel = new Label();
        public List<TimeRecordObj> timelist = new List<TimeRecordObj>();
        public List<TimeRecordObj> timelistForChart = new List<TimeRecordObj>();
        public List<TimeRecordObj> timelistForFilterChart = new List<TimeRecordObj>();
        DateTime currentDate;
        public Form3(List<TimeRecordObj> timelistForChart)
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            foreach(TimeRecordObj a in timelistForChart){
                this.timelistForChart.Add(a);
            }
        }
        private string format_date(TimeSpan time)
        {
            return String.Format("{0:00}", time.Hours) + ":" + String.Format("{0:00}", time.Minutes) + ":" + String.Format("{0:00}", time.Seconds);
        }
        private string format_date(DateTime time)
        {
            return String.Format("{0:00}", time.Hour) + ":" + String.Format("{0:00}", time.Minute) + ":" + String.Format("{0:00}", time.Second);
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            update_chart(timelistForChart);
        }
        private void update_chart(List<TimeRecordObj> timelist)
        {
            TimeSpan waste = new TimeSpan(0, 0, 0);
            TimeSpan work = new TimeSpan(0, 0, 0);
            TimeSpan rest = new TimeSpan(0, 0, 0);
            TimeSpan study = new TimeSpan(0, 0, 0);
            TimeSpan none = new TimeSpan(0, 0, 0);

            int i = 0;
            for (i = 0; i < timelist.Count; i++)
            {
                switch (timelist[i].timeType)
                {
                    case TimeRecordObj.TimeType.waste:
                        waste = waste + timelist[i].interval;
                        break;
                    case TimeRecordObj.TimeType.work:
                        work = work + timelist[i].interval;
                        break;
                    case TimeRecordObj.TimeType.study:
                        study = study + timelist[i].interval;
                        break;
                    case TimeRecordObj.TimeType.rest:
                        rest = rest + timelist[i].interval;
                        break;
                    case TimeRecordObj.TimeType.none:
                        none = none + timelist[i].interval;
                        break;
                }
            }
            Dictionary<string, TimeSpan> sum = new Dictionary<string, TimeSpan>();
            Dictionary<string, string> sumType = new Dictionary<string, string>();
            timelist.Sort((TimeRecordObj mx, TimeRecordObj my) => //该方法实现的是将Asm由大到小的排序
            {
                if (mx.timeType > my.timeType) return -1;  //返回-1表示mx被认定排序值小于my,所以排在前面
                else if (mx.timeType < my.timeType) return 1; //返回1 表示mx被认定排序值大于my,所以排在后面.
                else return 0;
            });
            foreach (TimeRecordObj obj in timelist)
            {
                if (obj.comment.Equals(""))
                {
                    obj.comment = obj.timeType.ToString();
                }
                if (sum.ContainsKey(obj.comment))
                {
                    sum[obj.comment] = sum[obj.comment] + obj.interval;
                }
                else
                {
                    sum.Add(obj.comment, obj.interval);
                    sumType.Add(obj.comment, obj.timeType.ToString());
                }
            }
            List<string> xData = new List<string>();
            List<double> yData = new List<double>();
            foreach (string key in sum.Keys)
            {
                xData.Add(key + " " + format_date(sum[key]));
                yData.Add(sum[key].TotalSeconds);
            }
            chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
            chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
            chart1.Series[0].Points.DataBindXY(xData, yData);
            List<string> xData2 = new List<string>() { "study", "work", "rest", "waste" };
            List<double> yData2 = new List<double>() { study.TotalSeconds, work.TotalSeconds, rest.TotalSeconds, waste.TotalSeconds };
            i = 0;
            int rest_item = 0;
            int work_item = 0;
            int study_item = 0;
            int waste_item = 0;
            int none_item = 0;
            foreach (string key in sumType.Keys)
            {
                if (sumType[key] == TimeRecordObj.TimeType.rest.ToString())
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(144 - rest_item * 5, 238, 144 - rest_item * 5);
                    if (rest_item < 28)
                        rest_item++;
                }
                else if (sumType[key] == TimeRecordObj.TimeType.study.ToString())
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(255, 195 - study_item * 5, 205 - study_item * 5);
                    if (study_item < 37)
                        study_item++;
                }
                else if (sumType[key] == TimeRecordObj.TimeType.work.ToString())
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(255, 242 - work_item * 5, 0 + work_item * 5);
                    if (work_item < 37)
                        work_item++;
                }
                else if (sumType[key] == TimeRecordObj.TimeType.waste.ToString())
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(255, 106 - waste_item * 5, 106 - waste_item * 5);
                    if (waste_item < 21)
                        waste_item++;
                }
                else
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(248 - none_item * 5, 248 - none_item * 5, 255 - none_item * 5);
                    none_item++;
                }
                i++;
            }
        }
       

        private void textBoxChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].textBox == sender)
                {
                    if (((TextBox)sender).Text.Length > 8)
                    {
                        timelist[i].comment = ((TextBox)sender).Text.Substring(8).TrimStart();
                        //if (useDatabase)
                        //{
                            string strcomm = "update mytime set note='" + timelist[i].comment + "'where currentIndex = " + timelist[i].index.ToString() + " and createDate='" + from_date.Value.Date + "'";
                            //update FilTer set 列名 = value where id = 3
                            SqlHelper.ExecuteNoQuery(strcomm);
                        //}
                        //else
                        //{
                        //    DataRow[] arrRows = dt.Select("CI='" + timelist[i].index + "'");
                        //    if (arrRows.Length > 0)
                        //        arrRows[0]["note"] = timelist[i].comment;
                        //}
                    }
                }
            }
        }

        private void setCurrentTextBoxObj(object sender, EventArgs e)
        {
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].textBox == sender)
                {
                    timelist[i].selected = true;
                }
                else
                {
                    timelist[i].selected = false;
                }
            }
        }
        private void LoadTimeInfo(DateTime fromDate,DateTime toDate)
        {
            string sqlText = "select currentIndex as CI,startTime,endTime,lastTime,note,type,createDate from mytime where createDate>='" + fromDate + "' and createDate <='" + toDate + "' order by CI";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlText);
            timelist.Clear();
            timelistForChart.Clear();
            panel2.Controls.Clear();
            panel2.Controls.Add(endLabel);
            
            foreach (DataRow row in dt.Rows)
            {
                TimeRecordObj timeObj = new TimeRecordObj();
                timeObj.mStartTime = DateTime.Parse(row["startTime"].ToString().Trim());
                timeObj.mEndTime = DateTime.Parse(row["endTime"].ToString().Trim());
                timeObj.interval = timeObj.mEndTime - timeObj.mStartTime;
                timeObj.comment = row["note"].ToString().Trim();
                timeObj.index = int.Parse(row["CI"].ToString());
                Type type = typeof(TimeRecordObj.TimeType);
                if (row["type"].ToString().Trim() == "")
                    timeObj.timeType = TimeRecordObj.TimeType.none;
                else
                    timeObj.timeType = (TimeRecordObj.TimeType)Enum.Parse(type, row["type"].ToString().Trim());
                Label description = new Label();
                description.BackColor = Color.Transparent;
                timeObj.rightLabel = description;
                switch (timeObj.timeType.ToString())
                {
                    case "study":
                        timeObj.textBox.BackColor = Color.FromArgb(255, 174, 201);
                        break;
                    case "work":
                        timeObj.textBox.BackColor = Color.FromArgb(255, 242, 0);
                        break;
                    case "rest":
                        timeObj.textBox.BackColor = Color.FromArgb(192, 255, 192);
                        break;
                    case "waste":
                        timeObj.textBox.BackColor = Color.FromArgb(255, 128, 128);
                        break;
                    default:
                        break;
                }
                timeObj.textBox.Location = new Point(50, 10);
                timeObj.textBox.Multiline = true;
                timeObj.textBox.Click += setCurrentTextBoxObj;
                timeObj.textBox.TextChanged += textBoxChanged;
                
                timelistForChart.Add(timeObj);
                if (DateTime.Parse(row["createDate"].ToString().Trim()) == fromDate)
                {
                    panel2.Controls.Add(description);
                    panel2.Controls.Add(timeObj.textBox);
                    timelist.Add(timeObj);
                }
            }
            calcHeight();
        }
        private void calcHeight()
        {
            TimeSpan total = new TimeSpan();
            int i = 0;
            double height;
            double startPoint = 10;
            TimeSpan minTime = new TimeSpan(24, 0, 0);
            double totalHeight = 0;
            //按开始时间对timelist进行排序
            IComparer<TimeRecordObj> comparer = new TimeRecordObj();
            timelist.Sort(comparer);
            for (i = 0; i < timelist.Count; i++)
            {
                timelist[i].interval = timelist[i].mEndTime - timelist[i].mStartTime;
                //记录所有的间隔时间总和
                total += timelist[i].interval;
                if (timelist[i].interval < minTime)
                {
                    //记录最小的timelist的间隔
                    minTime = timelist[i].interval;
                }
            }
            //计算按最小间隔为30，来计算时间总高度
            totalHeight = total.TotalMilliseconds / minTime.TotalMilliseconds * 30;
            //如果时间总高度大于窗口高度，则设置时间总高度为窗口高度
            if (totalHeight > panel2.Height - 30)
            {
                totalHeight = panel2.Height - 30;
            }
            for (i = 0; i < timelist.Count; i++)
            {
                timelist[i].rightLabel.Text = format_date(timelist[i].mStartTime);
                timelist[i].rightLabel.Location = new Point(260, Convert.ToInt32(startPoint));
                for (int j = i - 1; j >= 0; j--)
                {
                    if (timelist[j].rightLabel.Visible)
                    {
                        if (timelist[i].rightLabel.Location.Y - timelist[j].rightLabel.Location.Y < 20)
                        {
                            timelist[i].rightLabel.Visible = false;
                        }
                        break;
                    }
                }
                timelist[i].textBox.Location = new Point(50, Convert.ToInt32(startPoint));
                timelist[i].textBox.Width = 200;
                timelist[i].textBox.Text = format_date(timelist[i].interval) + " " + timelist[i].comment;
                height = Math.Round(timelist[i].interval.TotalMilliseconds / total.TotalMilliseconds * totalHeight, 2);
                timelist[i].textBox.Height = Convert.ToInt32(height);
                if (timelist[i].textBox.Height == 0)
                {
                    timelist[i].textBox.Visible = false;
                    timelist[i].rightLabel.Visible = false;
                }
                startPoint = startPoint + Convert.ToInt32(height);
                if (i == timelist.Count - 1)
                {
                    endLabel.Location = new Point(260, Convert.ToInt32(startPoint) - 5);
                    endLabel.BackColor = Color.Transparent;
                    endLabel.Text = format_date(timelist[i].mEndTime);
                    endLabel.Visible = true;
                }
            }
        }
        private void refresh_btn_Click(object sender, EventArgs e)
        {
            currentDate = from_date.Value.Date;
            LoadTimeInfo(from_date.Value.Date, to_date.Value.Date);
            update_chart(timelistForChart);
        }

        private void tgl_oneday_CheckedChanged(object sender, EventArgs e)
        {
            if(tgl_oneday.Checked){
                to_date.Enabled = false;
                currentDate = from_date.Value.Date;
            }
            else{
                to_date.Enabled = true;
            }
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            calcHeight();
        }
        private TimeRecordObj selectedTimeObj()
        {
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].selected)
                {
                    return timelist[i];
                }
            }
            return null;
        }
        private void update_timetype(string timetype, int index)
        {
            //if (useDatabase)
            //{
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@type", timetype));
                SqlHelper.ExecuteNoQuery("update mytime set type=@type where currentIndex='" + index.ToString() + "' and createDate='" + currentDate + "'", parameters.ToArray());
            //}
            //else
            //{
            //    DataRow[] arrRows = dt.Select("CI='" + index.ToString() + "'");
            //    arrRows[0]["type"] = timetype;
            //}
           // LoadTimeInfo(currentDate, currentDate);
        }
        private void rest_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("rest", currentObj.index);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.rest;
                currentObj.textBox.BackColor = Color.FromArgb(192, 255, 192);
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }
        private void study_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("study", currentObj.index);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.study;
                currentObj.textBox.BackColor = Color.FromArgb(255, 174, 201);
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void waste_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("waste", currentObj.index);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.waste;
                currentObj.textBox.BackColor = Color.FromArgb(255, 128, 128);
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void work_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("work", currentObj.index);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.work;
                currentObj.textBox.BackColor = Color.FromArgb(255, 242, 0);
             
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void Tgl_all_CheckedChanged(object sender, EventArgs e)
        {
            
            if (tgl_all.Checked)
            {
                tgl_rest.Checked = true;
                tgl_study.Checked = true;
                tgl_waste.Checked = true;
                tgl_work.Checked = true;
            }
            else
            {
                tgl_rest.Checked = false;
                tgl_study.Checked = false;
                tgl_waste.Checked = false;
                tgl_work.Checked = false;
            }
            update_chart(timelistForChart);
        }
        private void update_filter()
        {
            timelistForFilterChart.Clear();
            foreach (TimeRecordObj a in timelistForChart)
            {
                switch (a.timeType)
                {
                    case TimeRecordObj.TimeType.rest:
                        if (tgl_rest.Checked)
                        {
                            timelistForFilterChart.Add(a);
                        }
                        break;
                    case TimeRecordObj.TimeType.waste:
                        if (tgl_waste.Checked)
                        {
                            timelistForFilterChart.Add(a);
                        }
                        break;
                    case TimeRecordObj.TimeType.study:
                        if (tgl_study.Checked)
                        {
                            timelistForFilterChart.Add(a);
                        }
                        break;
                    case TimeRecordObj.TimeType.work:
                        if (tgl_work.Checked)
                        {
                            timelistForFilterChart.Add(a);
                        }
                        break;
                }
            }
            update_chart(timelistForFilterChart);
        }
        private void Tgl_study_CheckedChanged(object sender, EventArgs e)
        {
            update_filter();
        }

        private void Tgl_work_CheckedChanged(object sender, EventArgs e)
        {
            update_filter();
        }

        private void Tgl_waste_CheckedChanged(object sender, EventArgs e)
        {
            update_filter();
        }

        private void Tgl_rest_CheckedChanged(object sender, EventArgs e)
        {
            update_filter();
        }
    }
}
