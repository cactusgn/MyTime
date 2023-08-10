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
using MaterialDesignColors;

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
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public RecordPageUserControl RecordPageUserControl { get; set; } 
        public SummaryUserControl SummaryUserControl { get; set; }
        public ColorTool ColorTool { get; set; }
        public SummaryModel SummaryModel { get; set; }
        public string summaryBtnForegroundColor = Colors.Gray.ToString();
        public string recordBtnForegroundColor = Colors.Gray.ToString();
        public string colorBtnForegroundColor = Colors.Gray.ToString();
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
        public string ColorBtnForegroundColor
        {
            get { return colorBtnForegroundColor; }
            set { colorBtnForegroundColor = value; OnPropertyChanged(); }
        }
        public MainModel(SummaryModel summaryModel,RecordModel recordModel)
        {
            ITheme theme = _paletteHelper.GetTheme();
            //theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#2884D5"));
            theme.SetPrimaryColor(Colors.LightPink);
            _paletteHelper.SetTheme(theme);
            OpenPageCommand = new MyCommand(OpenPage);
            RecordPageUserControl = new RecordPageUserControl(recordModel);
            SummaryModel = summaryModel;
            SummaryUserControl = new SummaryUserControl(summaryModel);
            ColorTool = new ColorTool();
            OpenPage("RecordPageUserControl");
        }
       
        private void OpenPage(object o)
        {
            var palette = _paletteHelper.GetTheme().PrimaryDark;
            if (o.ToString() == "RecordPageUserControl")
            {
                MainContent = RecordPageUserControl;
                ResetColor();
                RecordBtnForegroundColor = palette.Color.ToString();
            }
            else if(o.ToString() == "SummaryUserControl")
            {
                MainContent = SummaryUserControl;
                ResetColor();
                SummaryBtnForegroundColor = palette.Color.ToString();
            }else if(o.ToString() == "ColorTool")
            {
                MainContent = ColorTool;
                ResetColor();
                ColorBtnForegroundColor = palette.Color.ToString();
            }
        }
        private void ResetColor()
        {
            ColorBtnForegroundColor = Colors.Gray.ToString();
            SummaryBtnForegroundColor = Colors.Gray.ToString();
            RecordBtnForegroundColor = Colors.Gray.ToString();
        }
    }


}
