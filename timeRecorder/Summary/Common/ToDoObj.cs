using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Summary.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Common
{
    public class ToDoObj : ViewModelBase
    {
		private bool finished;
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}
        private TimeSpan lastTime;

        public TimeSpan LastTime
        {
            get { return lastTime; }
            set { lastTime = value; OnPropertyChanged(); }
        }
        public bool Finished
		{
			get { return finished; }
			set { finished = value;
                if (finished) TextDecorations = "Strikethrough";
                else TextDecorations = "None";
                OnPropertyChanged(); }
		}
		private string note;

		public string Note
		{
			get { return note; }
			set { note = value; OnPropertyChanged(); }
		}
		private string textDecorations;

        public string TextDecorations{
			  get{
				return textDecorations; 
              }
             set { textDecorations = value; OnPropertyChanged(); }
        }
		private string type;
		public string Type{
			get { return type; }
            set { type = value; OnPropertyChanged(); }
        }
		private DateTime createdDate;

		public DateTime CreatedDate
		{
			get { return createdDate; }
			set { createdDate = value; OnPropertyChanged(); }
		}
        public string CreatedDateString{
            get{
                return createdDate.ToString("yyyy-MM-dd");
            }
        }
        private int bonus;

        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; OnPropertyChanged(); }
        }
        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(); }
        }
        private int categoryId;

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; OnPropertyChanged(); }
        }

    }
}
