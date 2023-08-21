using Summary.Common.Utils;
using Summary.Domain;
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
        System.Timers.Timer showTextBoxTimer = new System.Timers.Timer(); //新建一个Timer对象
        public RecordModel recordModel { get; set; }
        public MiniModel MiniModel { get; set; }    
        private bool DialogIsShown{ get;set; }
        public MinimizeWindow(MainWindow mainWindow, MiniModel miniModel, RecordModel rm)
        {
            InitializeComponent();
            recordModel = rm;
            this.DataContext = miniModel;
            MiniModel = miniModel;
            this.Topmost = true;
            
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
            
            showTextBoxTimer.Interval = 1000;//设定多少秒后行动，单位是毫秒
            showTextBoxTimer.Elapsed += new ElapsedEventHandler(showTextBoxTimer_Tick);//到时所有执行的动作
            showTextBoxTimer.Start();
        }
        
        private void showTextBoxTimer_Tick(object sender, EventArgs e)
        {
            var model = MiniModel;
            model.TickTime = Helper.TickTime;
            if (Helper.WorkMode)
            {
                model.ToggleIcon = "Pause";

                if (recordModel.CalculatedRemindTime >= new TimeSpan(0, recordModel.Interval, 0) && Helper.MiniWindowShow)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(delegate {
                            YESNOWindow YesNoDialog = new YESNOWindow("心态好最重要呀", "已经工作好一会了，休息一下眼睛更好哦");
                            YesNoDialog.Owner = this;
                            if(!DialogIsShown){
                                DialogIsShown = true;
                                recordModel.CalculatedRemindTime = new TimeSpan();
                                if (YesNoDialog.ShowDialog() == false)
                                {
                                    recordModel.EndClick(null);
                                    model.ToggleIcon = "Play";
                                    recordModel.EndbtnEnabled = false;
                                    recordModel.StartbtnEnabled = true;
                                    model.WorkContent = Helper.GetAppSetting("Slogan");
                                    DialogIsShown = false;
                                }else{
                                    DialogIsShown = false;
                                }
                            }
                            
                        }
                    ));
                }
            }
            else
            {
                model.ToggleIcon = "Play";
            }
        }

        private async void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            var model = (MiniModel)(this.DataContext);
            if (model.ToggleIcon == "Play")
            {
                if (recordModel.WorkContent == null || recordModel.WorkContent == "")
                {
                    await recordModel.showMessageBox("请先填写工作内容");
                    return;
                }
                await recordModel.StartClickMethod();
                model.ToggleIcon = "Pause";
                recordModel.EndbtnEnabled = true;
                recordModel.StartbtnEnabled = false;
                model.WorkContent = Helper.WorkContent;
            }
            else
            {
                recordModel.EndClick(null);
                model.ToggleIcon = "Play";
                recordModel.EndbtnEnabled = false;
                recordModel.StartbtnEnabled = true;
                model.WorkContent = Helper.GetAppSetting("Slogan");
            }
        }
    }
}
