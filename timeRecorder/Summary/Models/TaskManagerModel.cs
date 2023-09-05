using Microsoft.Data.SqlClient;
using Summary.Data;
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
        public ISQLCommands SQLCommands { get; set; }
        private Boolean isExpanded;

        public Boolean IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; OnPropertyChanged(); }
        }

        public TaskManagerModel(ISQLCommands SqlCommands) {
            IsExpanded = True;
            SQLCommands = SqlCommands;
        }
        public async void Init()
        {
            List<Category> AllCategories =  await SQLCommands.GetAllCategories();
            MenuItem root = new MenuItem() { Title="任务类别：" };
            MenuItem InvestRoot = new MenuItem() { Title = "Invest", Color = "#FFB6C1" };
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
    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public string Title { get; set; }
        public string Color { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
