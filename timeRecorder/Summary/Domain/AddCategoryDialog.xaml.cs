using Summary.Common;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Summary.Domain
{
    /// <summary>
    /// Interaction logic for RemindDialog.xaml
    /// </summary>
    public partial class AddCategoryDialog : UserControl
    {
        public AddCategoryDialog(AddCategoryModel categoryModel)
        {
            InitializeComponent();
            this.DataContext = categoryModel;
            
        }
       
    }
}
