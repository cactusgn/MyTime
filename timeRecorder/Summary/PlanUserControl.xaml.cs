using Summary.Models;
using System;
using System.Collections.Generic;
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
    /// PlanUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlanUserControl : UserControl
    {
        public PlanUserControl(PlanModel planModel)
        {
            InitializeComponent();
            this.DataContext = planModel;
            planModel.TimeGrid = this.TimeGrid;
            init();
        }
        public void init()
        {
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "日期",
                IsReadOnly = false,
                Binding = new Binding("taskDate")
                {
                    StringFormat = "yyyy/MM/dd HH:mm:ss"
                }
            });
            this.TimeGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "任务名称",
                IsReadOnly = false,
                Binding = new Binding("taskName")
            });
        }
       
        private void OnAddColumnButtonClick(object sender, RoutedEventArgs e)
        {
            // 创建一个新的 DataGridTextColumn
            var newColumn = new DataGridTextColumn
            {
                Header = "New Column " + (TimeGrid.Columns.Count + 1),
                Binding = new Binding($"NewField{TimeGrid.Columns.Count + 1}") // 这里需要一个对应的属性，但因为我们只是示例，可以跳过绑定验证
            };

            // 添加到 DataGrid 的 Columns 集合中
            TimeGrid.Columns.Add(newColumn);
        }
    }
}
