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
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        public MainWindow(MainModel mainModel)
        {
            InitializeComponent();
            this.DataContext = mainModel;
            

            ITheme theme = _paletteHelper.GetTheme();
            theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString("#2884D5"));
            _paletteHelper.SetTheme(theme);
        }

    }
}
