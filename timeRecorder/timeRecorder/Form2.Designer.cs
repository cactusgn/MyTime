namespace timeRecorder
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_control = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.min = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 37);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::timeRecorder.Properties.Resources.Image_1;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btn_control);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.min);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(262, 34);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(21, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Time";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Label1_MouseMove);
            // 
            // btn_control
            // 
            this.btn_control.BackColor = System.Drawing.Color.Transparent;
            this.btn_control.BackgroundImage = global::timeRecorder.Properties.Resources.开始2;
            this.btn_control.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_control.FlatAppearance.BorderSize = 0;
            this.btn_control.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_control.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_control.Location = new System.Drawing.Point(166, 5);
            this.btn_control.Name = "btn_control";
            this.btn_control.Size = new System.Drawing.Size(26, 25);
            this.btn_control.TabIndex = 2;
            this.btn_control.UseVisualStyleBackColor = false;
            this.btn_control.Click += new System.EventHandler(this.Btn_control_Click);
            this.btn_control.MouseLeave += new System.EventHandler(this.Btn_control_MouseLeave);
            this.btn_control.MouseHover += new System.EventHandler(this.Btn_control_MouseHover);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.BackgroundImage = global::timeRecorder.Properties.Resources.关闭;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(229, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 25);
            this.button2.TabIndex = 0;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button2_MouseDown);
            this.button2.MouseLeave += new System.EventHandler(this.Button2_MouseLeave);
            this.button2.MouseHover += new System.EventHandler(this.Button2_MouseHover);
            // 
            // min
            // 
            this.min.BackColor = System.Drawing.Color.Transparent;
            this.min.BackgroundImage = global::timeRecorder.Properties.Resources.最小化;
            this.min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.min.FlatAppearance.BorderSize = 0;
            this.min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.min.Location = new System.Drawing.Point(198, 6);
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(25, 25);
            this.min.TabIndex = 0;
            this.min.UseVisualStyleBackColor = false;
            this.min.Click += new System.EventHandler(this.Min_Click);
            this.min.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Min_MouseDown);
            this.min.MouseLeave += new System.EventHandler(this.Min_MouseLeave);
            this.min.MouseHover += new System.EventHandler(this.Min_MouseHover);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(96, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Work";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(0, 36);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(262, 32);
            this.panel3.TabIndex = 1;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(262, 67);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.VisibleChanged += new System.EventHandler(this.Form2_VisibleChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_control;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
    }
}