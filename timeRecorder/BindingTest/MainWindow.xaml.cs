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
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace BindingTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            
            this.DataContext = new MainViewModel()
            {
                Name = "nora"
            };
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textbox1.Text = "aa" + slider.Value.ToString();
            textbox2.Text = slider.Value.ToString();
            textbox3.Text = slider.Value.ToString();
        }

        private void textbox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(textbox1.Text, out double result))
            {
                slider.Value = result;
            }
        }

        private void animationBtn_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.By=-40; //在原来的基础上减40
            //animation.From=animationBtn.Width;
            //animation.To = animationBtn.Width-40;
            //animation.Duration = TimeSpan.FromSeconds(2);
            animation.AutoReverse = true; //恢复
            animation.RepeatBehavior = RepeatBehavior.Forever; //是否反复执行或执行指定次数
            animationBtn.BeginAnimation(Button.WidthProperty, animation);
        }
    }
    internal class Test
    {
        public string? name { get; set; }
    }
}
