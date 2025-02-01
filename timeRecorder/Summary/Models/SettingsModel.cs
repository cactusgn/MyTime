using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using ScottPlot.Drawing.Colormaps;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Summary.Models
{
    public class SettingsModel:ViewModelBase
    {
        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; OnPropertyChanged(); }
        }
        private string importDirectory;

        public string ImportDirectory
        {
            get { return importDirectory; }
            set { importDirectory = value; OnPropertyChanged(); }
        }
        private string outputDirectory;

        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; OnPropertyChanged(); }
        }

        private string workDirectory;

        public string WorkDirectory
        {
            get { return workDirectory; }
            set { workDirectory = value; OnPropertyChanged(); }
        }

        private string diaryContent;

        public string DiaryContent
        {
            get { return diaryContent; }
            set { diaryContent = value; OnPropertyChanged(); }
        }
        private string restContent;

        public string RestContent
        {
            get { return restContent; }
            set { restContent = value; OnPropertyChanged(); }
        }
        public MyCommand SaveCommand { get; set; }
        private string intervalMinutes;

        public string IntervalMinutes
        {
            get { return intervalMinutes; }
            set { intervalMinutes = value; OnPropertyChanged(); }
        }
        public ISQLCommands SQLCommands { get; set; }
        public TextBox Diary { get; internal set; }
        public MyCommand Tab_ClickCommand { get; set; }
        public MyCommand DiaryKeyDownCommand { get; set; }
        public MyCommand Return_ClickCommand { get; set; }
        private Diary template;
        public SettingsModel(ISQLCommands SqlCommands) {
            SaveCommand = new MyCommand(Save);
            Tab_ClickCommand = new MyCommand(TabKeySub);
            Return_ClickCommand = new MyCommand(ReturnKeySub);
            DiaryKeyDownCommand = new MyCommand(DiaryKeyDown);
            StartTime = Helper.GetAppSetting("StartTime");
            RestContent = Helper.GetAppSetting("RestContent");
            OutputDirectory = Helper.GetAppSetting("OutputDirectory");
            ImportDirectory = Helper.GetAppSetting("ImportDirectory");
            WorkDirectory = Helper.GetAppSetting("WorkDirectory");
            IntervalMinutes = Helper.GetAppSetting("IntervalMinutes");
            SQLCommands = SqlCommands;
            template = SQLCommands.GetDiary(DateTime.Now, DiaryType.Template);
            DiaryContent = template.Note;
            setHelperVariables();
        }
        private void DiaryKeyDown(object obj)
        {
            if ((Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) || Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift)) && Keyboard.IsKeyDown(System.Windows.Input.Key.Tab))
            {
                Helper.ClickShiftTabKey(Diary);
            }
            if (Keyboard.IsKeyDown(System.Windows.Input.Key.Space))
            {
                Helper.ClickSpace(Diary);
            }
        }
        private void ReturnKeySub(object obj)
        {
            Helper.ClickEnter(Diary);
        }
        private void TabKeySub(object obj)
        {
            Helper.ClickTabKey(Diary);
        }
        private void setHelperVariables()
        {
            Helper.GlobalStartTimeSpan = TimeSpan.Parse(StartTime);
            Helper.RestContent = RestContent;
            Helper.intervalRemindTimeSpan = new TimeSpan(0,int.Parse(IntervalMinutes), 0);
        }
        private async void Save(object obj)
        {
            Helper.SetAppSetting("StartTime", StartTime);
            Helper.SetAppSetting("RestContent", RestContent);
            Helper.SetAppSetting("OutputDirectory", OutputDirectory);
            Helper.SetAppSetting("ImportDirectory", ImportDirectory);
            Helper.SetAppSetting("WorkDirectory", WorkDirectory);
            Helper.SetAppSetting("IntervalMinutes", IntervalMinutes);
            SaveDiary();
            setHelperVariables();
            await showMessageBox("保存成功");
        }
        private async void SaveDiary()
        {
            Diary temp = new Diary()
            {
                Id = template.Id,
                Year = 0,
                Week = 0,
                Type = -1,
                Note = DiaryContent
            };
            await SQLCommands.SaveDiaryAsync(temp);
        }
        public async Task showMessageBox(string message)
        {
            var view = new SampleMessageDialog(message);
            await DialogHost.Show(view, "SubRootDialog");
        }
    }
}
