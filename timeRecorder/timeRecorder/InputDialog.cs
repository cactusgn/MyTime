using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timeRecorder
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
            //this.AcceptButton = this.button1;
            this.CancelButton = this.button2;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }
        public InputDialog(string label)
        {
            InitializeComponent();
            label1.Text = label;
        }
        public string Value { get; set; }
        public InputDialog(string label, string title)
        {
            InitializeComponent();
            label1.Text = label;
            this.Text = title;
        }

   
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Value = textBox1.Text == "" ? ConfigurationManager.AppSettings["defaultRest"].ToString() : textBox1.Text;
            this.Close();
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.Text = Value;
            
        }
        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Value = textBox1.Text == "" ? ConfigurationManager.AppSettings["defaultRest"].ToString() : textBox1.Text;
            this.Close();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.DialogResult = DialogResult.OK;
                Value = textBox1.Text == "" ? ConfigurationManager.AppSettings["defaultRest"].ToString() : textBox1.Text;
                this.Close();
            }
        }
    }
}
