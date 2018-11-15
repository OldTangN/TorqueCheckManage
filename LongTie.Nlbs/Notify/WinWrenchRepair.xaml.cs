using LT.Model.BllModel;
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

namespace LongTie.Nlbs.Notify
{
    /// <summary>
    /// Interaction logic for WinWrenchRepair.xaml
    /// </summary>
    public partial class WinWrenchRepair : Window
    {
        private List<WrenchNotice> wrench;
        public List<WrenchNotice> Wrench
        {
            get { return this.wrench; }
            set { this.wrench = value; }
        }
        public WinWrenchRepair(List<WrenchNotice> wrench)
        {
            InitializeComponent();
           // this.wrench = wrench;
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = wrench;
        }
    }
    //public class WrenchNotice
    //{

    //    public string wrenchbarcode { get; set; }
    //    public string lastrepairtime { get; set; }
    //    public string intime { get; set; }
    //    public string cycletime { get; set; }


    //}
}
