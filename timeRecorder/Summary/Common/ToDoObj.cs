using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
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

    }
}
