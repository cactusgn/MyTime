﻿using Summary.Models;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Summary
{
    /// <summary>
    /// Interaction logic for ColorTool.xaml
    /// </summary>
    public partial class ColorTool : UserControl
    {
        public ColorTool(MainModel mainModel)
        {
            InitializeComponent();
            DataContext = new ColorToolViewModel(mainModel);
        }
    }
}
