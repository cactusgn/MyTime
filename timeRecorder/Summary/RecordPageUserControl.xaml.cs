﻿using Summary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Summary
{
    /// <summary>
    /// RecordPageUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class RecordPageUserControl : UserControl
    {
        public RecordPageUserControl(RecordModel RecordModel)
        {
            InitializeComponent();
            RecordModel.SingleDayPlot = SingleDayPlot;
            RecordModel.ThirdLevelRB = ThirdLevelRB;
            RecordModel.FirstLevelRB = FirstLevelRB;
            this.DataContext = RecordModel;
        }

        private void RightPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((RecordModel)this.DataContext).RightPanelHeight = RightPanel.ActualHeight;
            ((RecordModel)this.DataContext).resizeHeight();
        }
    }
}
