using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeRecorder
{
    public partial class Form4 : Form
    {
        Form1 Form1;
        public Form4(Form1 form1)
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            Form1 = form1;
            this.TopMost = true;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            importDirectory.Text = ConfigurationManager.AppSettings["importDirectory"].ToString();
            outputDirectory.Text = ConfigurationManager.AppSettings["outputDirectory"].ToString();
            restbox.Text = ConfigurationManager.AppSettings["defaultRest"].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(importDirectory.Text)){
                MessageBox.Show("导入文件夹不存在！");
                return;
            }else{
                Utils.UpdateAppConfig("importDirectory", importDirectory.Text);
            }
            if (!Directory.Exists(outputDirectory.Text))
            {
                MessageBox.Show("导出文件夹不存在！");
                return;
            }
            else
            {
                Utils.UpdateAppConfig("outputDirectory", outputDirectory.Text);
            }
            if (restbox.Text.Equals(""))
            {
                Utils.UpdateAppConfig("defaultRest", "休息");
            }
            else
            {
                Utils.UpdateAppConfig("defaultRest", restbox.Text);
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定删除吗?", "注意", MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes){
                if(Form1.useDatabase){
                    SqlHelper.ExecuteNoQuery("delete from mytime where mytime.createDate = CAST(GETDATE() AS DATE)");
                    Form1.timelist.Clear();
                    Form1.panel1.Controls.Clear();
                }
                else
                {
                    Form1.dt.Rows.Clear();
                    Form1.timelist.Clear();
                    Form1.panel1.Controls.Clear();
                }
                Form1.endLabel.Text = "";
            }
        }

       
    }
}
