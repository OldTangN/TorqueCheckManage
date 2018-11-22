using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QDDL.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for WinCheckData.xaml
    /// </summary>
    public partial class WinCheckData : Window
    {

        public string Setvalue = "";
        public WinCheckData()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = 388;
            this.Top = 165;
            this.tb_setvalue.Focus();

        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
        
            this.Close();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidate())
            {
                MessageBox.Show("请输入正确的值");
                return;
            }
            this.Setvalue = this.tb_setvalue.Text.Trim();
            this.xButton_Click(sender,e);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Setvalue = "";
            this.xButton_Click(sender, e);
        }

        private void tb_setvalue_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter&&tb_setvalue.Text.Trim()!="")
              submit_Click(sender, e);
        }

        bool IsValidate()
        {
            if (string.IsNullOrEmpty(this.tb_setvalue.Text.Trim()))
                return true;
            try { Convert.ToDouble(this.tb_setvalue.Text.Trim()); return true; }
            catch { tb_setvalue.Text = ""; this.Setvalue = ""; return false; }
        }
    }
}
