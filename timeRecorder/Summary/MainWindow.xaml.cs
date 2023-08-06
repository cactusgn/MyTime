using MaterialDesignThemes.Wpf;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainModel mainModel)
        {
            InitializeComponent();
            mainModel.SingleDayPlot = SingleDayPlot;
            mainModel.SummaryPlot = SummaryPlot;
            mainModel.WorkRB = WorkRB;
            mainModel.StudyRB = StudyRB;
            mainModel.WasteRB = WasteRB;
            mainModel.RestRB = RestRB;
            mainModel.AllRB = AllRB;
            this.DataContext = mainModel;
            mainModel.showTimeView();
        }
        private void leftPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MainModel)this.DataContext).LeftPanelHeight = Result.ActualHeight;
            ((MainModel)this.DataContext).resizeHeight();
        }
       
    }
}
