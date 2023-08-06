using System;
using System.Collections.Generic;


namespace Summary.Common
{
    public class TimeViewObj  : ViewModelBase
    {
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }
        private double height;
        private string note;
        private string type;
        private string timeNote;
        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
                OnPropertyChanged();
            }
        }
        public string TimeNote { 
            get{
                timeNote = LastTime + "\n" + Note;
                return timeNote;
            }
            set{
                timeNote = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan LastTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type{
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged();
            }
        }
        private string color;
        public string Color {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }
        public int Id{ get; set; }
    }
    public enum TimeType
    {
        None,
        Study,
        Waste,
        Rest,
        Work
    }
    
}
