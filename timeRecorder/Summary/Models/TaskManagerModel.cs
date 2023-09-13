﻿using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using Summary.Common;
using Summary.Common.Utils;
using Summary.Data;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Summary.Models
{
    public class TaskManagerModel : ViewModelBase
    {
        public TreeView RootTreeView { get; set; }
        public ContextMenu CategoryContextMenu{ get; set; }

        public ISQLCommands SQLCommands { get; set; }
        
        public MyCommand TreeViewSelectedItemChangedCommand{ get; set; }
        public MyCommand AddCategoryCommand { get; set; }
        public MyCommand EditCategoryCommand { get; set; }
        public MyCommand DeleteCategoryCommand { get; set; }
        public AddCategoryModel CategoryModel { get; set; }
        private string contextVisible = "Visible";
        
        public string ContextVisible
        {
            get { return contextVisible; }
            set { contextVisible = value; OnPropertyChanged(); }
        }
        private UIElement mainContent;
        public UIElement MainContent
        {
            get { return mainContent; }
            set { mainContent = value; OnPropertyChanged(); }
        }
        private QueryTaskModel queryTaskModel;
        public TaskManagerModel(ISQLCommands SqlCommands, AddCategoryModel categoryModel) {
            SQLCommands = SqlCommands;
            TreeViewSelectedItemChangedCommand = new MyCommand(TreeViewSelectedItemChanged);
            AddCategoryCommand = new MyCommand(AddCategoryClick);
            EditCategoryCommand = new MyCommand(EditCategoryClick);
            DeleteCategoryCommand = new MyCommand(DeleteCategoryClick);
            CategoryModel = categoryModel;
            queryTaskModel =  new QueryTaskModel("", DateTime.Today.AddDays(-6), DateTime.Today, SqlCommands);
            MainContent = new QueryTaskUserControl(queryTaskModel);
        }

        private async void DeleteCategoryClick(object obj)
        {
            List<Category> AllCategories = await SQLCommands.GetAllCategories();
            MenuItem root = (MenuItem)RootTreeView.SelectedItem;
            YESNOWindow dialog = new YESNOWindow("提示", "确定删除类别" + root.Title +"及其子类别吗", "确定", "取消");
            if (dialog.ShowDialog() == true)
            {
                queryTaskModel.RestoreDeleteCategoryToParentCategory(root.Id, AllCategories);
                await SQLCommands.DeleteCategory(root.Id);
               
            }
            RefreshCategories();
        }

        private void EditCategoryClick(object obj)
        {
            MenuItem root = (MenuItem)RootTreeView.SelectedItem;
            showCategoryDialog("修改类别", root.Id, root.Title, root.Color, root.ParentId,root.Bonus);
        }

        public async void showCategoryDialog(string title,int id, string category, string color, int parentId, int bonus=20)
        {
            CategoryModel.Title = title;
            CategoryModel.Category = category;
            CategoryModel.SelectedColor = color;
            CategoryModel.Bonus = bonus;
            CategoryModel.Id = id;
            CategoryModel.ParentId = parentId;
            CategoryModel.NoCaption = "取消";
            CategoryModel.YesCaption = "确定";
            var view = new AddCategoryDialog(CategoryModel);
            await DialogHost.Show(view, "SubRootDialog");
        }
        private void AddCategoryClick(object obj)
        {
            MenuItem root = (MenuItem)RootTreeView.SelectedItem;
            showCategoryDialog("增加子类别",0,"",root.Color, root.Id, root.Bonus);
        }

        private void TreeViewSelectedItemChanged(object obj)
        {
            if (RootTreeView.SelectedItem != null){
                MenuItem root = (MenuItem)RootTreeView.SelectedItem;
                queryTaskModel.Category = root.Title=="任务类别："?"": root.Title;
            }
        }
        public void initNode(List<Category> Categories, MenuItem currentNode)
        {
            var subCategories = Categories.Where(x=>x.ParentCategoryId == currentNode.Id).ToList();
            foreach(var category in subCategories) {
                if(category.Visible){
                    MenuItem child = new MenuItem() { Title = category.Name, Id = category.Id, Color = category.Color, ParentId = category.ParentCategoryId};
                    initNode(Categories, child);
                    currentNode.Items.Add(child);
                }
            }
        }
        public async Task showMessageBox(string message)
        {
            var view = new SampleMessageDialog(message);
            await DialogHost.Show(view, "SubRootDialog");
        }
        public async void RefreshCategories()
        {
            List<Category> AllCategories =  await SQLCommands.GetAllCategories();
            MenuItem root = new MenuItem() { Title="任务类别：",Id=0};
            initNode(AllCategories, root);
            RootTreeView.Items.Clear();
            RootTreeView.Items.Add(root);
            
            queryTaskModel.UpdateContextMenu();
        }

        public async void addCategory(AddCategoryModel categoryModel){
            if(CategoryExist(categoryModel.Category).Result){
                await showMessageBox("已存在该类型，请重新输入名称");
                return;
            }
            await SQLCommands.AddCategory(categoryModel);
            RefreshCategories();
        }

        private async Task<bool> CategoryExist(string category)
        {
            List<Category> AllCategories = await SQLCommands.GetAllCategories();
            foreach (var item in AllCategories)
            {
                if(item.Name==category) return true;
            }
            return false;
        }

        public async void EditCategory(AddCategoryModel category)
        {
            if (CategoryExist(category.Category).Result)
            {
                await showMessageBox("已存在该类型，请重新输入名称");
                return;
            }
            await SQLCommands.UpdateCategory(category);
            MenuItem root = (MenuItem)RootTreeView.SelectedItem;
            root.Title = category.Category;
            root.Color = category.SelectedColor;
            root.Bonus = category.Bonus;
            queryTaskModel.UpdateContextMenu();
        }
    }
    public class MenuItem : ViewModelBase
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        private string color;

        public string Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged(); }
        }
        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        private int bonus;

        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
