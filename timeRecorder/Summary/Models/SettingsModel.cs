using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SettingsModel() {
            SaveCommand = new MyCommand(Save);
            StartTime = Helper.GetAppSetting("StartTime");
            RestContent = Helper.GetAppSetting("RestContent");
            OutputDirectory = Helper.GetAppSetting("OutputDirectory");
            ImportDirectory = Helper.GetAppSetting("ImportDirectory");
            IntervalMinutes = Helper.GetAppSetting("IntervalMinutes");
            setHelperVariables();
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
            Helper.SetAppSetting("IntervalMinutes", IntervalMinutes);
            setHelperVariables();
            await showMessageBox("保存成功");
        }
        public async Task showMessageBox(string message)
        {
            var view = new SampleMessageDialog(message);
            await DialogHost.Show(view, "SubRootDialog");
        }
    }
}
