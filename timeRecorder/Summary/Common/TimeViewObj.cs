using System;
using System.Collections.Generic;


namespace Summary.Common
{
    public class TimeViewObj  : ViewModelBase
    {
        public double Height
        {
            get;set;
        }
        private string note;
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
                return LastTime + "\n" + Note;
            }
            set{
                
            }
        }
        public TimeSpan LastTime { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type{ get; set; }
        public string Color { get; set; }
        public string Id{ get; set; }
    }
    
}
