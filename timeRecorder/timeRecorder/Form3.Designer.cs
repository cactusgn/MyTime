namespace timeRecorder
{
    partial class Form3
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.refresh_btn = new System.Windows.Forms.Button();
            this.tgl_oneday = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.to_date = new System.Windows.Forms.DateTimePicker();
            this.from_date = new System.Windows.Forms.DateTimePicker();
            this.tgl_all = new System.Windows.Forms.CheckBox();
            this.tgl_rest = new System.Windows.Forms.CheckBox();
            this.tgl_waste = new System.Windows.Forms.CheckBox();
            this.tgl_work = new System.Windows.Forms.CheckBox();
            this.tgl_study = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.study_btn = new System.Windows.Forms.Button();
            this.work_btn = new System.Windows.Forms.Button();
            this.rest_btn = new System.Windows.Forms.Button();
            this.waste_btn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.PapayaWhip;
            this.chart1.BackSecondaryColor = System.Drawing.Color.MistyRose;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(9, 3);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(619, 550);
            this.chart1.TabIndex = 16;
            this.chart1.Text = "chart1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.refresh_btn);
            this.panel1.Controls.Add(this.tgl_oneday);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.to_date);
            this.panel1.Controls.Add(this.from_date);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1124, 82);
            this.panel1.TabIndex = 17;
            // 
            // refresh_btn
            // 
            this.refresh_btn.BackColor = System.Drawing.Color.PaleTurquoise;
            this.refresh_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh_btn.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.refresh_btn.Location = new System.Drawing.Point(669, 26);
            this.refresh_btn.Name = "refresh_btn";
            this.refresh_btn.Size = new System.Drawing.Size(98, 33);
            this.refresh_btn.TabIndex = 7;
            this.refresh_btn.Text = "refresh";
            this.refresh_btn.UseVisualStyleBackColor = false;
            this.refresh_btn.Click += new System.EventHandler(this.refresh_btn_Click);
            // 
            // tgl_oneday
            // 
            this.tgl_oneday.AutoSize = true;
            this.tgl_oneday.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_oneday.Location = new System.Drawing.Point(41, 28);
            this.tgl_oneday.Name = "tgl_oneday";
            this.tgl_oneday.Size = new System.Drawing.Size(72, 29);
            this.tgl_oneday.TabIndex = 5;
            this.tgl_oneday.Text = "单日";
            this.tgl_oneday.UseVisualStyleBackColor = true;
            this.tgl_oneday.CheckedChanged += new System.EventHandler(this.tgl_oneday_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(407, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(132, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "从";
            // 
            // to_date
            // 
            this.to_date.Location = new System.Drawing.Point(444, 29);
            this.to_date.Name = "to_date";
            this.to_date.Size = new System.Drawing.Size(200, 22);
            this.to_date.TabIndex = 1;
            // 
            // from_date
            // 
            this.from_date.Location = new System.Drawing.Point(169, 29);
            this.from_date.Name = "from_date";
            this.from_date.Size = new System.Drawing.Size(200, 22);
            this.from_date.TabIndex = 0;
            // 
            // tgl_all
            // 
            this.tgl_all.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgl_all.AutoSize = true;
            this.tgl_all.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_all.Location = new System.Drawing.Point(81, 26);
            this.tgl_all.Name = "tgl_all";
            this.tgl_all.Size = new System.Drawing.Size(55, 29);
            this.tgl_all.TabIndex = 12;
            this.tgl_all.Text = "all";
            this.tgl_all.UseVisualStyleBackColor = true;
            this.tgl_all.CheckedChanged += new System.EventHandler(this.Tgl_all_CheckedChanged);
            // 
            // tgl_rest
            // 
            this.tgl_rest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgl_rest.AutoSize = true;
            this.tgl_rest.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_rest.Location = new System.Drawing.Point(485, 26);
            this.tgl_rest.Name = "tgl_rest";
            this.tgl_rest.Size = new System.Drawing.Size(68, 29);
            this.tgl_rest.TabIndex = 11;
            this.tgl_rest.Text = "rest";
            this.tgl_rest.UseVisualStyleBackColor = true;
            this.tgl_rest.CheckedChanged += new System.EventHandler(this.Tgl_rest_CheckedChanged);
            // 
            // tgl_waste
            // 
            this.tgl_waste.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgl_waste.AutoSize = true;
            this.tgl_waste.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_waste.Location = new System.Drawing.Point(377, 26);
            this.tgl_waste.Name = "tgl_waste";
            this.tgl_waste.Size = new System.Drawing.Size(87, 29);
            this.tgl_waste.TabIndex = 10;
            this.tgl_waste.Text = "waste";
            this.tgl_waste.UseVisualStyleBackColor = true;
            this.tgl_waste.CheckedChanged += new System.EventHandler(this.Tgl_waste_CheckedChanged);
            // 
            // tgl_work
            // 
            this.tgl_work.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgl_work.AutoSize = true;
            this.tgl_work.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_work.Location = new System.Drawing.Point(274, 26);
            this.tgl_work.Name = "tgl_work";
            this.tgl_work.Size = new System.Drawing.Size(78, 29);
            this.tgl_work.TabIndex = 9;
            this.tgl_work.Text = "work";
            this.tgl_work.UseVisualStyleBackColor = true;
            this.tgl_work.CheckedChanged += new System.EventHandler(this.Tgl_work_CheckedChanged);
            // 
            // tgl_study
            // 
            this.tgl_study.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tgl_study.AutoSize = true;
            this.tgl_study.Font = new System.Drawing.Font("Microsoft YaHei", 10.8F);
            this.tgl_study.Location = new System.Drawing.Point(167, 26);
            this.tgl_study.Name = "tgl_study";
            this.tgl_study.Size = new System.Drawing.Size(84, 29);
            this.tgl_study.TabIndex = 8;
            this.tgl_study.Text = "study";
            this.tgl_study.UseVisualStyleBackColor = true;
            this.tgl_study.CheckedChanged += new System.EventHandler(this.Tgl_study_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.study_btn);
            this.panel6.Controls.Add(this.work_btn);
            this.panel6.Controls.Add(this.rest_btn);
            this.panel6.Controls.Add(this.waste_btn);
            this.panel6.Location = new System.Drawing.Point(991, 91);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(124, 652);
            this.panel6.TabIndex = 20;
            // 
            // study_btn
            // 
            this.study_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.study_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(174)))), ((int)(((byte)(201)))));
            this.study_btn.FlatAppearance.BorderSize = 0;
            this.study_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.study_btn.Location = new System.Drawing.Point(25, 306);
            this.study_btn.Name = "study_btn";
            this.study_btn.Size = new System.Drawing.Size(84, 37);
            this.study_btn.TabIndex = 8;
            this.study_btn.Text = "study";
            this.study_btn.UseVisualStyleBackColor = false;
            this.study_btn.Click += new System.EventHandler(this.study_btn_Click);
            // 
            // work_btn
            // 
            this.work_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.work_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.work_btn.FlatAppearance.BorderSize = 0;
            this.work_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.work_btn.Location = new System.Drawing.Point(25, 227);
            this.work_btn.Name = "work_btn";
            this.work_btn.Size = new System.Drawing.Size(84, 37);
            this.work_btn.TabIndex = 6;
            this.work_btn.Text = "work";
            this.work_btn.UseVisualStyleBackColor = false;
            this.work_btn.Click += new System.EventHandler(this.work_btn_Click);
            // 
            // rest_btn
            // 
            this.rest_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rest_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rest_btn.FlatAppearance.BorderSize = 0;
            this.rest_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rest_btn.Location = new System.Drawing.Point(25, 48);
            this.rest_btn.Name = "rest_btn";
            this.rest_btn.Size = new System.Drawing.Size(84, 37);
            this.rest_btn.TabIndex = 7;
            this.rest_btn.Text = "rest";
            this.rest_btn.UseVisualStyleBackColor = false;
            this.rest_btn.Click += new System.EventHandler(this.rest_btn_Click);
            // 
            // waste_btn
            // 
            this.waste_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.waste_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.waste_btn.FlatAppearance.BorderSize = 0;
            this.waste_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.waste_btn.Location = new System.Drawing.Point(25, 133);
            this.waste_btn.Name = "waste_btn";
            this.waste_btn.Size = new System.Drawing.Size(84, 37);
            this.waste_btn.TabIndex = 5;
            this.waste_btn.Text = "waste";
            this.waste_btn.UseVisualStyleBackColor = false;
            this.waste_btn.Click += new System.EventHandler(this.waste_btn_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(11, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(340, 652);
            this.panel2.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(536, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 27);
            this.label3.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(634, 88);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(490, 655);
            this.panel3.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(536, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 27);
            this.label4.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.tgl_all);
            this.panel4.Controls.Add(this.tgl_rest);
            this.panel4.Controls.Add(this.tgl_study);
            this.panel4.Controls.Add(this.tgl_waste);
            this.panel4.Controls.Add(this.tgl_work);
            this.panel4.Location = new System.Drawing.Point(3, 574);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(619, 78);
            this.panel4.TabIndex = 21;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.chart1);
            this.panel5.Controls.Add(this.panel4);
            this.panel5.Location = new System.Drawing.Point(0, 88);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(628, 655);
            this.panel5.TabIndex = 22;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1127, 745);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "Form3";
            this.Text = "统计详情";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.Resize += new System.EventHandler(this.Form3_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker to_date;
        private System.Windows.Forms.DateTimePicker from_date;
        private System.Windows.Forms.Button refresh_btn;
        private System.Windows.Forms.CheckBox tgl_oneday;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button study_btn;
        private System.Windows.Forms.Button work_btn;
        private System.Windows.Forms.Button rest_btn;
        private System.Windows.Forms.Button waste_btn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox tgl_work;
        private System.Windows.Forms.CheckBox tgl_study;
        private System.Windows.Forms.CheckBox tgl_waste;
        private System.Windows.Forms.CheckBox tgl_rest;
        private System.Windows.Forms.CheckBox tgl_all;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}