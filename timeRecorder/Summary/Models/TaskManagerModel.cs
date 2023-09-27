using MaterialDesignDemo.Domain;
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
        public MenuItem ShowInvisibleCateContextMenu { get; set; }
        public ISQLCommands SQLCommands { get; set; }
        
        public MyCommand TreeViewSelectedItemChangedCommand{ get; set; }
        public MyCommand AddCategoryCommand { get; set; }
        public MyCommand EditCategoryCommand { get; set; }
        public MyCommand DeleteCategoryCommand { get; set; }
        public AddCategoryModel CategoryModel { get; set; }
        public MyCommand ShowInvisibleCategoryCommand { get; set; }
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
        
        private string showVisibleHeader;

        public string ShowVisibleHeader
        {
            get { return showVisibleHeader; }
            set { showVisibleHeader = value; OnPropertyChanged(); }
        }

        public QueryTaskModel queryTaskModel;
        private List<Category> AllCategories;
        public TaskManagerModel(ISQLCommands SqlCommands, AddCategoryModel categoryModel) {
            SQLCommands = SqlCommands;
            TreeViewSelectedItemChangedCommand = new MyCommand(TreeViewSelectedItemChanged);
            AddCategoryCommand = new MyCommand(AddCategoryClick);
            EditCategoryCommand = new MyCommand(EditCategoryClick);
            DeleteCategoryCommand = new MyCommand(DeleteCategoryClick);
            ShowInvisibleCategoryCommand = new MyCommand(ShowInvisibleCategoryClick);
            CategoryModel = categoryModel;
            queryTaskModel =  new QueryTaskModel("", DateTime.Today.AddDays(-6), DateTime.Today, SqlCommands);
            MainContent = new QueryTaskUserControl(queryTaskModel);
            queryTaskModel.GetSummaryDate();
            ShowVisibleHeader = "显示隐藏类别";
        }

        private void ShowInvisibleCategoryClick(object obj)
        {
            if (ShowVisibleHeader == "显示隐藏类别") {
                ShowVisibleHeader = "不显示隐藏类别";
                queryTaskModel.displayInvisibleItems = true;
            }
            else
            {
                ShowVisibleHeader = "显示隐藏类别";
                queryTaskModel.displayInvisibleItems = false;
            }
            RefreshCategories();
            Helper.initColor(SQLCommands);
        }

        private async void DeleteCategoryClick(object obj)
        {
            AllCategories = await SQLCommands.GetAllCategories();
            MenuItemModel root = (MenuItemModel)RootTreeView.SelectedItem;
            YESNOWindow dialog = new YESNOWindow("提示", "确定删除类别" + root.Title +"及其子类别吗", "确定", "取消");
            if (dialog.ShowDialog() == true)
            {
                queryTaskModel.RestoreDeleteCategoryToParentCategory(root.Id, AllCategories);
                await SQLCommands.DeleteCategory(root.Id);
               
            }
            RefreshCategories();
            Helper.initColor(SQLCommands);
        }

        private void EditCategoryClick(object obj)
        {
            MenuItemModel root = (MenuItemModel)RootTreeView.SelectedItem;
            showCategoryDialog("修改类别", root.Id, root.Title, root.Color, root.ParentId,root.VisibleValue,root.Bonus,root.AutoCreateTask);
        }

        public async void showCategoryDialog(string title,int id, string category, string color, int parentId, bool visible, int bonus=20, bool autoCreateTask=true)
        {
            CategoryModel.Title = title;
            CategoryModel.Category = category;
            CategoryModel.SelectedColor = color;
            CategoryModel.Bonus = bonus;
            CategoryModel.Id = id;
            CategoryModel.ParentId = parentId;
            CategoryModel.NoCaption = "取消";
            CategoryModel.YesCaption = "确定";
            CategoryModel.ShowInvalidCateMessage = "Collapsed";
            CategoryModel.ParentCategoryList = await getCategorySVs(id);
            CategoryModel.AutoCreateTask = autoCreateTask;
            CategoryModel.Visible = visible;
            var view = new AddCategoryDialog(CategoryModel);
            await DialogHost.Show(view, "SubRootDialog");
        }
        public async Task<ObservableCollection<ParentCategorySV>> getCategorySVs(int exceptId)
        {
            AllCategories = await SQLCommands.GetAllCategories();
            ObservableCollection<ParentCategorySV> parentCategorySVs = new ObservableCollection<ParentCategorySV>();
            ParentCategorySV parentCategorySV = null;
            foreach (var category in AllCategories)
            {
                if (category.Id == exceptId) continue;
                parentCategorySV = new ParentCategorySV() { 
                    ParentCategoryId = category.Id,
                    ParentCategoryName = category.Name
                };
                parentCategorySVs.Add(parentCategorySV);
            }
            return parentCategorySVs;
        }
        private void AddCategoryClick(object obj)
        {
            MenuItemModel root = (MenuItemModel)RootTreeView.SelectedItem;
            showCategoryDialog("增加子类别",0,"",root.Color, root.Id, true, root.Bonus,root.AutoCreateTask);
        }

        private void TreeViewSelectedItemChanged(object obj)
        {
            if (RootTreeView.SelectedItem != null){
                MenuItemModel root = (MenuItemModel)RootTreeView.SelectedItem;
                queryTaskModel.Category = root.Title=="任务类别："?"": root.Title;
            }
        }
        public void initNode(List<Category> Categories, MenuItemModel currentNode)
        {
            var subCategories = Categories.Where(x=>x.ParentCategoryId == currentNode.Id).ToList();
            foreach(var category in subCategories) {
                //即初始状态，不显示隐藏类别
                if(ShowVisibleHeader == "显示隐藏类别")
                {
                    if (category.Visible)
                    {
                        MenuItemModel child = new MenuItemModel() { 
                            Title = category.Name, 
                            Id = category.Id, 
                            Color = category.Color, 
                            ParentId = category.ParentCategoryId, 
                            Bonus = category.BonusPerHour,
                            AutoCreateTask=category.AutoAddTask,
                            VisibleValue= category.Visible
                        };
                        initNode(Categories, child);
                        currentNode.Items.Add(child);
                    }
                }
                else
                {
                    MenuItemModel child = new MenuItemModel() { 
                        Title = category.Name, 
                        Id = category.Id, 
                        Color = category.Color, 
                        ParentId = category.ParentCategoryId,
                        Bonus = category.BonusPerHour,
                        AutoCreateTask =category.AutoAddTask,
                        VisibleValue= category.Visible
                    };
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
            AllCategories =  await SQLCommands.GetAllCategories();
            MenuItemModel root = new MenuItemModel() { Title="任务类别：",Id=0, IsSelected=true};
            initNode(AllCategories, root);
            RootTreeView.Items.Clear();
            RootTreeView.Items.Add(root);
            queryTaskModel.UpdateContextMenu();
        }

        public async void addCategory(AddCategoryModel categoryModel){
            categoryModel.ShowInvalidCateMessage="Collapsed";
            
            await SQLCommands.AddCategory(categoryModel);
            RefreshCategories();
            Helper.initColor(SQLCommands);
        }

        public async Task<bool> CategoryExist(string category)
        {
            AllCategories = await SQLCommands.GetAllCategories();
            foreach (var item in AllCategories)
            {
                if(item.Name==category) return true;
            }
            return false;
        }
        public async Task<bool> EditCheck(AddCategoryModel category)
        {
            AllCategories = await SQLCommands.GetAllCategories();
            if (AllCategories.Single(x => x.Id == category.Id).Name!=category.Category)
            {
                if (CategoryExist(category.Category).Result)
                {
                    category.ShowInvalidCateMessage="Visible";
                    return false;
                }
            }
            return true;
        }
        public async void EditCategory(AddCategoryModel category)
        {
            await SQLCommands.UpdateCategory(category);
            MenuItemModel root = (MenuItemModel)RootTreeView.SelectedItem;
            string oldVisibleValue = root.Visible;
            root.Title = category.Category;
            root.Color = category.SelectedColor;
            root.Bonus = category.Bonus;
            root.AutoCreateTask = category.AutoCreateTask;
            root.Visible = category.Visible==false&&ShowVisibleHeader == "显示隐藏类别" ? "Collapsed":"Visible";
            root.VisibleValue = category.Visible;
            queryTaskModel.UpdateContextMenu();
            if(root.ParentId != category.ParentId||root.Visible!=oldVisibleValue)
            {
                RefreshCategories();
            }
            root.ParentId = category.ParentId;
            Helper.initColor(SQLCommands);
        }
    }
   
    public class MenuItemModel : ViewModelBase
    {
        public MenuItemModel()
        {
            this.Items = new ObservableCollection<MenuItemModel>();
        }
        private bool visibleValue;

        public bool VisibleValue
        {
            get { return visibleValue; }
            set { visibleValue = value; OnPropertyChanged(); }
        }

        private string visible= "Visible";

        public string Visible
        {
            get { return visible; }
            set { visible = value; OnPropertyChanged(); }
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
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }
        private bool autoCreateTask;

        public bool AutoCreateTask
        {
            get { return autoCreateTask; }
            set { autoCreateTask = value; OnPropertyChanged(); }
        }

        public ObservableCollection<MenuItemModel> Items { get; set; }
    }
}
