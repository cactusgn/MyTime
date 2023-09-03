﻿using MyToDo.ViewModels;
using MyToDo.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry?.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry?.RegisterForNavigation<AddMemoView, AdddMemoViewModel>();
            containerRegistry?.RegisterForNavigation<AboutView>();
            //containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry?.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry?.RegisterForNavigation<ToDoView, ToDoViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry?.RegisterForNavigation<SettingsView, SettingsViewModel>();
        }
    }
}
