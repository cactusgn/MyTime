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
        private int workFontSize = 16;

        public int WorkFontSize
        {
            get { return workFontSize; }
            set { workFontSize = value; OnPropertyChanged(); }
        }

        private string toggleIcon;

        public string ToggleIcon
        {
            get { return toggleIcon; }
            set { toggleIcon = value; OnPropertyChanged(); }
        }
        public MyCommand ToggleBtnCommand { get; set; }
        public RecordModel recordModel { get; set; }
        public YESNOWindow YesNoDialog { get; set; }
        public MiniModel( RecordModel rm) {
            recordModel = rm;
        }
       
        public bool openDialog = false;
        //private void showTextBoxTimer_Tick(object sender, EventArgs e)
        //{
        //    TickTime = Helper.TickTime;
        //    if (Helper.WorkMode)
        //    {
        //        ToggleIcon = "Pause";
                
        //        if (recordModel.CalculatedRemindTime >= new TimeSpan(0, 0, 5)&& Helper.MiniWindowShow)
        //        {
        //            Application.Current.Dispatcher.BeginInvoke(new Action(delegate{ 
        //                if (!openDialog){
        //                    YesNoDialog = new YESNOWindow("心态好最重要呀", "已经工作好一会了，休息一下眼睛更好哦", "继续", "休息");
        //                    openDialog = true;
        //                    if (YesNoDialog.ShowDialog() == true)
        //                    {
        //                        recordModel.EndClick(null);
        //                        ToggleIcon = "Play";
        //                        recordModel.EndbtnEnabled = false;
        //                        recordModel.StartbtnEnabled = true;
        //                        WorkContent = Helper.GetAppSetting("Slogan");
        //                        while (WorkFontSize>0&& WorkContent.Length*WorkFontSize>220)
        //                        {
        //                            WorkFontSize-=1;
        //                        }
        //                    }
        //                    openDialog = false;
        //                }
        //            }));
        //        }
        //    }
        //    else
        //    {
        //        ToggleIcon = "Play";
        //        WorkFontSize = 16;
        //    }
        //}
       
    }
}
