using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using Summary.Common;
using Summary.Data;
using Summary.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public AddCategoryModel CategoryModel { get; set; }
        private string contextVisible = "Visible";
        
        public string ContextVisible
        {
            get { return contextVisible; }
            set { contextVisible = value; OnPropertyChanged(); }
        }
        public TaskManagerModel(ISQLCommands SqlCommands, AddCategoryModel categoryModel) {
            SQLCommands = SqlCommands;
            TreeViewSelectedItemChangedCommand = new MyCommand(TreeViewSelectedItemChanged);
            AddCategoryCommand = new MyCommand(AddCategory);
            CategoryModel = categoryModel;
        }
        public async void showCategoryDialog(string title, string category, string color, int bonus=20)
        {
            CategoryModel.Title = title;
            CategoryModel.Category = category;
            CategoryModel.SelectedColor = color;
            CategoryModel.Bonus = bonus;
            CategoryModel.NoCaption = "取消";
            CategoryModel.YesCaption = "确定";
            var view = new AddCategoryDialog(CategoryModel);
            await DialogHost.Show(view, "SubRootDialog");
        }
        private void AddCategory(object obj)
        {
            MenuItem root = (MenuItem)RootTreeView.SelectedItem;
            showCategoryDialog("增加类别",root.Id,root.Color);
        }

        private void TreeViewSelectedItemChanged(object obj)
        {
            if (RootTreeView.SelectedItem != null){
                MenuItem root = (MenuItem)RootTreeView.SelectedItem;
                if (root != null)
                {
                    if (root.Title == "Invest" || root.Title == "Work" || root.Title == "Play")
                    {
                        ContextVisible = "Hidden";
                    }
                    else
                    {
                        ContextVisible = "Visible";
                    }
                }
            }
        }

        public async void Init()
        {
            List<Category> AllCategories =  await SQLCommands.GetAllCategories();
            MenuItem root = new MenuItem() { Title="任务类别："};
            MenuItem InvestRoot = new MenuItem() { Title = "Invest", Color = "#FFB6C1"};
            MenuItem child1 = new MenuItem() { Title="Invest1" };
            MenuItem child2 = new MenuItem() { Title="Invest2" };
            InvestRoot.Items.Add(child1);
            InvestRoot.Items.Add(child2);
            MenuItem WorkRoot = new MenuItem() { Title = "Work", Color = "#FFD700" };
            MenuItem Work1 = new MenuItem() { Title="Work1" };
            MenuItem Work2 = new MenuItem() { Title="Work2" };
            WorkRoot.Items.Add(Work1);
            WorkRoot.Items.Add(Work2);
            MenuItem playRoot = new MenuItem() { Title = "Play", Color = "#ADD8E6" };
            root.Items.Add(InvestRoot);
            root.Items.Add(WorkRoot);
            root.Items.Add(playRoot);
            RootTreeView.Items.Add(root);
            
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
        

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
