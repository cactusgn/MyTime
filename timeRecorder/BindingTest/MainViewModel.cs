using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BindingTest
{
    public class MainViewModel:ViewModelBase
    {
        public MainViewModel()
        {
            ShowCommand = new MyCommand(Show);
        }
        public MyCommand ShowCommand { get; set; }
        private string name;
        public string? Name { 
            get 
            { 
                return name; 
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }


        public void Show()
        {
            Name="changed nora";
            MessageBox.Show("点击了按钮");
        }
    }
}
