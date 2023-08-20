using Summary.Common.Utils;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Summary
{
    /// <summary>
    /// MinimizeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MinimizeWindow : Window
    {
        public MinimizeWindow(MainWindow mainWindow, MiniModel miniModel)
        {
            InitializeComponent();
            this.Topmost = true;
            this.DataContext = miniModel;
            
            UpperPanel.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
            btnClose.Click += (s, e) =>
            {
                this.Hide();
                Helper.MiniWindowShow = false;
                mainWindow.Topmost = false;
                mainWindow.Show();
            };
            btnMinimize.Click += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
            };
            this.Deactivated += (s, e) =>
            {
                this.Topmost = true;
            };
        }
        private void DialogHost2_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
        }

        private void DialogHost2_DialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, true))
                return;
            ((MiniModel)this.DataContext).recordModel.EndClick(null);
        }
    }
}
