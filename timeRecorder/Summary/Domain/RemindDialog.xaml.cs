using Summary.Common;
using System.Windows.Controls;

namespace Summary.Domain
{
    /// <summary>
    /// Interaction logic for RemindDialog.xaml
    /// </summary>
    public partial class RemindDialog : UserControl
    {
        public RemindDialog(string title, string message)
        {
            InitializeComponent();
            this.DataContext = new
            {
                Title = title,
                Message = message
            };
        }
    }
}
