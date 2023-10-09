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
       
    }
}
