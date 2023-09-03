using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeRecorder
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
        }
        private Form1 form1;
        public Form2(Form1 form)
        {
            InitializeComponent();
            form1 = form;
            this.TopMost = true;
        }
        private Point mPoint;
        private string mode;
        public string Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }
      

        public void SetTime(string time)
        {
            this.label1.Text = time;
        }
        public void SetWork(string work)
        {
            this.label2.Text = work;
            int labelLength = label2.Width;
            int form2Length = this.Width;
            label2.Location = new Point((form2Length - labelLength) / 2, 2);
        }
        public bool ShowTip(){
            DialogResult result = MessageBox.Show("可以喝杯水休息一下眼睛啦~是否继续呢？", "心态好最重要呀", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            if (result == DialogResult.No){
                mode = "stop";
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
                return false;
            }
            return true;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            if(mode == "start")
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止.png");
            }
            else
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
            }
            btn_control.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            min.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn_control.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button2.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            min.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            
        }
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            form1.Show();
            form1.hideform1 = false;
        }

        private void Btn_control_Click(object sender, EventArgs e)
        {
            if(mode == "start")
            {
                mode = "stop";
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
                form1.end_btn_Click(sender, e);
            }
            else
            {
                mode = "start";
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止.png");
                form1.btn_start_Click(sender, e);
            }
        }

        private void Label1_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void Label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        private void Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Btn_control_MouseHover(object sender, EventArgs e)
        {
            if(mode == "stop")
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始3.png");
            }
            else
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止3.png");
            }
        }

        private void Min_MouseHover(object sender, EventArgs e)
        {
            min.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\最小化2.png");
        }

        private void Button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\关闭2.png");
        }

        private void Button2_MouseDown(object sender, MouseEventArgs e)
        {
            button2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\关闭2.png");
        }

        private void Min_MouseDown(object sender, MouseEventArgs e)
        {
            min.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\最小化2.png");
        }

        private void Btn_control_MouseLeave(object sender, EventArgs e)
        {
            if (mode == "stop")
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
            }
            else
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止.png");
            }
        }

        private void Min_MouseLeave(object sender, EventArgs e)
        {
            min.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\最小化.png");
        }

        private void Button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\关闭.png");
        }
        

    

        private void Form2_VisibleChanged(object sender, EventArgs e)
        {
            if (mode == "start")
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\停止.png");
            }
            else
            {
                btn_control.BackgroundImage = Image.FromFile(Application.StartupPath + "\\images\\开始.png");
            }
        }
    }
}
