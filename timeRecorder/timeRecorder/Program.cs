using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace timeRecorder
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingleInstanceManager manager = new SingleInstanceManager();//单实例管理器
            manager.Run(new string[] { });
            //Application.Run(new Form1());
        }

        // Using VB bits to detect single instances and process accordingly:
        // * OnStartup is fired when the first instance loads
        // * OnStartupNextInstance is fired when the application is re-run again
        //    NOTE: it is redirected to this instance thanks to IsSingleInstance
        public class SingleInstanceManager : WindowsFormsApplicationBase
        {
            Form1 app;

            public SingleInstanceManager()
            {
                this.IsSingleInstance = true;
            }

            protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
            {
                // First time app is launched
                //app = new SingleInstanceApplication();
                //app.Run();
                app = new Form1();//改为自己的程序运行窗体
                Application.Run(app);

                return false;
            }

            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                // Subsequent launches

                base.OnStartupNextInstance(eventArgs);
                app.Activate();
            }
        }
    }
}
