using System;

namespace Summary.Domain
{
    public class SampleDialogViewModel : ViewModelBase
    {
        private TimeSpan startTime;

        public TimeSpan StartTime
        {
            get => startTime;
            set { startTime = value; OnPropertyChanged(); }
        }
        private TimeSpan endTime;

        public TimeSpan EndTime
        {
            get => endTime;
            set { endTime = value; OnPropertyChanged(); }
        }
        private TimeSpan splitTime;

        public TimeSpan SplitTime
        {
            get => splitTime;
            set { splitTime = value; OnPropertyChanged(); }
        }
        private string content1;

        public string Content1
        {
            get { return content1; }
            set { content1 = value; OnPropertyChanged(); }
        }
        private string content2;

        public string Content2
        {
            get { return content2; }
            set { content2 = value; OnPropertyChanged(); }
        }

        public SampleDialogViewModel(){

        }
    }
}
