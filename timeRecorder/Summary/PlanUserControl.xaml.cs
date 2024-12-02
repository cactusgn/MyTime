using Summary.Common;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// PlanUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlanUserControl : UserControl
    {
        public PlanUserControl(PlanModel planModel)
        {
            InitializeComponent();
            this.DataContext = planModel;
            planModel.TimeGrid = this.TimeGrid;
            planModel.clickOkButton(null);
        }
        
       
       
        private void ThemeListBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen flag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;

            while (dependencyObject != null)
            {
                if (dependencyObject is MaterialDesignThemes.Wpf.DrawerHost) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

           // ThemeToggleButton.IsChecked = false;
        }

        private void TimeGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var columnIndex = TimeGrid.CurrentCell.Column.DisplayIndex;
            var columnName = TimeGrid.Columns[columnIndex].Header.ToString();
            if (columnName=="获得"||columnName=="时间")
            {
                TimeGrid.UnselectAllCells();
            }
        }
    }
}
