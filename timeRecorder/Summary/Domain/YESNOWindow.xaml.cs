﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Summary.Domain
{
    /// <summary>
    /// Interaction logic for RemindWindow.xaml
    /// </summary>
    public partial class YESNOWindow : Window
    {
        public YESNOWindow(string title="", string message = "")
        {
            InitializeComponent();
            this.DataContext = new
            {
                Title = title,
                Message = message,
            };
            TitleRow.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
