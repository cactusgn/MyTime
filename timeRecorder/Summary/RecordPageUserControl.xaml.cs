using Summary.Models;
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
            RecordModel.refreshSingleDayPlot();
        }

        private void RightPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((RecordModel)this.DataContext).RightPanelHeight = RightPanel.ActualHeight;
            ((RecordModel)this.DataContext).resizeHeight();
        }
        private void DialogHost2_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true))
                return;
            var dialogRes = ((RecordModel)this.DataContext).sampleDialogViewModel;

            if (!(dialogRes.SplitTime<dialogRes.EndTime&&dialogRes.SplitTime>dialogRes.StartTime))
            {
                dialogRes.ShowTip = "Visible";
                eventArgs.Cancel();
                return;
            }
            else
            {
                dialogRes.ShowTip = "Hidden";
            }
            if (dialogRes.Content1=="")
            {
                eventArgs.Cancel();
                return;
            }
            ((RecordModel)this.DataContext).SplitTimeBlock(dialogRes.SplitTime, dialogRes.Content1, dialogRes.Content2);
        }

        private void DialogHost2_DialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {
            var dialogRes = ((RecordModel)this.DataContext).sampleDialogViewModel;
            dialogRes.ShowTip = "Hidden";
            ((RecordModel)this.DataContext).sampleDialogViewModel.Content1 = "";
            ((RecordModel)this.DataContext).sampleDialogViewModel.Content2 = "";
            if (!Equals(eventArgs.Parameter, true))
                return;
        }
    }
}
