using QDDL.Model;
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

namespace QDDL.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for WrenchShow.xaml
    /// </summary>
    public partial class WrenchShow : Window
    {
        wrenchinfo _wrenchinfo = null;
        public WrenchShow(wrenchinfo w)
        {
            InitializeComponent();
            _wrenchinfo = w;
            ShowWrench(_wrenchinfo);
        }
        void ShowWrench(wrenchinfo w)
        {
            if (w == null)
                return;
            if (w.wrench == null)
                return;
            this.wrenchbarcode.Text = w.wrench.wrenchBarCode;
            this.wrenchcode.Text = w.wrench.wrenchCode;
            this.setvalue.Text = w.wrench.targetvalue.ToString ("f2");
            this.factory.Text = w.wrench.factory;
            this.rang.Text = w.wrench.rangeMin.ToString("f2") + "~" + w.wrench.rangeMax.ToString("f2");
            this.time.Text = w.wrench.createDate.ToString().Replace('T',' ');
            this.lasttime.Text = w.wrench.lastrepair.ToString().Replace('T',' ');
            this.cycletime.Text = w.wrench.cycletime.ToString ("f0");
            if (w.species != null)
                this.species.Text = w.species.speciesName;
            if (w.status != null)
                this.status.Text  = w.status.statusName;

        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
