using LT.Model;
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
using System.Windows.Shapes;

namespace LongTie.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for GridCheckdata.xaml
    /// </summary>
    public partial class GridCheckdata
    {
        //int arry;
        //int count;
        public List<CheckDatashow> _checkdatashow=new List<CheckDatashow> ();
        public GridCheckdata(List <CheckDatashow >checkdatalist)
        {
            InitializeComponent();
            _checkdatashow = checkdatalist;
            BindingDataGrid();
        }
        public GridCheckdata() { }
        private void BindingDataGrid() {
            if (_checkdatashow != null) { 
           // this.chekshow.ItemsSource = null;
            this.chekshow.ItemsSource = _checkdatashow;}
        }
    }
}
