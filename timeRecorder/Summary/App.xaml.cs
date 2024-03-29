﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Summary.Data;
using Summary.Domain;
using Summary.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Summary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(ConfiguratioHostServices).Build();
            using (var db = new MytimeContext())
            {
                try
                {
                    using(var context = new MytimeContext())
                    {
                        //是否有新迁移
                        if(context.Database.GetPendingMigrations().Any()){
                            //EnsureCreated和Migrate的区别是EnsureCreated在数据库不存在时也会自动创建，但如果创建后数据库有更改，则不会更新到数据库中
                            context.Database.EnsureCreated();
                            context.Database.Migrate();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
            // throw new NotImplementedException();
        }
        private void ConfiguratioHostServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            //这个根据自己需要注入
            services.AddSingleton<ISQLCommands, SQLServerCommand>();
            services.AddSingleton<MainModel>();
            services.AddSingleton<MiniModel>();
            services.AddSingleton<SummaryModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<RecordModel>();
            services.AddSingleton<TaskManagerModel>();
            services.AddSingleton<SampleDialogViewModel>();
            services.AddSingleton<AddCategoryModel>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host?.Start();
            MainWindow? window = _host?.Services.GetRequiredService<MainWindow>();
            window?.Show();
            base.OnStartup(e);
        }
        
        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (_host != null)
            {
                await _host.StopAsync();
            }
            _host?.Dispose();
        }


    }
}
