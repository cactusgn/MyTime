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
    /// SummaryUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class SummaryUserControl : UserControl
    {

        public SummaryUserControl(SummaryModel summaryModel)
        {
            InitializeComponent();
            summaryModel.SingleDayPlot = SingleDayPlot;
            summaryModel.SummaryPlot = SummaryPlot;
            summaryModel.WorkRB = WorkRB;
            summaryModel.StudyRB = StudyRB;
            summaryModel.WasteRB = WasteRB;
            summaryModel.RestRB = RestRB;
            summaryModel.AllRB = AllRB;
            summaryModel.ThirdLevelRB = ThirdLevelRB;
            summaryModel.FirstLevelRB = FirstLevelRB;
            this.DataContext = summaryModel;
            summaryModel.showTimeView();
        }
        private void leftPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((SummaryModel)this.DataContext).LeftPanelHeight = Result.ActualHeight;
            ((SummaryModel)this.DataContext).resizeHeight();
        }

        private void DialogHost2_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true))
                return;
            var dialogRes = ((SummaryModel)this.DataContext).sampleDialogViewModel;

            if (!(dialogRes.SplitTime<dialogRes.EndTime&&dialogRes.SplitTime>dialogRes.StartTime)) {
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
            ((SummaryModel)this.DataContext).SplitTimeBlock(dialogRes.SplitTime, dialogRes.Content1, dialogRes.Content2);
        }

        private void DialogHost2_DialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {
            var dialogRes = ((SummaryModel)this.DataContext).sampleDialogViewModel;
            dialogRes.ShowTip = "Hidden";
            ((SummaryModel)this.DataContext).sampleDialogViewModel.Content1 = "";
            ((SummaryModel)this.DataContext).sampleDialogViewModel.Content2 = "";
            if (!Equals(eventArgs.Parameter, true))
                return;
        }
    }
}
