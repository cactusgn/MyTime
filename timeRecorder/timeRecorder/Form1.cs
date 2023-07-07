using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeRecorder
{
   
    public partial class Form1 : Form
    {
        private class BaseTextBox : TextBox
        {

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr LoadLibrary(string lpFileName);
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams prams = base.CreateParams;
                    if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                    {
                        prams.ExStyle |= 0x020; // transparent 
                        prams.ClassName = "RICHEDIT50W";
                    }
                    return prams;
                }
            }


        }


        public static void DeleteDirectory(string directoryPath, string fileName)
        {

            //删除文件
            for (int i = 0; i < Directory.GetFiles(directoryPath).ToList().Count; i++)
            {
                if (Directory.GetFiles(directoryPath)[i].Substring(directoryPath.Length+1) == fileName)
                {
                    File.Delete(Directory.GetFiles(directoryPath)[i]);
                }
            }
        }

        System.Windows.Forms.Timer showTextBoxTimer = new System.Windows.Forms.Timer(); //新建一个Timer对象
        DateTime startTime = new DateTime();
        DateTime stopTime = new DateTime();
        public Label endLabel = new Label();
        public List<TimeRecordObj> timelist = new List<TimeRecordObj>();
        TimeRecordObj currentObj = new TimeRecordObj();
        //BaseTextBox textBox1 = new BaseTextBox();
        Form2 form2;
        public bool useDatabase = false;
        bool firstLoad = true;
        public DataTable dt = null;
        bool hasRemindCurrentTask = false;
        public bool hideform1 = false;
        bool inResetProgress = false;
        Dictionary<string,bool> workContents = new Dictionary<string,bool>();
        TimeSpan accumulateTime;
        BaseTextBox memo = new BaseTextBox();
        ComboBox typeCombo = new ComboBox();
        public int remindTime = 0;
        int CI = 0;
        bool copiedFromList = false;
        public Form1()
        {
            InitializeComponent();
            form2 = new Form2(this);
        }
        private void addFocusEvent(Control parent){
            foreach (Control control in parent.Controls)
            {
                if (control.GetType() != typeof(ComboBox) && control.GetType() != typeof(DataGridView))
                {
                    control.Click += control_click;
                }
                if(control.Controls.Count>0){
                    addFocusEvent(control);
                }
            }
        }
        private void control_click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        private void deleteFile(){
            DeleteDirectory(ConfigurationManager.AppSettings["outputDirectory"].ToString(), "time_record.txt");
        }
        public static void addText(String name)
        {
            FileStream fs = new FileStream(ConfigurationManager.AppSettings["outputDirectory"].ToString()+ "\\time_record.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(name);
            sw.Close();
        }
        public void btn_start_Click(object sender, EventArgs e)
        {
            
            hasRemindCurrentTask = false;
            if (ThingsToDo.Text.Equals("")){
                MessageBox.Show("请填写事件","Tip", MessageBoxButtons.OK,MessageBoxIcon.Question);
                return;
            }
            accumulateTime = new TimeSpan(0);
            form2.Mode = "start";
            startTime = DateTime.Now;
            label1.Text = "间隔时间：";
            form2.SetWork(ThingsToDo.Text);
            if (timelist.Count >0)
            {
                stopTime = timelist[timelist.Count - 1].mEndTime;
                TimeRecordObj time = null;
                if(Math.Abs((stopTime - startTime).TotalMinutes) > 2){
                    InputDialog inp = new InputDialog("输入休息期间做的事吧：");
                    DialogResult dr = inp.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        time = addTimeRecord(stopTime, startTime, inp.Value,type:"rest");
                    }
                    inp.Dispose();
                }
                else{
                    time = addTimeRecord(stopTime, startTime, ConfigurationManager.AppSettings["defaultRest"].ToString(), "rest");
                }
                for (int i = 0; i < timelist.Count; i++)
                {
                    if (timelist[i].textBox == time.textBox)
                    {
                        timelist[i].selected = true;
                    }
                    else
                    {
                        timelist[i].selected = false;
                    }
                    if(timelist[i].comment == ThingsToDo.Text)
                    {
                        accumulateTime += timelist[i].interval;
                    }
                }
            }
            remindTime = Convert.ToInt32(TimeReminder.Text);
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Tick += new EventHandler(showTextBoxTimer_Tick);//到时所有执行的动作
            showTextBoxTimer.Start();//启动计时
            btn_start.Enabled = false;
            btn_start.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始2.png");
            end_btn.Enabled = true;
            end_btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止.png");
            reset.Enabled = false;
            reset.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\导入2.png");
            output_btn.Enabled = false;
            output_btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\导出2.png");
            calcHeight();
        }
        private void showTextBoxTimer_Tick_stopMode(object sender, EventArgs e){
            DateTime time3 = DateTime.Now;
            TimeSpan timeSpan = time3 - stopTime;
            form2.SetTime(format_date(timeSpan));
        }
        private void showTextBoxTimer_Tick(object sender, EventArgs e)
        {
            DateTime time3 = DateTime.Now;
            TimeSpan timeSpan = time3 - startTime;
            bool continueTask = true;
            if (accu_mode.Checked)
            {
                timeSpan += accumulateTime;
            }
            label2.Text = format_date(timeSpan);
            label1.Text = "间隔时间：";
            form2.SetTime(format_date(timeSpan));
            int tempRemindTime = Convert.ToInt32(TimeReminder.Text);
            if (time3.AddSeconds(2).Date != startTime.Date && form2.Mode.Equals("start"))
            {
                end_btn_Click(sender, e);
                if (useDatabase)
                {
                    connectToDb.Checked = false;
                    connectToDb.Checked = true;
                }
                Thread.Sleep(2000);
                timelist.Clear();
                btn_start_Click(sender, e);
            }
            if (TimeReminder.Text!= "" && !hasRemindCurrentTask)
            {
                if ((int)(time3 - startTime).TotalMinutes % remindTime == 0 && (int)(time3 - startTime).TotalMinutes > 1)
                {
                    hasRemindCurrentTask = true;
                    if (hideform1){
                        continueTask = form2.ShowTip();
                    }else{
                        DialogResult result = MessageBox.Show("可以喝杯水休息一下眼睛啦~是否继续呢？", "心态好最重要呀", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                        if (result == DialogResult.No)
                        {
                            continueTask = false;
                        }else
                        {
                            continueTask = true;
                        }
                    }
                    if(!continueTask)
                    {
                        end_btn_Click(sender, e);
                    } else{
                        remindTime = remindTime + tempRemindTime;
                        hasRemindCurrentTask = false;
                    }
                }
            }
        }
        private string format_date(DateTime time){
            return String.Format("{0:00}", time.Hour) + ":" + String.Format("{0:00}", time.Minute) + ":" + String.Format("{0:00}", time.Second);
        }
        private string format_date(TimeSpan time)
        {
            return String.Format("{0:00}", time.Hours) + ":" + String.Format("{0:00}", time.Minutes) + ":" + String.Format("{0:00}", time.Seconds);
        }
        public void end_btn_Click(object sender, EventArgs e)
        {
            hasRemindCurrentTask = true;
            showTextBoxTimer.Stop();
            showTextBoxTimer.Dispose();
            stopTime = DateTime.Now;
            btn_start.Enabled = true;
            reset.Enabled = true;
           
            TimeRecordObj time = addTimeRecord(startTime, stopTime, ThingsToDo.Text);
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].textBox == time.textBox)
                {
                    timelist[i].selected = true;
                }
                else
                {
                    timelist[i].selected = false;
                }
            }
            btn_start.Enabled = true;
            btn_start.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
            end_btn.Enabled = false;
            end_btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止2.png");
            reset.Enabled = true;
            reset.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\导入.png");
            output_btn.Enabled = true;
            output_btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\导出.png");
            calcHeight();
            form2.Mode = "stop";
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Tick += new EventHandler(showTextBoxTimer_Tick_stopMode);//到时所有执行的动作
            showTextBoxTimer.Start();//启动计时
            
            form2.SetWork(ConfigurationManager.AppSettings["motto"].ToString());
        }
        //加载数据
        private void LoadTimeInfo(bool reload)
        {
            string sqlText = "select currentIndex as CI,startTime,endTime,lastTime,note,type from mytime where createDate='" + DateTime.Today.ToShortDateString() + "' order by CI";
            LoadTime2DataGridView(sqlText,reload);
        }

        private void LoadTime2DataGridView(string sqlText,bool reload, params SqlParameter[] parameters)
        {
            
            if (useDatabase)
            {
                dt = SqlHelper.ExecuteDataTable(sqlText, parameters);
            }
            else
            {
                if (firstLoad && dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("CI");
                    dt.Columns.Add("startTime");
                    dt.Columns.Add("endTime");
                    dt.Columns.Add("lastTime");
                    dt.Columns.Add("note");
                    dt.Columns.Add("type");
                }
            }
            if (firstLoad || reload)
            {
                firstLoad = false;
                inResetProgress = true;
                timelist.Clear();
                panel1.Controls.Clear();
                panel1.Controls.Add(endLabel);
                foreach (DataRow row in dt.Rows)
                {
                    TimeRecordObj timeObj = new TimeRecordObj();
                    timeObj.mStartTime = DateTime.Parse(row["startTime"].ToString().Trim());
                    timeObj.mEndTime = DateTime.Parse(row["endTime"].ToString().Trim());
                    timeObj.interval = timeObj.mEndTime - timeObj.mStartTime;
                    timeObj.comment = row["note"].ToString().Trim();
                    timeObj.index = int.Parse(row["CI"].ToString());
                    CI = timeObj.index;
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
                    timeObj.textBox.Leave += textBoxChanged;
                    panel1.Controls.Add(description);
                    panel1.Controls.Add(timeObj.textBox);
                    timelist.Add(timeObj);
                }
                calcHeight();
                
            }
            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            dataGridView1.Columns["CI"].Width = 40;
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft YaHei", 9); 
            inResetProgress = false;
            showWorkItem();
        }

        private void setCurrentTextBoxObj(object sender, EventArgs e)
        {
            for (int i =0; i<timelist.Count; i++){
                if (timelist[i].textBox == sender){
                    timelist[i].selected = true;
                }else{
                    timelist[i].selected = false;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            end_btn.Enabled = false;
            end_btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止2.png");
            rest_btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            waste_btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            work_btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            study_btn.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            panel1.Controls.Add(endLabel);
            if (System.IO.File.Exists("c:\\temp\\time_record.txt"))
            {
                FileInfo fi = new FileInfo("c:\\temp\\time_record.txt");
                if (fi.CreationTime.Date == DateTime.Today)
                {
                    reset_Click(sender, e);
                }
                else
                {
                    deleteFile();
                }
            }
            form2.Mode = "stop";
            firstLoad = true;
            panel3.Controls.Add(memo);
            memo.Hide();
            LoadTimeInfo(false);
            showWorkItem();
            memo_label.Text = ConfigurationManager.AppSettings["motto"].ToString();
            typeCombo.Items.Add("none");
            typeCombo.Items.Add("work");
            typeCombo.Items.Add("rest");
            typeCombo.Items.Add("waste");
            typeCombo.Items.Add("study");
            dataGridView1.Controls.Add(typeCombo);
            typeCombo.Visible = false;
            typeCombo.SelectedIndexChanged += typeCombo_SelectedIndexChanged;
            typeCombo.LostFocus += typeCombo_LostFocus;
            work_btn.Parent = panel6;
            rest_btn.Parent = panel6;
            waste_btn.Parent = panel6;
            study_btn.Parent = panel6;
            panel1.Parent = panel7;
            panel6.Parent = panel7;
            chartPanel.Hide();
            panel1.Show();
            panel6.Show();
            remindTime = int.Parse(ConfigurationManager.AppSettings["remindTime"].ToString());
            TimeReminder.Text = remindTime.ToString();
            accu_mode.Checked = bool.Parse(ConfigurationManager.AppSettings["defaultAccuMode"].ToString());
            addFocusEvent(panel1);
            addFocusEvent(panel2);
            addFocusEvent(panel3);
            addFocusEvent(panel4);
            addFocusEvent(panel5);
            addFocusEvent(panel6);
        }

        private void typeCombo_LostFocus(object sender, EventArgs e)
        {
            typeCombo.Hide();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            inResetProgress = true;
            string comment, timeType = "";
            startTime = DateTime.Parse("1994-11-11");
            stopTime = DateTime.Parse("1994-11-11");
            TimeRecordObj lastObj = new TimeRecordObj();
            
            string text = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["importDirectory"].ToString() + "\\time_record.txt");
            
            string[] lines = text.Split(new char[2] { '\r', '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("开始时间"))
                {
                    startTime = DateTime.Parse(lines[i].Substring(5, 8));
                }
                if (lines[i].StartsWith("结束时间"))
                {
                    stopTime = DateTime.Parse(lines[i].Substring(5, 8));
                }
                
                if (lines[i].StartsWith("类型"))
                {
                    timeType = lines[i].Substring(3);
                }
                if (lines[i].StartsWith("备注"))
                {
                    comment = lines[i].Substring(3);
                    lastObj = addTimeRecord(startTime, stopTime, comment, timeType);
                    if (lastObj != null)
                    {
                        Label description = new Label();
                        description.BackColor = Color.Transparent;
                        lastObj.rightLabel = description;
                        lastObj.textBox.Text = format_date(lastObj.mEndTime - lastObj.mStartTime) + " " + lastObj.comment;
                        panel1.Controls.Add(lastObj.textBox);
                        panel1.Controls.Add(description);
                    }
                }
            }
            panel1.Controls.Add(endLabel);
            calcHeight();
            inResetProgress = false;
        }
        private TimeRecordObj addTimeRecord(DateTime startTime, DateTime stopTime,string comment = "", string type=""){
            if (startTime == stopTime)
            {
                return null;
            }
            if (useDatabase)
            {
                object type2 = SqlHelper.ExecuteScalar("select type from mytime where createDate='" + DateTime.Today.ToShortDateString() + "' and note='" + comment + "' and type!=''");
                if(type == "" && type2!=null)
                    type = type2.ToString();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@currentIndex", ++CI));
                parameters.Add(new SqlParameter("@startTime", format_date(startTime)));
                parameters.Add(new SqlParameter("@endTime", format_date(stopTime)));
                parameters.Add(new SqlParameter("@lastTime", format_date(stopTime - startTime)));
                parameters.Add(new SqlParameter("@createDate", DateTime.Today));
                parameters.Add(new SqlParameter("@note", comment));
                parameters.Add(new SqlParameter("@type", type));
                int result = SqlHelper.ExecuteNoQuery("insert into mytime(currentIndex, startTime, endTime, lastTime,createDate,note,type) values(@currentIndex, @startTime,@endTime,@lastTime,@createDate,@note,@type)", parameters.ToArray());
            }
            else
            {
                if(dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("CI");
                    dt.Columns.Add("startTime");
                    dt.Columns.Add("endTime");
                    dt.Columns.Add("lastTime");
                    dt.Columns.Add("note");
                    dt.Columns.Add("type");
                }
                if (dt.Select("startTime = '" + format_date(startTime) + "' and endTime = '" + format_date(stopTime) + "'").Count() ==0)
                {
                    dt.Rows.Add(new object[] {  ++CI,
                    format_date(startTime),
                    format_date(stopTime),
                    format_date(stopTime - startTime),
                    comment,
                    type});
                }
                else
                {
                    return null;
                }
            }
            TimeRecordObj time = new TimeRecordObj(startTime, stopTime);
            time.interval = stopTime - startTime;
            TextBox textbox = new TextBox();
            textbox.Location = new Point(50, 10);
            textbox.Multiline = true;
            time.index = CI;
            time.textBox = textbox;
            time.textBox.Click += setCurrentTextBoxObj;
            time.textBox.Leave += textBoxChanged;
            time.comment = comment;
            Label description = new Label();
            description.BackColor = Color.Transparent;
            time.rightLabel = description;
            panel1.Controls.Add(time.textBox);
            panel1.Controls.Add(description);
            timelist.Add(time);
            LoadTimeInfo(true);
            AddEndTime.Text = "";
            AddStartTime.Text = "";
            return time;
        }
        private void textBoxChanged(object sender, EventArgs e)
        {
            if (!inResetProgress)
            {
                for (int i = 0; i < timelist.Count; i++)
                {
                    if (timelist[i].textBox == sender)
                    {
                        if (((TextBox)sender).Text.Length > 8)
                        {
                            string oldComment = timelist[i].comment;
                            if(oldComment != ((TextBox)sender).Text.Substring(8).TrimStart()){
                                timelist[i].comment = ((TextBox)sender).Text.Substring(8).TrimStart();
                                if (useDatabase)
                                {
                                    string strcomm = "update mytime set note='" + timelist[i].comment + "' where currentIndex = " + timelist[i].index.ToString() + " and createDate='" + DateTime.Today.ToShortDateString() + "'";
                                    //update FilTer set 列名 = value where id = 3
                                    SqlHelper.ExecuteNoQuery(strcomm);
                                }
                                else
                                {
                                    DataRow[] arrRows = dt.Select("CI='" + timelist[i].index + "'");
                                    if (arrRows.Length > 0)
                                        arrRows[0]["note"] = timelist[i].comment;
                                }
                                LoadTimeInfo(false);
                            }
                        }
                    }
                }
            }
            
        }

        private void calcHeight(){
            TimeSpan total = new TimeSpan();
            int i = 0;
            double height;
            double startPoint = 10;
            TimeSpan minTime = new TimeSpan(24,0,0);
            double totalHeight = 0;
            //按开始时间对timelist进行排序
            IComparer<TimeRecordObj> comparer = new TimeRecordObj();
            timelist.Sort(comparer);
            for (i = 0; i < timelist.Count; i++){
                timelist[i].interval = timelist[i].mEndTime - timelist[i].mStartTime;
                //记录所有的间隔时间总和
                total += timelist[i].interval;
                if (timelist[i].interval < minTime){
                    //记录最小的timelist的间隔
                    minTime = timelist[i].interval;
                }
            }
            //计算按最小间隔为30，来计算时间总高度
            totalHeight = total.TotalMilliseconds / minTime.TotalMilliseconds * 30;
            //如果时间总高度大于窗口高度，则设置时间总高度为窗口高度
            if(totalHeight > panel1.Height - 30){
                totalHeight = panel1.Height - 30;
            }
            for (i = 0; i < timelist.Count; i++)
            {
                timelist[i].rightLabel.Text = format_date(timelist[i].mStartTime);
                timelist[i].rightLabel.BackColor = Color.Transparent;
                timelist[i].rightLabel.Location = new Point(260, Convert.ToInt32(startPoint));
                for (int j = i - 1; j >= 0; j--)
                {
                    if (timelist[j].rightLabel.Visible)
                    {
                       if( timelist[i].rightLabel.Location.Y - timelist[j].rightLabel.Location.Y < 20)
                        {
                            timelist[i].rightLabel.Visible = false;
                        }
                        break;
                    }
                }
                timelist[i].textBox.Location= new Point(50, Convert.ToInt32(startPoint));
                timelist[i].textBox.Width = 200;
                timelist[i].textBox.Text = format_date(timelist[i].interval) + " " + timelist[i].comment;
                height = Math.Round(timelist[i].interval.TotalMilliseconds / total.TotalMilliseconds * totalHeight, 2);
                timelist[i].textBox.Height = Convert.ToInt32(height);
                startPoint = startPoint + Convert.ToInt32(height);
                if(i == timelist.Count - 1){
                    endLabel.Location = new Point(260, Convert.ToInt32(startPoint)-5);
                    endLabel.BackColor = Color.Transparent;
                    endLabel.Text = format_date(timelist[i].mEndTime);
                    endLabel.Visible = true;
                }
            }
        }
        private TimeRecordObj selectedTimeObj(){
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].selected){
                    return timelist[i];
                }
            }
            return null;
        }
        private void update_timetype(string timetype, int index,string comment){
            if (useDatabase)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@type", timetype));
                //SqlHelper.ExecuteNoQuery("update mytime set type=@type where currentIndex='" + index.ToString() + "' and createDate='" + DateTime.Today.ToShortDateString() + "'", parameters.ToArray());
                SqlHelper.ExecuteNoQuery("update mytime set type=@type where note='" + comment + "' and createDate='" + DateTime.Today.ToShortDateString() + "'", parameters.ToArray());
            }
            else
            {
                DataRow[] arrRows = dt.Select("CI='" + index.ToString() + "'");
                arrRows[0]["type"] = timetype;
            }
            LoadTimeInfo(true);
        }
        private void waste_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("waste", currentObj.index,currentObj.comment);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.waste;
                currentObj.textBox.BackColor = Color.FromArgb(255, 128, 128);
            }
            else{
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void work_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("work", currentObj.index, currentObj.comment);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.work;
                currentObj.textBox.BackColor = Color.FromArgb(255, 242, 0);
                showWorkItem();
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void rest_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("rest", currentObj.index, currentObj.comment);
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

        private void summary_Click(object sender, EventArgs e)
        {
            TimeSpan waste = new TimeSpan(0, 0, 0);
            TimeSpan work = new TimeSpan(0, 0, 0);
            TimeSpan rest = new TimeSpan(0, 0, 0);
            TimeSpan study = new TimeSpan(0, 0, 0);
            TimeSpan none = new TimeSpan(0, 0, 0);
            
            int i = 0;
            for (i = 0; i<timelist.Count; i++){
                switch(timelist[i].timeType){
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
            foreach (TimeRecordObj obj in timelist){
                if (obj.comment.Equals("")){
                    obj.comment = obj.timeType.ToString();
                }
                if(sum.ContainsKey(obj.comment)){
                    sum[obj.comment] = sum[obj.comment] + obj.interval;
                }else{
                    sum.Add(obj.comment, obj.interval);
                    sumType.Add(obj.comment, obj.timeType.ToString());
                }
            }
            List<string> xData = new List<string>() ;
            List<double> yData = new List<double>() ;
            foreach (string key in sum.Keys)
            {
                xData.Add(key + " " + format_date(sum[key]));
                yData.Add(sum[key].TotalSeconds);
            }
            chart1.Series[0]["PieLabelStyle"] = "Outside";//将文字移到外侧
            chart1.Series[0]["PieLineColor"] = "Black";//绘制黑色的连线。
            chart1.Series[0].Points.DataBindXY(xData, yData);
            List<string> xData2 = new List<string>() {"study", "work", "rest", "waste"};
            List<double> yData2 = new List<double>() {study.TotalSeconds, work.TotalSeconds, rest.TotalSeconds, waste.TotalSeconds};
            chart2.Series[0].Points.DataBindXY(xData2, yData2);
            chart2.Series[0].Points[0].Color = Color.FromArgb(255, 195, 205);
            chart2.Series[0].Points[0].Label = format_date(study);
            chart2.Series[0].Points[1].Color = Color.FromArgb(255, 242, 0);
            chart2.Series[0].Points[1].Label = format_date(work);
            chart2.Series[0].Points[2].Color = Color.FromArgb(144, 238, 144);
            chart2.Series[0].Points[2].Label = format_date(rest);
            chart2.Series[0].Points[3].Color = Color.FromArgb(255, 106, 106);
            chart2.Series[0].Points[3].Label = format_date(waste);
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
                }else if(sumType[key] == TimeRecordObj.TimeType.study.ToString())
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
                    if(waste_item < 21)
                        waste_item++;
                }
                else
                {
                    chart1.Series[0].Points[i].Color = Color.FromArgb(248 - none_item * 5, 248 - none_item * 5, 255-none_item*5);
                    none_item++;
                }
                i++;
            }
            panel1.Hide();
            panel6.Hide();
            chartPanel.Show();
        }

        private void output_btn_Click(object sender, EventArgs e)
        {
            outputText();
        }
        private void outputText(bool showMessage = true)
        {
            deleteFile();
            for (int i = 0; i < timelist.Count; i++)
            {
                addText("开始时间：" + format_date(timelist[i].mStartTime));
                if (timelist[i].mEndTime == DateTime.Parse("1994-11-11"))
                {
                    timelist[i].mEndTime = DateTime.Now;
                    timelist[i].interval = timelist[i].mEndTime - timelist[i].mStartTime;
                }
                addText("间隔时间：" + format_date(timelist[i].interval));
                addText("结束时间：" + format_date(timelist[i].mEndTime));
                addText("类型：" + timelist[i].timeType);
                addText("备注：" + timelist[i].comment);
                addText("");
            }
            if (showMessage)
                MessageBox.Show("导出成功！");
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            form2.SetWork(ThingsToDo.Text);
            form2.Show();
            this.Hide();
            hideform1 = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string strcolumn = dataGridView1.Columns[e.ColumnIndex].HeaderText;//获取列标题
            string strrow = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();//获取焦点触发行的第一个值
            string value = dataGridView1.CurrentCell.Value.ToString();//获取当前点击的活动单元格的值
            TimeRecordObj currentObj = null;
            if (useDatabase)
            {
                string strcomm = "update mytime set " + strcolumn + "='" + value + "'where currentIndex = " + strrow + " and createDate='" + DateTime.Today.ToShortDateString() + "'";
                //update FilTer set 列名 = value where id = 3
                SqlHelper.ExecuteNoQuery(strcomm);
            }
            else
            {
                DataRow[] arrRows = dt.Select("CI='" + strrow+"'");
                arrRows[0][strcolumn] = value;
            }
            foreach(TimeRecordObj tro in timelist){
                if (tro.index.ToString() == strrow){
                    currentObj = tro;
                    break;
                }
            }
            if(strcolumn.Equals("startTime")){
                currentObj.mStartTime = DateTime.Parse(value);
            }
            else if (strcolumn.Equals("startTime"))
            {
                currentObj.mEndTime = DateTime.Parse(value);
            }
            else if (strcolumn.Equals("note"))
            {
                currentObj.comment = value;
            }
            else if (strcolumn.Equals("type"))
            {
                switch (value.Trim())
                {
                    case "study":
                        currentObj.timeType = TimeRecordObj.TimeType.study;
                        currentObj.textBox.BackColor = Color.FromArgb(255, 174, 201);
                        break;
                    case "work":
                        currentObj.timeType = TimeRecordObj.TimeType.study;
                        currentObj.textBox.BackColor = Color.FromArgb(255, 242, 0);
                        break;
                    case "rest":
                        currentObj.timeType = TimeRecordObj.TimeType.rest;
                        currentObj.textBox.BackColor = Color.FromArgb(192, 255, 192);
                        break;
                    case "waste":
                        currentObj.timeType = TimeRecordObj.TimeType.waste;
                        currentObj.textBox.BackColor = Color.FromArgb(255, 128, 128);
                        break;
                    default:
                        break;
                }

            }
            calcHeight();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            calcHeight();
            chart1.Height = (this.Height - chart1.Location.Y*4)/2;
            //chart2.Location = new Point(chart1.Location.X-10, Convert.ToInt32(chart1.Location.Y + chart1.Height + chart1.Location.Y));
            //chart2.Height = chart1.Height-40;
            //chart2.Width = chart1.Width-40;
            //chart2.Visible = true;
        }

        private void ConnectToDb_CheckedChanged(object sender, EventArgs e)
        {
            useDatabase = connectToDb.Checked;
            if (useDatabase)
            {
                try
                {
                    SqlConnection con = new SqlConnection(SqlHelper.GetConnectionString());
                    con.Open();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法连接数据库：" + ex.ToString());
                    useDatabase = false;
                    connectToDb.Checked = false;
                }
                if (useDatabase)
                {
                    firstLoad = true;
                    timelist.Clear();
                    LoadTimeInfo(false);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定退出吗?", "Tip", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (result == DialogResult.OK)
            {
                //if (!useDatabase)
                //{
                //    outputText(false);
                //}
            }
            else
            {
                e.Cancel = true;
            }
           
        }

        

       //private void removeNonexistentWorkItems(){
       //     Dictionary<string, bool> workContentsTemp = new Dictionary<string, bool>();
       //     for (int i = 0; i < timelist.Count; i++)
       //     {
       //         if (timelist[i].timeType == TimeRecordObj.TimeType.work || timelist[i].timeType == TimeRecordObj.TimeType.study)
       //         {
       //             if (!workContentsTemp.Keys.Contains(timelist[i].comment))
       //             {
       //                 workContentsTemp.Add(timelist[i].comment, false);
       //             }
       //         }
       //     }
       //     string[] workitems = workContents.Keys.ToArray<String>();
       //     foreach (string name in workitems)
       //     {
       //         if (!workContentsTemp.ContainsKey(name))
       //         {
       //             workContents.Remove(name);
       //         }
       //     }
       //     sortTodayList();
       // }

        private void showWorkItem()
        {
            for (int i = 0; i < timelist.Count; i++)
            {
                if (timelist[i].timeType == TimeRecordObj.TimeType.work || timelist[i].timeType == TimeRecordObj.TimeType.study)
                {
                    if (!workContents.Keys.Contains(timelist[i].comment))
                    {
                        addWorkItem(timelist[i].comment,false);
                    }
                }
            }
            sortTodayList();
           
        }

        private void workItemTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addWorkItem(workItemTextBox.Text,false);
                sortTodayList();
                workItemTextBox.Text = "";
                e.Handled = true;
                e.SuppressKeyPress = true;

            }
        }

        private void addWorkItem(string itemContent, bool completeStatus)
        {
            const int workItemHeight = 45;
            if (itemContent != "")
            {
                int index = todayList.Controls.OfType<Label>().Count();
                Label label = new Label();
                label.Text = itemContent;
                label.Width = todayList.Width;
                label.Height = 30;
                label.Font = new Font("Microsoft YaHei", 11);
                label.Location = new Point(25, 5 + index * workItemHeight);
                label.Click += Label_Click;
                label.DoubleClick += label_doubleClick;
                label.MouseDown += label_MouseDown;
                label.Name = index.ToString();
                CheckBox cb = new CheckBox();
                cb.Location = new Point(3, 9 + index * workItemHeight);
                cb.Text = "";
                cb.Width = 20;
                cb.Name = index.ToString();
                if (completeStatus)
                {
                    cb.Checked = true;
                    label.Font = new Font("Microsoft YaHei", 11, FontStyle.Strikeout);
                    label.ForeColor = Color.Gray;
                }
                else
                {
                    cb.Checked = false;
                    label.Font = new Font("Microsoft YaHei", 11, FontStyle.Regular);
                    label.ForeColor = Color.Black;
                }
                cb.CheckedChanged += cb_checkStateChanged;


                TextBox split = new TextBox();
                split.Location = new Point(4, 40 + index * workItemHeight);
                split.Multiline = true;
                split.Width = panel4.Width-3;
                split.Height = 1;
                split.BackColor = Color.Black;
                split.Name = index.ToString();
                
                todayList.Controls.Add(cb);
                todayList.Controls.Add(label);
                todayList.Controls.Add(split);
                if (!workContents.ContainsKey(itemContent))
                    workContents.Add(itemContent,false);
              
            }
        }

        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right){
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add("删除");
                menu.MenuItems.Add("编辑");
                menu.Show((Label)sender, new Point(e.X, e.Y));
                menu.MenuItems[0].Click += delegate
                {
                    workContents.Remove(((Label)sender).Text);
                    sortTodayList();
                };
                menu.MenuItems[1].Click += delegate
                {
                    label_doubleClick(sender, e);
                };
            }
        }

        private void cb_checkStateChanged(object sender, EventArgs e)
        {
            for(int i=0; i<todayList.Controls.Count;i++){
                if(todayList.Controls[i].GetType()==typeof(Label))
                {
                    if(((Label)todayList.Controls[i]).Name == ((CheckBox)sender).Name){
                        if(((CheckBox)sender).Checked){
                          
                            workContents[((Label)todayList.Controls[i]).Text] = true;
                        }
                        else{
                           
                            workContents[((Label)todayList.Controls[i]).Text] = false;
                        }
                        break;
                    }
                }
            }
            sortTodayList();
        }
        private void sortTodayList(){
            todayList.Controls.Clear();
            foreach (string name in workContents.Keys)
            {
                if (!workContents[name])
                {
                    addWorkItem(name, false);
                }
            }
            foreach (string name in workContents.Keys)
            {
                if (workContents[name])
                {
                    addWorkItem(name, true);
                }
            }
        }
        private void label_doubleClick(object sender, EventArgs e)
        {
            BaseTextBox text = new BaseTextBox();
            text.Location = ((Label)sender).Location;
            text.Size = ((Label)sender).Size;
            text.Width = text.Width + 10;
            text.KeyDown += workitem_keydown;
            text.Leave += workitem_leave;
            text.Name = ((Label)sender).Name;
            text.Text = ((Label)sender).Text;
            text.Font = new System.Drawing.Font("微软雅黑", 11F); ;
            ((Label)sender).Hide();
            todayList.Controls.Add(text);
            text.Focus();
        }
        private void update_workItem(object sender){
            TextBox text = (TextBox)sender;
            Control[] controls = todayList.Controls.Find(text.Name, true);
            bool remove_flag = false;
            if (text.Text == ""){
                remove_flag = true;
            }
            foreach (Control control in controls)
            {
                if (control.GetType() == typeof(Label))
                {
                    workContents.Remove(control.Text);
                }
                if (remove_flag){
                    todayList.Controls.Remove(control);
                }
                else{
                    if (control.GetType() == typeof(Label))
                    {
                        control.Text = text.Text;
                        workContents.Add(control.Text,false);
                        control.Show();
                        text.Hide();
                    }
                }
            }
        }
        private void workitem_leave(object sender, EventArgs e)
        {
            update_workItem(sender);
        }

        private void workitem_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter){
                update_workItem(sender);
                e.Handled = true;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            copiedFromList = true;
            ThingsToDo.Text = ((Label)sender).Text;
        }

        private void Add_btn_Click(object sender, EventArgs e)
        {
            if(AddEndTime.Text!="" && AddStartTime.Text != "")
            {
                DateTime addStartTime, addEndTime;
                int j = -1;
                bool sameStart = false, sameEnd = false;
                try
                {
                    addStartTime = Convert.ToDateTime(AddStartTime.Text);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("开始时间格式不对哦", "Tip",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    addEndTime = Convert.ToDateTime(AddEndTime.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("结束时间格式不对哦", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //2. 开始时间在某个时间段内
                for (int i = 0; i < timelist.Count; i++)
                {
                    if (addStartTime.Ticks/10000000 >= timelist[i].mStartTime.Ticks / 10000000 & addEndTime.Ticks / 10000000 <= timelist[i].mEndTime.Ticks / 10000000)
                    {
                        j = i;
                        if (addStartTime == timelist[i].mStartTime) sameStart = true;
                        if (addEndTime == timelist[i].mEndTime) sameEnd = true;
                    }
                }
                if (j != -1)
                {
                    if (!sameStart)
                        addTimeRecord(timelist[j].mStartTime, addStartTime);
                    if (!sameEnd)
                        addTimeRecord(addEndTime, timelist[j].mEndTime);
                    addTimeRecord(addStartTime, addEndTime);
                    DeleteTimeRecord(j);
                }
                else
                {
                    
                    if (timelist.Count >= 1)
                    {
                        //1. 结束时间小于第一个开始时间
                        if (addEndTime.Ticks / 10000000 <= timelist[0].mStartTime.Ticks / 10000000)
                        {
                            addTimeRecord(addStartTime, addEndTime);
                            if (!(addEndTime == timelist[0].mStartTime))
                                addTimeRecord(addEndTime, timelist[0].mStartTime);
                        }else if(addStartTime.Ticks / 10000000 >= timelist[timelist.Count - 1].mEndTime.Ticks / 10000000) //2. 开始时间大于最后一个结束时间
                        {
                            if (form2.Mode == "start")
                            {
                                if(addEndTime.Ticks / 10000000 <= startTime.Ticks / 10000000)
                                {
                                    addTimeRecord(addStartTime, addEndTime);
                                }
                            }
                            else
                            {
                                if (addEndTime.Ticks / 10000000 <= DateTime.Now.Ticks / 10000000)
                                {
                                    addTimeRecord(addStartTime, addEndTime);
                                }
                            }
                        }
                    }
                    else //3. timelist列表为空时，直接添加
                    {
                        addTimeRecord(addStartTime, addEndTime);
                        
                    }
                }
                calcHeight();
                
            }
        }
        public void DeleteTimeRecord(int index)
        {
            if (!useDatabase)
            {
                DataRow[] dr = dt.Select("startTime='" + format_date(timelist[index].mStartTime) + "' and endTime='" + format_date(timelist[index].mEndTime) + "'");
                if (dr != null && dr.Count() > 0)
                {
                    foreach (DataRow row in dr)
                    {
                        dt.Rows.Remove(row);
                    }
                }
            }
            else
            {
                string sqlText = "delete from mytime where currentIndex = '" + timelist[index].index + "' and createDate = '" + DateTime.Today.ToShortDateString() + "'";
                SqlHelper.ExecuteNoQuery(sqlText);
            }
            panel1.Controls.Remove(timelist[index].textBox);
            timelist.RemoveAt(index);
            LoadTimeInfo(false);
            calcHeight();
        }
        private void Large_btn_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(timelist);
            form3.Show();
        }

       

        private void Memo_label_Click(object sender, EventArgs e)
        {
            memo.Location = memo_label.Location;
            memo.Leave += memo_leave;
            memo.KeyDown += memo_keydown;
            memo.Size = memo_label.Size;
            memo.Width = memo.Width + 20;
            if(memo.Width < 200)
            {
                memo.Width = 200;
            }
            memo.Text = memo_label.Text;
            memo.Font = memo_label.Font;
            memo.LostFocus += Memo_LostFocus;
            memo_label.Hide();
            memo.Show();
            memo.Focus();
        }
        private void Memo_LostFocus(object sender, EventArgs e)
        {
            update_memo();
        }
        private void memo_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter){
                update_memo();
                e.Handled = true;
            }
        }
        private void update_memo()
        {
            memo.Hide();
            memo_label.Text = memo.Text;
            memo_label.Show();
            Utils.UpdateAppConfig("motto", memo.Text);
        }
        private void memo_leave(object sender, EventArgs e)
        {
            update_memo();
        }

        private void DataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                DataGridViewColumn column = dataGridView1.CurrentCell.OwningColumn;
                if (column.Name.Equals("type"))
                {
                    int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    Rectangle rect = dataGridView1.GetCellDisplayRectangle(columnIndex, rowIndex, false);
                    typeCombo.Left = rect.Left;
                    typeCombo.Top = rect.Top;
                    typeCombo.Size = rect.Size;
                    typeCombo.Font = new Font("Microsoft YaHei", 9);
                    string consultingRoom = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
                    int index = typeCombo.Items.IndexOf(consultingRoom);
                    typeCombo.Text = consultingRoom;
                    typeCombo.SelectedIndex = index;
                    typeCombo.Visible = true;
                }
                else
                {
                    typeCombo.Visible = false;
                }
            }
            
        }
        private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (typeCombo.SelectedItem!=null)
                dataGridView1.CurrentCell.Value = typeCombo.SelectedItem.ToString();
        }


        private void return_btn_Click(object sender, EventArgs e)
        {
            chartPanel.Hide();
            panel1.Show();
            panel6.Show();
        }

        private void study_btn_Click(object sender, EventArgs e)
        {
            TimeRecordObj currentObj = selectedTimeObj();
            update_timetype("study", currentObj.index, currentObj.comment);
            if (currentObj != null)
            {
                currentObj.timeType = TimeRecordObj.TimeType.study;
                currentObj.textBox.BackColor = Color.FromArgb(255, 174, 201);
                showWorkItem();
            }
            else
            {
                MessageBox.Show("请选择一个时间区域");
            }
        }

        private void setupBtn_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(this);
            form4.Show();
        }

        private void accu_mode_CheckedChanged(object sender, EventArgs e)
        {
            Utils.UpdateAppConfig("defaultAccuMode", accu_mode.Checked.ToString());
        }

        private void TimeReminder_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(TimeReminder.Text, out remindTime))
            {
                MessageBox.Show("请输入数字", "注意", MessageBoxButtons.OK);
                TimeReminder.Focus();
            }
            else
            {
                remindTime = int.Parse(TimeReminder.Text);
                Utils.UpdateAppConfig("remindTime", TimeReminder.Text);
            }
        }
        private void ThingsToDo_Enter(object sender, EventArgs e)
        {
            if (useDatabase)
                setAutoCompleteSource();
        }
        private void setAutoCompleteSource()
        {
            var source = new AutoCompleteStringCollection();
            string[] rangeString = new string[5];
            int count = 0;
            SqlDataReader noteReader = null;
            
                 noteReader = SqlHelper.ExecutedReader("SELECT top(5) note from mytime where " +
                "(type = 'work' or type = 'study' or type = '') and note != '' and " +
                "note like '%" + ThingsToDo.Text + "%'" +
                "group by note " +
                "order by Max(mytime.createDate) desc, Max(mytime.startTime) desc");
           
            
            while (noteReader.HasRows && count < 5 && noteReader.Read())
            {
                rangeString[count] = (noteReader.IsDBNull(0) ? null : noteReader.GetString(0));
                count++;
            }
            source.AddRange(rangeString);
            ThingsToDo.AutoCompleteCustomSource = source;
            listBox1.Items.Clear();
            if(ThingsToDo.AutoCompleteCustomSource.Count>0){
                foreach (String s in ThingsToDo.AutoCompleteCustomSource)
                {
                    if(s!=null){
                        listBox1.Items.Add(s);
                    }
                    listBox1.Visible = true;
                  
                }
            }
            ThingsToDo.Focus();
        }
        private void ThingsToDo_TextChanged(object sender, EventArgs e)
        {
            if(copiedFromList){
                copiedFromList = false;
                hideResults();
                return;
            }
            if(useDatabase)
                setAutoCompleteSource();
        }
        void hideResults()
        {
            listBox1.Visible = false;
        }

     

        private void listBox1_Leave(object sender, EventArgs e)
        {
            hideResults();
        }

        

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==((char)Keys.Enter)){
                ThingsToDo.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                hideResults();
            }
        }

        

        private void ThingsToDo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                listBox1.Focus();
                listBox1.SelectedIndex = 0;
            }
        }

        private void ThingsToDo_Leave(object sender, EventArgs e)
        {
            if (!listBox1.Focused)
            {
                hideResults();
            }
        }

       

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listBox1.SelectedItem!=null)
                ThingsToDo.Text = listBox1.SelectedItem.ToString();
            hideResults();
        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = new Point((Size)e.Location);
            int currentIndex = listBox1.IndexFromPoint(p);
            listBox1.SelectedIndex = currentIndex;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem != null)
                ThingsToDo.Text = listBox1.SelectedItem.ToString();
            hideResults();
        }

        private void panel4_SizeChanged(object sender, EventArgs e)
        {
            for (int i = 0; i< todayList.Controls.Count; i++)
            {
                if (todayList.Controls[i].GetType()==typeof(TextBox)&& todayList.Controls[i].Height==1)
                {
                    todayList.Controls[i].Width = panel4.Width-3;
                }
            }
            workItemTextBox.Width = panel4.Width-3;
        }

        private void summary_btn_Click(object sender, EventArgs e)
        {
            
        }
    }
}
