using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summary.Data
{
    public class CategoryTheme
    {
        public string Name { get; set; }
        public int Level {  get; set; }
        public bool Checked { get; set; }
        public string Margin {
            get
            {
                return $"{Level*20 - 10},0,8,0";
            }
        }
    }
}
