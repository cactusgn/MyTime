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
            summaryModel.SingleDayTypeRadioGroupPanel = SingleDayTypeRadioGroupPanel;
            summaryModel.TypeComboBox = TypeComboBox;
            summaryModel.ComboBoxItemStyle = this.Resources["ComboBoxItemStyle"] as Style;
            summaryModel.TypeRadioGroupPanel = TypeRadioGroupPanel;
            summaryModel.LeftSchedule = leftSchedule;
            summaryModel.RightSchedule = RightSchedule;
            summaryModel.Diary = Diary;
            summaryModel.drawerDiaryPanel = drawerDiaryPanel;
            summaryModel.rightPanel = rightPanel;
            this.DataContext = summaryModel;
            summaryModel.initTypeCombobox();
            summaryModel.RefreshSingleDayRadioButtons();
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
        private void AddBindingWidthForDrawerPanel()
        {
            // 确保sourcePanel已加载并有一个有效的ActualWidth
            if (rightPanel != null && rightPanel.IsLoaded && rightPanel.ActualWidth > 0)
            {
                // 创建一个新的Binding对象
                Binding binding = new Binding("ActualWidth")
                {
                    Source = rightPanel, // 设置源元素
                    Mode = BindingMode.OneWay // 通常我们不需要双向绑定ActualWidth
                };

                // 使用BindingOperations.SetBinding来设置绑定
                BindingOperations.SetBinding(drawerDiaryPanel, StackPanel.WidthProperty, binding);
            }
        }
        private void rightPanel_Loaded(object sender, RoutedEventArgs e)
        {
            AddBindingWidthForDrawerPanel();
        }
    }
}
