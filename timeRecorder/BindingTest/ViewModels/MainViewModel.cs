using BindingTest.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BindingTest.ViewModels
{
    public class MainViewModel:BindableBase
    {
        public DelegateCommand<string> OpenCommand { get; private set; }
        public MainViewModel(IRegionManager regionManager)
        {
            OpenCommand = new DelegateCommand<string>(Open);
            RegionManager=regionManager;
        }
        public IRegionManager RegionManager;
        private void Open(string obj)
        {
            RegionManager.Regions["ContentRegion"].RequestNavigate(obj);
        }

        //private object body;
        //public object Body
        //{
        //    get { return body; }
        //    set { body = value;RaisePropertyChanged(); }
        //}



        //private void Open(string obj)
        //{
        //    switch (obj)
        //    {
        //        case "ViewA":
        //            Body = new ViewA();
        //            break;
        //        case "ViewB":
        //            Body = new ViewB();
        //            break;
        //    }
        //}
    }
}
