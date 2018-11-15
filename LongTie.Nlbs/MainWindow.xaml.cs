using LongTie.Nlbs.Borrow;
using LongTie.Nlbs.Check;
using LongTie.Nlbs.User;
using LongTie.Nlbs.Wrench;
using LT.BLL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public users user { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //CheckTools ct = new CheckTools();
            //ct.Owner = this;
            //ct.Show();
        }

        private void grid_users_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //UserGrid u = new UserGrid(ruc);
            //mainGrid.Children.Clear();
            //mainGrid.Children.Add(u);

        }

        private void grid_wrench_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //WrenchList wl = new WrenchList(this);
            //mainGrid.Children.Clear();
            //mainGrid.Children.Add(wl);
        }

        private void grid_wrenchout_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //WrenchBorrow wb = new WrenchBorrow();
            //wrenchborrowmainGrid.Children.Clear();
            //wrenchborrowmainGrid.Children.Add(wb);
        }

        private void grid_wrenchin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //WrenchReturn wr = new WrenchReturn( );
            //wrenchborrowmainGrid.Children.Clear();
            //wrenchborrowmainGrid.Children.Add(wr);

        }

        private void bt_loginout_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void grid_check_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //SearchChechResult scr = new SearchChechResult(this);
            //checkmainGrid.Children.Clear();
            //checkmainGrid.Children.Add(scr);
        }
    }
}
