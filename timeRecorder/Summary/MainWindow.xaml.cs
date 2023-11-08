using MaterialDesignThemes.Wpf;
using Summary.Common.Utils;
using Summary.Data;
using Summary.Domain;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.InteropServices;
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
using System.Windows.Interop;

namespace Summary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double normalTop = 0;
        double normalLeft = 0;
        public MainWindow(MainModel mainModel)
        {
            InitializeComponent();
            this.DataContext = mainModel;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MiniModel miniModel = new MiniModel((RecordModel)(mainModel.RecordPageUserControl.DataContext));
            MinimizeWindow minimizeWindow = new MinimizeWindow(this, miniModel, (RecordModel)(mainModel.RecordPageUserControl.DataContext));
            
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) => {
                if (this.WindowState == WindowState.Maximized)
                {
                    Restore(this);
                    //this.WindowState = WindowState.Normal;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                }
                else
                {
                   Maximize(this);
                    //this.WindowState = WindowState.Maximized;
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                    
                }
            };
            btnClose.Click += (s, e) => { this.Close(); minimizeWindow.Close(); };
            ColorZone.MouseMove += (s, e) => {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        this.WindowState = WindowState.Normal;
                        btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                    }
                    
                    this.DragMove();
                }
                else
                {
                    if (this.WindowState == WindowState.Normal&&this.Left!=0&&this.Top!=0)
                    {
                        normalLeft = this.Left;
                        normalTop = this.Top;
                    }
                }
            };
            btnLittleWin.Click+=(s, e) =>
            {
                this.Hide();
                Helper.MiniWindowShow = true;
                
                if (Helper.WorkMode)
                {
                    ((MiniModel)minimizeWindow.DataContext).ToggleIcon = "Pause";
                    ((MiniModel)minimizeWindow.DataContext).WorkContent = Helper.WorkContent;
                }
                else
                {
                    ((MiniModel)minimizeWindow.DataContext).ToggleIcon = "Play";
                    ((MiniModel)minimizeWindow.DataContext).WorkContent = Helper.GetAppSetting("Slogan");
                }
                var model = ((MiniModel)minimizeWindow.DataContext);
                model.WorkFontSize = 16;
                while (model.WorkFontSize>0&& Helper.getTextSize(model.WorkContent,model.WorkFontSize)>210)
                {
                    model.WorkFontSize-=1;
                }
                minimizeWindow.Show();
            };
            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                {
                    Maximize(this);
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                }
                else
                {
                    Restore(this);
                    btnMaxIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                   
                }
                    
            };
            
        }

       
        // Rectangle (used by MONITORINFOEX)
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Monitor information (used by GetMonitorInfo())
        [StructLayout(LayoutKind.Sequential)]
        public class MonitorInfoEx {
            public int cbSize;
            public Rect rcMonitor; // Total area
            public Rect rcWork; // Working area
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public char[] szDevice;
        }
                // To get a handle to the specified monitor
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // To get the working area of the specified monitor
        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MonitorInfoEx monitorInfo);

        private static MonitorInfoEx GetMonitorInfo(Window window, IntPtr monitorPtr) {
            var monitorInfo = new MonitorInfoEx();

            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            GetMonitorInfo(new HandleRef(window, monitorPtr), monitorInfo);

            return monitorInfo;
        }

        private static void Minimize(Window window) {
             if(window == null) {
                 return;
             }

             window.WindowState = WindowState.Minimized;
         }

         private void Restore(Window window) {
            if(window == null) {
                return;
            }

            window.WindowState = WindowState.Normal;
            window.ResizeMode = ResizeMode.CanResizeWithGrip;


            this.Top = normalTop;
            this.Left = normalLeft;
        }

        private static void Maximize(Window window) {
            window.ResizeMode = ResizeMode.NoResize;

            // Get handle for nearest monitor to this window
            var wih = new WindowInteropHelper(window);

            // Nearest monitor to window
            const int MONITOR_DEFAULTTONEAREST = 2;
            var hMonitor = MonitorFromWindow(wih.Handle, MONITOR_DEFAULTTONEAREST);

            // Get monitor info
            var monitorInfo = GetMonitorInfo(window, hMonitor);

            // Create working area dimensions, converted to DPI-independent values
            var source = HwndSource.FromHwnd(wih.Handle);

            if(source?.CompositionTarget == null) {
                return;
            }

            var matrix = source.CompositionTarget.TransformFromDevice;
            var workingArea = monitorInfo.rcWork;

            var dpiIndependentSize =
                matrix.Transform(
                    new Point(workingArea.Right - workingArea.Left,
                                workingArea.Bottom - workingArea.Top));



            // Maximize the window to the device-independent working area ie
            // the area without the taskbar.
            window.Top = workingArea.Top;
            window.Left = workingArea.Left;

            window.MaxWidth = dpiIndependentSize.X;
            window.MaxHeight = dpiIndependentSize.Y;

            window.WindowState = WindowState.Maximized;
        }
    }
}
