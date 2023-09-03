using Summary.Common;
using System.Windows.Controls;

namespace Summary.Domain
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class SampleDialog : UserControl
    {
        public SampleDialog(TimeViewObj timeObj, SampleDialogViewModel SDVM)
        {
            InitializeComponent();
            SDVM.StartTime = timeObj.StartTime;
            SDVM.EndTime = timeObj.EndTime;
            SDVM.SplitTime = timeObj.StartTime;
            this.DataContext = SDVM;
        }
    }
}
