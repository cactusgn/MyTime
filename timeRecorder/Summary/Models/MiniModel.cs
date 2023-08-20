using MaterialDesignThemes.Wpf;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace Summary.Models
{
    public class MiniModel:ViewModelBase
    {
        private bool DialogIsShown;
        private TimeSpan tickTime;

        public TimeSpan TickTime
        {
            get { return tickTime; }
            set { tickTime = value; OnPropertyChanged(); }
        }
        private string workContent;

        public string WorkContent
        {
            get { return workContent; }
            set { workContent = value; OnPropertyChanged(); }
        }
        private string toggleIcon;

        public string ToggleIcon
        {
            get { return toggleIcon; }
            set { toggleIcon = value; OnPropertyChanged(); }
        }
        public MyCommand ToggleBtnCommand { get; set; }
        public RecordModel recordModel { get; set; }
        public MiniModel( RecordModel rm) {
            System.Timers.Timer showTextBoxTimer = new System.Timers.Timer(); //新建一个Timer对象
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Elapsed += new ElapsedEventHandler(showTextBoxTimer_Tick);//到时所有执行的动作
            showTextBoxTimer.Start();//启动计时
            ToggleBtnCommand = new MyCommand(ToggleBtnClick);
            recordModel = rm;
        }
        private async void ToggleBtnClick(object obj)
        {
            if(ToggleIcon == "Play")
            {
                bool res = await recordModel.StartClickMethod();
                if (!res)
                {
                    return;
                }
                ToggleIcon = "Pause";
                recordModel.EndbtnEnabled = true;
                recordModel.StartbtnEnabled = false;
                WorkContent = Helper.WorkContent;
            }
            else
            {
                recordModel.EndClick(null);
                ToggleIcon = "Play";
                recordModel.EndbtnEnabled = false;
                recordModel.StartbtnEnabled = true;
                WorkContent = Helper.GetAppSetting("Slogan");
            }
        }

        private void showTextBoxTimer_Tick(object sender, EventArgs e)
        {
            TickTime = Helper.TickTime;
            if (Helper.WorkMode)
            {
                ToggleIcon = "Pause";
                
                if (recordModel.CalculatedRemindTime >= new TimeSpan(0, recordModel.Interval, 0)&& Helper.MiniWindowShow)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                    {
                        MessageBoxResult result = MessageBox.Show("可以喝杯水休息一下眼睛啦，准备休息吗？", "心态好最重要呀", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.ServiceNotification);
                        if (result == MessageBoxResult.Yes)
                        {
                            recordModel.EndClick(null);
                            ToggleIcon = "Play";
                            recordModel.EndbtnEnabled = false;
                            recordModel.StartbtnEnabled = true;
                            WorkContent = Helper.GetAppSetting("Slogan");
                        }
                    }));

                }
            }
            else
            {
                ToggleIcon = "Play";
            }
        }
        //public async void showRemindDialog()
        //{
        //    if (!DialogIsShown)
        //    {
        //        DialogIsShown = true;
        //        var view = new RemindDialog();
                
        //        await DialogHost.Show(view, "MiniRootDialog");
        //        DialogIsShown = false;
        //    }
        //}
    }
}
