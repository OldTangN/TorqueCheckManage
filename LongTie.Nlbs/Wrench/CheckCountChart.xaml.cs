using LT.BLL.MyChart;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace LongTie.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for CheckCountChart.xaml
    /// </summary>
    public partial class CheckCountChart 
    {
        IWrench Wrench = DataAccess.CreateWrench();
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        public CheckCountChart()
        {
            InitializeComponent();
        }
        void check(bool b)
        {
            this.rb_week.IsChecked = b;
            this.rb_year.IsChecked = !b;       
        }
        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim())) 
            {
                MessageAlert.Alert("请填写工具条码！");
                return;
            }
            if (this.dp_date.SelectedDate.ToString() == "") {
                MessageAlert.Alert("请选择时间！");
                return;
            }

            List<string> strListy = new List<string>();
            List<string> strListx = new List<string>();
            DateTime time = Convert.ToDateTime(this.dp_date.SelectedDate.ToString());
            wrench w = Wrench.selectByBarcode(this.tb_wrenchbarcode.Text.Trim());
            if (rb_week.IsChecked != null && rb_week.IsChecked==true) 
            {
                WeekCheckData weekcheckdata = new WeekCheckData(time, this.tb_wrenchbarcode.Text.Trim());
                strListy = weekcheckdata.GetList();
                strListx = weekcheckdata.datelist;
            }



            if (rb_year.IsChecked != null && rb_year.IsChecked==true )
            {
                YearCheckData weekcheckdata = new YearCheckData(time, this.tb_wrenchbarcode.Text.Trim());
                strListy = weekcheckdata.GetList();
                 strListx = weekcheckdata.datelist;
            }
          
            int count = 0;
            foreach (string s in strListy)
            {
                count += Convert.ToInt32(s);
            }
            this.lb_count.Content = "校验总次数：" + count.ToString();
            this.lb_bar.Content = "工具编码：" +( w != null ? w.wrenchCode : "");
            ColumnSeries cs = new ColumnSeries(strListx ,strListy);
            if(cs.CreateChar()!=null)
            this.chart.Children.Add(cs.CreateChar());
        }

         

        private void rb_year_Checked(object sender, RoutedEventArgs e)
        {
            check(false);
        }

        private void rb_week_Checked(object sender, RoutedEventArgs e)
        {
            check(true);
        }

        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.bt_search_Click(this,e);
        }
    }
}
