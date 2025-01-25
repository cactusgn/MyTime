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
using System.Configuration;
using Summary.Common.Utils;
using Microsoft.Data.SqlClient;
using ScottPlot.Drawing.Colormaps;

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
        public TaskManagerUserControl TaskManagerUserControl { get; set; }
        public PlanUserControl PlanUserControl { get; set; }
        public ColorTool ColorTool { get; set; }
        public Settings Settings { get; set; }
        public SummaryModel SummaryModel { get; set; }
        public string summaryBtnForegroundColor = Colors.Gray.ToString();
        public string recordBtnForegroundColor = Colors.Gray.ToString();
        public string colorBtnForegroundColor = Colors.Gray.ToString();
        public string settingsBtnForegroundColor = Colors.Gray.ToString();
        public string taskBtnForegroundColor = Colors.Gray.ToString();
        public string planBtnForegroundColor = Colors.Gray.ToString();
        public string PlanBtnForegroundColor
        {
            get { return planBtnForegroundColor; }
            set { planBtnForegroundColor = value; OnPropertyChanged(); }
        }
        public string TaskBtnForegroundColor
        {
            get { return taskBtnForegroundColor; }
            set { taskBtnForegroundColor = value; OnPropertyChanged(); }
        }
        public string SettingsBtnForegroundColor
        {
            get { return settingsBtnForegroundColor; }
            set { settingsBtnForegroundColor = value; OnPropertyChanged(); }
        }
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
        public string hoverForegroundColor = Colors.Gray.ToString();
        public string HoverForegroundColor
        {
            get { return hoverForegroundColor; }
            set { hoverForegroundColor = value; OnPropertyChanged(); }
        }
        private RecordModel RecordModel;
        private PlanModel PlanModel;
        private TaskManagerModel TaskManagerModel;
        private bool isDark ;
        public MainModel(SummaryModel summaryModel,RecordModel recordModel, TaskManagerModel taskManagerModel, PlanModel planModel)
        {
            ITheme theme = _paletteHelper.GetTheme();
            //theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#2884D5"));
            string ThemeColor = Helper.GetAppSetting("ThemeColor");
            isDark = bool.Parse(Helper.GetAppSetting("IsDark"));
            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString(ThemeColor));
            if(isDark){
                theme.SetBaseTheme(new MaterialDesignDarkTheme());
            }else{
                theme.SetBaseTheme(new MaterialDesignLightTheme());
            }
            _paletteHelper.SetTheme(theme);
            HoverForegroundColor = getColor(_paletteHelper.GetTheme().PrimaryMid.Color.ToString());
            OpenPageCommand = new MyCommand(OpenPage);
            Settings = new Settings(new SettingsModel());
            RecordPageUserControl = new RecordPageUserControl(recordModel);
            RecordModel = recordModel;
            SummaryModel = summaryModel;
            SummaryUserControl = new SummaryUserControl(summaryModel);
            TaskManagerModel = taskManagerModel;
            TaskManagerUserControl = new TaskManagerUserControl(taskManagerModel);
            PlanModel = planModel;
            PlanUserControl = new PlanUserControl(planModel);
            ColorTool = new ColorTool(this);
            OpenPage("RecordPageUserControl");
        }
        static System.Windows.Media.Color AdjustBrightness(System.Drawing.Color color, float factor)
        {
            // 将颜色的R、G、B分量分别乘以因子，并限制在0-255范围内
            int r = (int)(color.R * factor);
            int g = (int)(color.G * factor);
            int b = (int)(color.B * factor);

            // 使用Math.Min和Math.Max确保值在有效范围内
            r = Math.Min(255, Math.Max(0, r));
            g = Math.Min(255, Math.Max(0, g));
            b = Math.Min(255, Math.Max(0, b));

            // 返回新的颜色
            return System.Windows.Media.Color.FromRgb(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));
        }
        private string getColor(string colorString){
            isDark = bool.Parse(Helper.GetAppSetting("IsDark"));
            System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(colorString);
            if (!isDark){
                System.Windows.Media.Color darkerColor = AdjustBrightness(color, 0.8f);
                HoverForegroundColor = darkerColor.ToString();
                return darkerColor.ToString();
            }else{
                HoverForegroundColor = colorString;
                return colorString;
            }
        }
        private  void OpenPage(object o)
        {
            ITheme theme = _paletteHelper.GetTheme();
            //bool IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;
            var palette =  _paletteHelper.GetTheme().PrimaryMid;
            //var palette = IsDarkTheme? _paletteHelper.GetTheme().PrimaryMid : _paletteHelper.GetTheme().PrimaryMid;
            if (o.ToString() == "RecordPageUserControl")
            {
                RecordModel.initCategoryDic();
                RecordModel.InitTodayData();
                RecordModel.initDiary();
                RecordModel.RefreshRadioButtons();
                RecordModel.refreshSingleDayPlot();
                MainContent = RecordPageUserControl;
                ResetColor();
                RecordBtnForegroundColor = getColor(palette.Color.ToString());
            }
            else if(o.ToString() == "SummaryUserControl")
            {
                MainContent = SummaryUserControl;
                ResetColor();
                SummaryBtnForegroundColor = getColor(palette.Color.ToString());
                //SummaryModel.initTypeCombobox();
                //SummaryModel.RefreshSingleDayRadioButtons();
                //SummaryModel.clickOkButton();
            }else if(o.ToString() == "ColorTool")
            {
                MainContent = ColorTool;
                ResetColor();
                ColorBtnForegroundColor = getColor(palette.Color.ToString());
            }
            else if (o.ToString() == "Settings")
            {
                MainContent = Settings;
                ResetColor();
                SettingsBtnForegroundColor = getColor(palette.Color.ToString());
            }
            else if (o.ToString() == "TaskManager")
            {
                TaskManagerModel.queryTaskModel.clickOkButton();
                MainContent = TaskManagerUserControl;
                ResetColor();
                TaskBtnForegroundColor = getColor(palette.Color.ToString());
            }else if (o.ToString()=="PlanManager")
            {
                MainContent = PlanUserControl;
                ResetColor();
                PlanBtnForegroundColor = getColor(palette.Color.ToString());
            }
        }
        private void ResetColor()
        {
            ColorBtnForegroundColor = Colors.Gray.ToString();
            SummaryBtnForegroundColor = Colors.Gray.ToString();
            RecordBtnForegroundColor = Colors.Gray.ToString();
            SettingsBtnForegroundColor = Colors.Gray.ToString();
            TaskBtnForegroundColor = Colors.Gray.ToString();
            PlanBtnForegroundColor = Colors.Gray.ToString();
        }
    }


}
