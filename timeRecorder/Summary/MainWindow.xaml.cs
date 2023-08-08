using MaterialDesignThemes.Wpf;
using Summary.Data;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Summary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainModel mainModel)
        {
            InitializeComponent();
            
            this.DataContext = mainModel;
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) => {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                }
            };
            btnClose.Click += (s, e) => { this.Close(); };
            ColorZone.MouseMove += (s, e) => {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                    
            };
            
        }
     
        
    }
}
