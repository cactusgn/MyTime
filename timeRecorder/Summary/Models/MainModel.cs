using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Threading.Tasks;
using Summary.Domain;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using ScottPlot;
using static ScottPlot.Plottable.PopulationPlot;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using ScottPlot.MarkerShapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel;
using System.Reflection;

namespace Summary.Models
{
    public class MainModel : ViewModelBase
    {

        public MyCommand OpenPageCommand { get; set; }

        private UIElement mainContent;
        public UIElement MainContent
        {
            get { return mainContent; }
            set { mainContent = value; OnPropertyChanged(); }
        }

        public RecordPageUserControl RecordPageUserControl { get; set; } 
        public SummaryUserControl SummaryUserControl { get; set; }
        public SummaryModel SummaryModel { get; set; }
        public string summaryBtnForegroundColor = Colors.Gray.ToString();
        public string recordBtnForegroundColor = Colors.Gray.ToString();
        public string SummaryBtnForegroundColor
        {
            get { return summaryBtnForegroundColor; }
            set { summaryBtnForegroundColor = value; OnPropertyChanged(); }
        }
        public string RecordBtnForegroundColor
        {
            get { return recordBtnForegroundColor; }
            set { recordBtnForegroundColor = value; OnPropertyChanged(); }
        }
        public MainModel(SummaryModel summaryModel)
        {
            OpenPageCommand = new MyCommand(OpenPage);
            RecordPageUserControl = new RecordPageUserControl();
            SummaryModel = summaryModel;
            SummaryUserControl = new SummaryUserControl(summaryModel);
        }

        private void OpenPage(object o)
        {
            if (o.ToString() == "RecordPageUserControl")
            {
                MainContent = RecordPageUserControl;
                RecordBtnForegroundColor = "#2884D5";
                SummaryBtnForegroundColor = Colors.Gray.ToString();
            }
            else
            {
                MainContent = SummaryUserControl;
                RecordBtnForegroundColor = Colors.Gray.ToString();
                SummaryBtnForegroundColor = "#2884D5";
            }
        }

    }


}
