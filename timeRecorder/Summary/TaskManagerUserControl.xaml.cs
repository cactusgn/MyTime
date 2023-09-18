using Summary.Data;
using Summary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
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

namespace Summary
{
    /// <summary>
    /// Interaction logic for TaskManagerUserControl.xaml
    /// </summary>
    public partial class TaskManagerUserControl : UserControl
    {
        ContextMenu CategoryContextMenu;
        public TaskManagerUserControl(TaskManagerModel taskManagerModel)
        {
            InitializeComponent();
            this.DataContext = taskManagerModel;
            taskManagerModel.RootTreeView = RootTreeView;
            taskManagerModel.RefreshCategories();
            CategoryContextMenu = (ContextMenu)this.Resources["CategoryContextMenu"];
            taskManagerModel.CategoryContextMenu = CategoryContextMenu;
            taskManagerModel.CategoryContextMenu.DataContext = taskManagerModel;
        }

        private void RootTreeView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
                if(((Models.MenuItemModel)treeViewItem.Header).Title == "任务类别：")
                {
                    ((System.Windows.Controls.MenuItem)CategoryContextMenu.Items.GetItemAt(1)).IsEnabled = false;
                    ((System.Windows.Controls.MenuItem)CategoryContextMenu.Items.GetItemAt(2)).IsEnabled = false;
                }
                else{
                    ((System.Windows.Controls.MenuItem)CategoryContextMenu.Items.GetItemAt(1)).IsEnabled = true;
                    if (((Models.MenuItemModel)treeViewItem.Header).Title == "invest" || ((Models.MenuItemModel)treeViewItem.Header).Title == "work" || ((Models.MenuItemModel)treeViewItem.Header).Title == "play")
                    {
                        ((System.Windows.Controls.MenuItem)CategoryContextMenu.Items.GetItemAt(2)).IsEnabled = false;
                    }
                    else
                    {
                        ((System.Windows.Controls.MenuItem)CategoryContextMenu.Items.GetItemAt(2)).IsEnabled = true;
                    }
                }
                
            }
        }
        /// <summary>
        /// Searches for the <see cref="TreeViewItem"/> up the parent tree.
        /// </summary>
        /// <param name="source"></param>
        public static TreeViewItem? VisualUpwardSearch(DependencyObject? source)
        {
            while (source != null && source is not TreeViewItem)
            {
                source = VisualTreeHelper.GetParent(source);
            }

            return source as TreeViewItem;
        }
        private void DialogHost2_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true))
                return;
            var tmModel = (TaskManagerModel)this.DataContext;
            var dialogRes = tmModel.CategoryModel;

            if (dialogRes.Category=="")
            {
                return;
            }

            if (dialogRes.Title=="增加子类别"&&dialogRes.Visible)
            {
                if (tmModel.CategoryExist(dialogRes.Category).Result)
                {
                    dialogRes.ShowInvalidCateMessage="Visible";
                    eventArgs.Cancel();
                    return;
                }
                tmModel.addCategory(dialogRes);
            }
            else if (dialogRes.Title=="修改类别")
            {
                if (tmModel.EditCheck(dialogRes).Result)
                {
                    tmModel.EditCategory(dialogRes);
                }
                else
                {
                    eventArgs.Cancel();
                }
            }
        }

        private void DialogHost2_DialogClosed(object sender, MaterialDesignThemes.Wpf.DialogClosedEventArgs eventArgs)
        {
            
        }

        private void RootTreeView_PreviewDragOver(object sender, DragEventArgs e)
        {
            Trace.Write(sender);
        }
    }
}
