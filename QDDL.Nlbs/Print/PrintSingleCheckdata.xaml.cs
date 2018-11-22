using QDDL.Comm;
using QDDL.Model;
using QDDL.Model.BllModel;
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

namespace QDDL.Nlbs.Print
{
    /// <summary>
    /// PrintSingleCheckdata.xaml 的交互逻辑
    /// </summary>
    public partial class PrintSingleCheckdata : Window
    {
        public static PrintSingleCheckdata printsinglecheckdata=null;
        public static PrintSingleCheckdata GetPrintSingleCheckdata(wrench wrenchmodel, List<ShowCheckresult> checkrecodelist, int configcount,decimal setvalue  ,userinfo  juser, userinfo  zuser,DateTime dt,bool wrenchgood)
        {
            if (printsinglecheckdata == null || !printsinglecheckdata.IsLoaded)
                printsinglecheckdata = new PrintSingleCheckdata(  wrenchmodel, checkrecodelist,configcount,setvalue ,juser ,zuser ,dt,wrenchgood);
            return printsinglecheckdata;
        }
        wrench _wrench = new wrench();
        List<ShowCheckresult> _checkhistory = new List<ShowCheckresult>();
        int confint =0;
        decimal _setvalue = 0;
        DateTime _dt = new DateTime();
        userinfo  _zuser = new userinfo ();
        userinfo  _juser = new userinfo ();
        bool _iswrenchgood = false;
        PrintSingleCheckdata(wrench wrenchmodel, List<ShowCheckresult> checkrecodelist, int confgcount, decimal setvalue, userinfo  juser, userinfo  zuser,DateTime dt,bool wrenchgood)
        {
            InitializeComponent();
            _wrench = wrenchmodel;
            _checkhistory = checkrecodelist;
            _zuser = zuser;
            _juser = juser;
            _dt = dt;
            confint=confgcount;
            _setvalue = setvalue;
            _iswrenchgood = wrenchgood;
            showdata();
           
        }
         void showdata()
         {
             this.lb_id.Content = _wrench.wrenchCode;
             this.lb_rang.Content = _wrench.rangeMin.ToString("f2") + "~" + _wrench.rangeMax.ToString("f2");
             this.lb_setcheckvalue.Content = _wrench.wrenchBarCode.ToString();
             this.tb_jname.Text = _juser.user.username;
             this.tb_jtime.Text  = _dt.ToString("yyyy-MM-dd");
             this.tb_jdepartment.Text = _juser.department.departmentName;
             
             if (_zuser != null && _zuser.user != null)
             {
                 this.tb_zname.Text = _zuser.user.username;
                 this.tb_ztime.Text = _dt.ToString("yyyy-MM-dd");
                 this.tb_zdepartment.Text = _zuser.department.departmentName;
             
             }
                 for (int i = 0; i < _checkhistory.Count;i++ ) {
                     this.main_centergrid .RowDefinitions.Add(new RowDefinition());
                     Border b0 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1,0,1,1) };
                     Label t0 = new Label() { Content  = (i + 1).ToString(), Width = 80, Height = 40, HorizontalContentAlignment  = HorizontalAlignment.Center, VerticalContentAlignment  = VerticalAlignment.Center };
                    
                      b0.Child = t0;
                     Grid.SetColumn(b0, 0);
                     Grid.SetRow(b0, i+3);
                     this.main_centergrid.Children.Add(b0);


                     Border b1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                     Label t1 = new Label() { Content = _checkhistory[i].setdata.ToString("f2"), Width = 80, Height = 40, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                     b1.Child = t1;
                     Grid.SetColumn(b1, 1);
                     Grid.SetRow(b1, i + 3);
                     this.main_centergrid.Children.Add(b1);


                     Border b2 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                     Label t2 = new Label() { Content = _checkhistory[i].checkdata.ToString("f2"), Width = 80, Height = 40, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                     b2.Child = t2;
                     Grid.SetColumn(b2, 2);
                     Grid.SetRow(b2, i + 3);
                     this.main_centergrid.Children.Add(b2);


                     Border b3 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                     Label t3 = new Label() { Content = _checkhistory[i].normalrang, Width = 160, Height = 40, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                     b3.Child = t3;
                     Grid.SetColumn(b3, 3);
                     Grid.SetRow(b3, i + 3);
                     this.main_centergrid.Children.Add(b3);


                     Border b4 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                     Label t4 = new Label() { Content = _checkhistory[i].result, Width = 80, Height = 40, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                     b4.Child = t4;
                     Grid.SetColumn(b4, 4);
                     Grid.SetRow(b4, i + 3);
                     this.main_centergrid.Children.Add(b4);


                     Border b5 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                     Label t5 = new Label() { Content = _checkhistory[i].error, Width = 100, Height = 40, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                     b5.Child = t5;
                     Grid.SetColumn(b5, 5);
                     Grid.SetRow(b5, i + 3);
                     this.main_centergrid.Children.Add(b5);
                
                 }



                 Border b6 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(0, 0, 1, 1) };
                 Label t6= new Label() { Content = _iswrenchgood==false? "不合格" : "合格", Width = 80, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                 b6.Child = t6;
                 Grid.SetColumn(b6,6);
                 Grid.SetRow(b6, 3);
                 Grid.SetRowSpan(b6,_checkhistory .Count);
                 this.main_centergrid.Children.Add(b6);

             }
         
        //string getrang(double  setvalue, double rate) {
        //    double tempvalue = setvalue * rate;
        //    double  max = setvalue + tempvalue;
        //    double  min = setvalue - tempvalue;
        //    return min.ToString() + "-" + max.ToString();

        //}

        private void bt_print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            pd.PrintVisual(willprint, "校验报告");
        }

        private void bt_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

   

  

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.WindowStyle = System.Windows.WindowStyle.None;
            //this.Left = 0.0;
            this.Top = 0.0;
            //this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
