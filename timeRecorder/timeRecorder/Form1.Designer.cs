namespace timeRecorder
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rest_btn = new System.Windows.Forms.Button();
            this.waste_btn = new System.Windows.Forms.Button();
            this.work_btn = new System.Windows.Forms.Button();
            this.chartPanel = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.study_btn = new System.Windows.Forms.Button();
            this.large_btn = new System.Windows.Forms.Button();
            this.return_btn = new System.Windows.Forms.Button();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.memo_label = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.Add_btn = new System.Windows.Forms.Button();
            this.AddEndTime = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.AddStartTime = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.setupBtn = new System.Windows.Forms.Button();
            this.accu_mode = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.connectToDb = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TimeReminder = new System.Windows.Forms.TextBox();
            this.ThingsToDo = new System.Windows.Forms.TextBox();
            this.minimize = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.output_btn = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.end_btn = new System.Windows.Forms.Button();
            this.summary = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.workItemTextBox = new System.Windows.Forms.TextBox();
            this.todayList = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1.SuspendLayout();
            this.chartPanel.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(536, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 27);
            this.label3.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(14, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(340, 773);
            this.panel1.TabIndex = 10;
            // 
            // rest_btn
            // 
            this.rest_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rest_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rest_btn.FlatAppearance.BorderSize = 0;
            this.rest_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rest_btn.Location = new System.Drawing.Point(26, 69);
            this.rest_btn.Name = "rest_btn";
            this.rest_btn.Size = new System.Drawing.Size(84, 34);
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
            this.waste_btn.Location = new System.Drawing.Point(26, 148);
            this.waste_btn.Name = "waste_btn";
            this.waste_btn.Size = new System.Drawing.Size(84, 34);
            this.waste_btn.TabIndex = 5;
            this.waste_btn.Text = "waste";
            this.waste_btn.UseVisualStyleBackColor = false;
            this.waste_btn.Click += new System.EventHandler(this.waste_btn_Click);
            // 
            // work_btn
            // 
            this.work_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.work_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.work_btn.FlatAppearance.BorderSize = 0;
            this.work_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.work_btn.Location = new System.Drawing.Point(26, 236);
            this.work_btn.Name = "work_btn";
            this.work_btn.Size = new System.Drawing.Size(84, 34);
            this.work_btn.TabIndex = 6;
            this.work_btn.Text = "work";
            this.work_btn.UseVisualStyleBackColor = false;
            this.work_btn.Click += new System.EventHandler(this.work_btn_Click);
            // 
            // chartPanel
            // 
            this.chartPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartPanel.Controls.Add(this.panel6);
            this.chartPanel.Controls.Add(this.panel1);
            this.chartPanel.Controls.Add(this.large_btn);
            this.chartPanel.Controls.Add(this.return_btn);
            this.chartPanel.Controls.Add(this.chart2);
            this.chartPanel.Controls.Add(this.chart1);
            this.chartPanel.Location = new System.Drawing.Point(17, 3);
            this.chartPanel.Name = "chartPanel";
            this.chartPanel.Size = new System.Drawing.Size(481, 773);
            this.chartPanel.TabIndex = 17;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.study_btn);
            this.panel6.Controls.Add(this.work_btn);
            this.panel6.Controls.Add(this.rest_btn);
            this.panel6.Controls.Add(this.waste_btn);
            this.panel6.Location = new System.Drawing.Point(355, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(124, 773);
            this.panel6.TabIndex = 18;
            // 
            // study_btn
            // 
            this.study_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.study_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(174)))), ((int)(((byte)(201)))));
            this.study_btn.FlatAppearance.BorderSize = 0;
            this.study_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.study_btn.Location = new System.Drawing.Point(26, 310);
            this.study_btn.Name = "study_btn";
            this.study_btn.Size = new System.Drawing.Size(84, 34);
            this.study_btn.TabIndex = 8;
            this.study_btn.Text = "study";
            this.study_btn.UseVisualStyleBackColor = false;
            this.study_btn.Click += new System.EventHandler(this.study_btn_Click);
            // 
            // large_btn
            // 
            this.large_btn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.large_btn.BackColor = System.Drawing.Color.Transparent;
            this.large_btn.BackgroundImage = global::timeRecorder.Properties.Resources.放大;
            this.large_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.large_btn.FlatAppearance.BorderSize = 0;
            this.large_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.large_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.large_btn.Location = new System.Drawing.Point(411, 337);
            this.large_btn.Name = "large_btn";
            this.large_btn.Size = new System.Drawing.Size(30, 28);
            this.large_btn.TabIndex = 15;
            this.large_btn.UseVisualStyleBackColor = false;
            this.large_btn.Click += new System.EventHandler(this.Large_btn_Click);
            // 
            // return_btn
            // 
            this.return_btn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.return_btn.BackColor = System.Drawing.Color.Transparent;
            this.return_btn.BackgroundImage = global::timeRecorder.Properties.Resources.返回;
            this.return_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.return_btn.FlatAppearance.BorderSize = 0;
            this.return_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.return_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.return_btn.Location = new System.Drawing.Point(447, 337);
            this.return_btn.Name = "return_btn";
            this.return_btn.Size = new System.Drawing.Size(30, 28);
            this.return_btn.TabIndex = 17;
            this.return_btn.UseVisualStyleBackColor = false;
            this.return_btn.Click += new System.EventHandler(this.return_btn_Click);
            // 
            // chart2
            // 
            this.chart2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chart2.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX2.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackSecondaryColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.LegendStyle = System.Windows.Forms.DataVisualization.Charting.LegendStyle.Row;
            legend1.Name = "Legend1";
            this.chart2.Legends.Add(legend1);
            this.chart2.Location = new System.Drawing.Point(24, 392);
            this.chart2.Name = "chart2";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(415, 331);
            this.chart2.TabIndex = 16;
            this.chart2.Text = "chart2";
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BackSecondaryColor = System.Drawing.Color.LightBlue;
            chartArea2.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(3, 26);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(481, 269);
            this.chart1.TabIndex = 15;
            this.chart1.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "间隔时间:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(130, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 2;
            // 
            // memo_label
            // 
            this.memo_label.BackColor = System.Drawing.Color.Transparent;
            this.memo_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.memo_label.ForeColor = System.Drawing.Color.DimGray;
            this.memo_label.Location = new System.Drawing.Point(18, 189);
            this.memo_label.Name = "memo_label";
            this.memo_label.Size = new System.Drawing.Size(350, 26);
            this.memo_label.TabIndex = 4;
            this.memo_label.Text = "let time go with your mind";
            this.memo_label.Click += new System.EventHandler(this.Memo_label_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.Location = new System.Drawing.Point(15, 286);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(597, 489);
            this.dataGridView1.TabIndex = 12;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.DataGridView1_CurrentCellChanged);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.Add_btn);
            this.panel5.Controls.Add(this.AddEndTime);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.AddStartTime);
            this.panel5.Controls.Add(this.label11);
            this.panel5.Location = new System.Drawing.Point(15, 237);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(597, 37);
            this.panel5.TabIndex = 15;
            // 
            // Add_btn
            // 
            this.Add_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Add_btn.FlatAppearance.BorderSize = 0;
            this.Add_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Add_btn.Location = new System.Drawing.Point(469, 2);
            this.Add_btn.Name = "Add_btn";
            this.Add_btn.Size = new System.Drawing.Size(84, 34);
            this.Add_btn.TabIndex = 19;
            this.Add_btn.Text = "Add";
            this.Add_btn.UseVisualStyleBackColor = false;
            this.Add_btn.Click += new System.EventHandler(this.Add_btn_Click);
            // 
            // AddEndTime
            // 
            this.AddEndTime.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddEndTime.Location = new System.Drawing.Point(294, 3);
            this.AddEndTime.Name = "AddEndTime";
            this.AddEndTime.Size = new System.Drawing.Size(115, 31);
            this.AddEndTime.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("幼圆", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(226, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 19);
            this.label12.TabIndex = 16;
            this.label12.Text = "结束：";
            // 
            // AddStartTime
            // 
            this.AddStartTime.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AddStartTime.Location = new System.Drawing.Point(72, 3);
            this.AddStartTime.Name = "AddStartTime";
            this.AddStartTime.Size = new System.Drawing.Size(115, 31);
            this.AddStartTime.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("幼圆", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(6, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 19);
            this.label11.TabIndex = 15;
            this.label11.Text = "开始：";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.setupBtn);
            this.panel3.Controls.Add(this.accu_mode);
            this.panel3.Controls.Add(this.memo_label);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.connectToDb);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.TimeReminder);
            this.panel3.Controls.Add(this.ThingsToDo);
            this.panel3.Controls.Add(this.minimize);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.output_btn);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.btn_start);
            this.panel3.Controls.Add(this.reset);
            this.panel3.Controls.Add(this.end_btn);
            this.panel3.Controls.Add(this.summary);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(10, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(602, 219);
            this.panel3.TabIndex = 11;
            // 
            // setupBtn
            // 
            this.setupBtn.BackColor = System.Drawing.Color.Transparent;
            this.setupBtn.BackgroundImage = global::timeRecorder.Properties.Resources.setup;
            this.setupBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.setupBtn.FlatAppearance.BorderSize = 0;
            this.setupBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.setupBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.setupBtn.Location = new System.Drawing.Point(399, 53);
            this.setupBtn.Name = "setupBtn";
            this.setupBtn.Size = new System.Drawing.Size(39, 39);
            this.setupBtn.TabIndex = 16;
            this.setupBtn.UseVisualStyleBackColor = false;
            this.setupBtn.Click += new System.EventHandler(this.setupBtn_Click);
            // 
            // accu_mode
            // 
            this.accu_mode.AutoSize = true;
            this.accu_mode.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.accu_mode.Location = new System.Drawing.Point(373, 151);
            this.accu_mode.Name = "accu_mode";
            this.accu_mode.Size = new System.Drawing.Size(110, 29);
            this.accu_mode.TabIndex = 15;
            this.accu_mode.Text = "累计模式";
            this.accu_mode.UseVisualStyleBackColor = true;
            this.accu_mode.CheckedChanged += new System.EventHandler(this.accu_mode_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(201, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 27);
            this.label9.TabIndex = 14;
            this.label9.Text = "Min";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(11, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 27);
            this.label8.TabIndex = 14;
            this.label8.Text = "间隔提醒：";
            // 
            // connectToDb
            // 
            this.connectToDb.AutoSize = true;
            this.connectToDb.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.connectToDb.Location = new System.Drawing.Point(373, 109);
            this.connectToDb.Name = "connectToDb";
            this.connectToDb.Size = new System.Drawing.Size(129, 29);
            this.connectToDb.TabIndex = 13;
            this.connectToDb.Text = "连接数据库";
            this.connectToDb.UseVisualStyleBackColor = true;
            this.connectToDb.CheckedChanged += new System.EventHandler(this.ConnectToDb_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(10, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 27);
            this.label7.TabIndex = 12;
            this.label7.Text = "工作内容:";
            // 
            // TimeReminder
            // 
            this.TimeReminder.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TimeReminder.Location = new System.Drawing.Point(131, 149);
            this.TimeReminder.Name = "TimeReminder";
            this.TimeReminder.Size = new System.Drawing.Size(64, 31);
            this.TimeReminder.TabIndex = 11;
            this.TimeReminder.Leave += new System.EventHandler(this.TimeReminder_Leave);
            // 
            // ThingsToDo
            // 
            this.ThingsToDo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.ThingsToDo.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ThingsToDo.Location = new System.Drawing.Point(131, 111);
            this.ThingsToDo.Name = "ThingsToDo";
            this.ThingsToDo.Size = new System.Drawing.Size(205, 31);
            this.ThingsToDo.TabIndex = 11;
            this.ThingsToDo.TextChanged += new System.EventHandler(this.ThingsToDo_TextChanged);
            this.ThingsToDo.Enter += new System.EventHandler(this.ThingsToDo_Enter);
            this.ThingsToDo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThingsToDo_KeyDown);
            this.ThingsToDo.Leave += new System.EventHandler(this.ThingsToDo_Leave);
            // 
            // minimize
            // 
            this.minimize.BackColor = System.Drawing.Color.Transparent;
            this.minimize.BackgroundImage = global::timeRecorder.Properties.Resources.最小化_2_;
            this.minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minimize.FlatAppearance.BorderSize = 0;
            this.minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimize.Location = new System.Drawing.Point(474, 53);
            this.minimize.Name = "minimize";
            this.minimize.Size = new System.Drawing.Size(39, 37);
            this.minimize.TabIndex = 10;
            this.minimize.UseVisualStyleBackColor = false;
            this.minimize.Click += new System.EventHandler(this.minimize_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(536, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 27);
            this.label6.TabIndex = 3;
            // 
            // output_btn
            // 
            this.output_btn.BackColor = System.Drawing.Color.Transparent;
            this.output_btn.BackgroundImage = global::timeRecorder.Properties.Resources.导出;
            this.output_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.output_btn.FlatAppearance.BorderSize = 0;
            this.output_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.output_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.output_btn.Location = new System.Drawing.Point(327, 56);
            this.output_btn.Name = "output_btn";
            this.output_btn.Size = new System.Drawing.Size(35, 34);
            this.output_btn.TabIndex = 9;
            this.output_btn.UseVisualStyleBackColor = false;
            this.output_btn.Click += new System.EventHandler(this.output_btn_Click);
            // 
            // btn_start
            // 
            this.btn_start.BackColor = System.Drawing.Color.Transparent;
            this.btn_start.BackgroundImage = global::timeRecorder.Properties.Resources.开始;
            this.btn_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_start.FlatAppearance.BorderSize = 0;
            this.btn_start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_start.Location = new System.Drawing.Point(29, 56);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(40, 40);
            this.btn_start.TabIndex = 0;
            this.btn_start.UseVisualStyleBackColor = false;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // reset
            // 
            this.reset.BackColor = System.Drawing.Color.Transparent;
            this.reset.BackgroundImage = global::timeRecorder.Properties.Resources.导入_1_;
            this.reset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.reset.FlatAppearance.BorderSize = 0;
            this.reset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reset.Location = new System.Drawing.Point(248, 57);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(35, 34);
            this.reset.TabIndex = 0;
            this.reset.UseVisualStyleBackColor = false;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // end_btn
            // 
            this.end_btn.BackColor = System.Drawing.Color.Transparent;
            this.end_btn.BackgroundImage = global::timeRecorder.Properties.Resources.停止;
            this.end_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.end_btn.FlatAppearance.BorderSize = 0;
            this.end_btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.end_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.end_btn.Location = new System.Drawing.Point(95, 57);
            this.end_btn.Name = "end_btn";
            this.end_btn.Size = new System.Drawing.Size(35, 34);
            this.end_btn.TabIndex = 0;
            this.end_btn.UseVisualStyleBackColor = false;
            this.end_btn.Click += new System.EventHandler(this.end_btn_Click);
            // 
            // summary
            // 
            this.summary.BackColor = System.Drawing.Color.Transparent;
            this.summary.BackgroundImage = global::timeRecorder.Properties.Resources.总计_1_;
            this.summary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.summary.FlatAppearance.BorderSize = 0;
            this.summary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.summary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.summary.Location = new System.Drawing.Point(170, 57);
            this.summary.Name = "summary";
            this.summary.Size = new System.Drawing.Size(35, 34);
            this.summary.TabIndex = 8;
            this.summary.UseVisualStyleBackColor = false;
            this.summary.Click += new System.EventHandler(this.summary_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("微软雅黑", 10.8F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 24;
            this.listBox1.Location = new System.Drawing.Point(140, 150);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(205, 124);
            this.listBox1.TabIndex = 17;
            this.listBox1.Visible = false;
            this.listBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseClick);
            this.listBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listBox1_KeyPress);
            this.listBox1.Leave += new System.EventHandler(this.listBox1_Leave);
            this.listBox1.LostFocus += new System.EventHandler(this.listBox1_Leave);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.listBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseMove);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Linen;
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.workItemTextBox);
            this.panel4.Controls.Add(this.todayList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(360, 779);
            this.panel4.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(4, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(172, 27);
            this.label10.TabIndex = 15;
            this.label10.Text = "今天要做的工作：";
            // 
            // workItemTextBox
            // 
            this.workItemTextBox.BackColor = System.Drawing.Color.Honeydew;
            this.workItemTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.workItemTextBox.Font = new System.Drawing.Font("微软雅黑", 10.8F);
            this.workItemTextBox.Location = new System.Drawing.Point(3, 51);
            this.workItemTextBox.Multiline = true;
            this.workItemTextBox.Name = "workItemTextBox";
            this.workItemTextBox.Size = new System.Drawing.Size(392, 32);
            this.workItemTextBox.TabIndex = 1;
            this.workItemTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.workItemTextBox_KeyDown);
            // 
            // todayList
            // 
            this.todayList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.todayList.BackColor = System.Drawing.Color.Linen;
            this.todayList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.todayList.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.todayList.FormattingEnabled = true;
            this.todayList.ItemHeight = 24;
            this.todayList.Location = new System.Drawing.Point(0, 104);
            this.todayList.Name = "todayList";
            this.todayList.Size = new System.Drawing.Size(357, 672);
            this.todayList.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter1.Location = new System.Drawing.Point(360, 0);
            this.splitter1.MinExtra = 650;
            this.splitter1.MinSize = 300;
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 779);
            this.splitter1.TabIndex = 19;
            this.splitter1.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel7.Controls.Add(this.chartPanel);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(1009, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(501, 779);
            this.panel7.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(363, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(643, 779);
            this.panel2.TabIndex = 21;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(1006, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 779);
            this.splitter2.TabIndex = 21;
            this.splitter2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1510, 779);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel4);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "计时器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.chartPanel.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button end_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label memo_label;
        private System.Windows.Forms.Button waste_btn;
        private System.Windows.Forms.Button work_btn;
        private System.Windows.Forms.Button rest_btn;
        private System.Windows.Forms.Button summary;
        private System.Windows.Forms.Button output_btn;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button minimize;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox ThingsToDo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox connectToDb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TimeReminder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Panel chartPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button large_btn;
        private System.Windows.Forms.ListBox todayList;
        private System.Windows.Forms.TextBox workItemTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox AddStartTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox AddEndTime;
        private System.Windows.Forms.Button Add_btn;
        private System.Windows.Forms.CheckBox accu_mode;
        private System.Windows.Forms.Button return_btn;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Button study_btn;
        private System.Windows.Forms.Button setupBtn;
        private System.Windows.Forms.ListBox listBox1;
    }
}

