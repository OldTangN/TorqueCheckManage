using LongTie.Nlbs.Check;
using LongTie.Nlbs.Model;
using LongTie.Nlbs.Print;
using LT.BLL.Check;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LongTie.Nlbs.MyDefine
{
    /// <summary>
    /// Interaction logic for MyTabitem.xaml
    /// </summary>
    public partial class MyTabitem 
    {
      public   int count = 0;
        int arry = 0;
        bool currentsuccess =true;
        int successcount = 0;
        int confcount = 5;
        int confarry = 5;
        IWrench Wrench = DataAccess.CreateWrench();
       public  wrench _wrench = new wrench();
      public     bool isstart = false;
       public  bool issave = false;
       public bool isend = false;
        users _juser = new users();
       users _zuser = new users();
      public   List<CheckDatashow> _historycheckdatalist = new List<CheckDatashow>();
        public FilterData fd = new FilterData();
        public MyTabitem(users juser,users zuser)
        {
            InitializeComponent();
            _juser = juser;
            _zuser = zuser;
            confarry = Convert .ToInt16 ( OperationConfig.GetValue("arry"));
            confcount = Convert.ToInt16(OperationConfig.GetValue("count"));
            cb_errorrangbinding();
        }
        void cb_errorrangbinding() {
            List<RateModel> rm = new List<RateModel>() {new RateModel (){id=1,value=0.01,name="1%"},new RateModel(){id=2,value =0.04,name ="4%"},new RateModel (){id=3,value=0.06,name="6%"}};
            cb_errorrang.ItemsSource = rm;
            cb_errorrang.DisplayMemberPath = "name";
            cb_errorrang.SelectedValuePath = "value";
            cb_errorrang.SelectedIndex = 2;
        }
      
        private bool addcount(CheckDatashow cds)
        {
            if (successcount > (confcount))
            {
                arry = confarry;
                isend = true;
                return false;
            }
            if (arry >= confarry) {
                isend = true;
                return false;
            }
            if (currentsuccess)
            {
                if (count >= confcount) { count = 0; arry++; }
                count++;
            }
            else { arry++; count = 0; }

            return true;
        }
        //bool  isgetvalue() {
        //    if (_wrench.guid ==null) { MessageBox.Show("请输入该工具条形码！获取工具信息！"); return false ; }
        //    string s = @"^([-+]?[1-9]\d*\.\d+|-?0\.\d*[1-9]\d*)$";//正则表达式
        //    Regex reg = new Regex(s);
        //    if (!reg.IsMatch(this.tb_targetvalue.Text.Trim()) || string.IsNullOrEmpty(this.tb_targetvalue.Text.Trim()))
        //    {
        //        MessageBox.Show("请在输入正确的校验目标值");
        //        return false ;
        //    }
        //    return true;
        //}

        public void add(CheckDatashow cd)
        {
            
          
            fd.CheckData = cd;
            CheckDatashow cds = new CheckDatashow();
            if (fd.CheckData == null) return;
            cds = fd.returncheck();
            if (!string.IsNullOrEmpty(cds.isgood))
            {

                if (cds.isgood.Equals("√"))
                {
                    currentsuccess = true;
                    successcount++;
                }
                else { currentsuccess = false; if (successcount < confcount)successcount = 0; }
                if (addcount(cds))
                {
                    cds.id = count;
                    _historycheckdatalist.Add(cds);
                }

            }
            updata();
        }

        void updata() {
            this.dg_testdata.ItemsSource = null;
            this.dg_testdata.ItemsSource = _historycheckdatalist;
            if (successcount >= confcount) {
                this.lb_result.Content = "校验结果：工具合格";
            }
        }

     public    void contentEnable(bool b) { }
        #region
        //private void updata()
        //{
        //    this.checkdata.Children.Clear();
        //    int row = 0;
        //    List<CheckDatashow> _checkdatashow = new List<CheckDatashow>();
        //    for (int i = 0; i < _historycheckdatalist.Count; i++)
        //    {
        //        if (_historycheckdatalist[i].isgood.Equals("×"))
        //        {
        //            _checkdatashow.Add(_historycheckdatalist[i]);
        //            List<CheckDatashow> templist = new List<CheckDatashow>();
        //            foreach (CheckDatashow c in _checkdatashow) { c.id = i; templist.Add(c); }
        //            showdata(templist, row++);
        //            _checkdatashow.Clear();
        //            continue;
        //        }
        //        _checkdatashow.Add(_historycheckdatalist[i]);
        //    }
        //    if (_checkdatashow.Count > 0)
        //        showdata(_checkdatashow, row);
        //}


        //void showdata(List<CheckDatashow >lcds,int row) {
        //    this.checkdata.RowDefinitions.Add (new  RowDefinition ());

        //    Border b0= new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1), };
        //    Label t0 = new Label() { Content = row.ToString(), Width = 80, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
        //    b0.Child = t0;
        //    Grid.SetColumn(b0, 0);
        //    Grid.SetRow(b0, row);
        //    this.checkdata.Children.Add(b0);

        //    Border b1 = new Border() { BorderBrush=Brushes.Black, BorderThickness =new Thickness (1),};
        //    Label t1 = new Label() { Content = "工具编码", Width = 80, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b1.Child = t1;
        //    Grid.SetColumn(b1,1);
        //    Grid.SetRow(b1, row);
        //    this. checkdata.Children .Add (b1);

        //    Border b2 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
        //    Label t2 = new Label() { Content = "工具量程", Width = 60, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b2.Child = t2;
        //    Grid.SetColumn(b2, 2);
        //    Grid.SetRow(b2, row);
        //    this.checkdata.Children.Add(b2);

        //    Border b3 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
        //    Label t3 = new Label() { Content = this.tb_targetvalue.Text.Trim().ToString(), Width = 80, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b3.Child = t3;
        //    Grid.SetColumn(b3, 3);
        //    Grid.SetRow(b3, row);
        //    this.checkdata.Children.Add(b3);

        //    GridCheckdata dt = new GridCheckdata(lcds);
        //    Grid.SetRow(dt, row);
        //    Grid.SetColumn(dt, 4);
        //    this.checkdata.Children.Add(dt);


        //    Border b5 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1)};
        //    Label t5 = new Label() { Content = successcount >= confcount ? "成功" : "失败", Width = 60, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b5.Child = t5;
        //    Grid.SetColumn(b5, 5);
        //    Grid.SetRow(b5, row);
        //    this.checkdata.Children.Add(b5);


        //    Border b6 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
        //    Label t6 = new Label() { Content = "校验员信息".ToString(), Width = 80, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b6.Child = t6;
        //    Grid.SetColumn(b6, 6);
        //    Grid.SetRow(b6, row);
        //    this.checkdata.Children.Add(b6);


        //    Border b7 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
        //    Label t7 = new Label() { Content = DateTime.Now.ToShortDateString(), Width = 60, HorizontalContentAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        //    b7.Child = t7;
        //    Grid.SetColumn(b7, 7);
        //    Grid.SetRow(b7, row);
        //    this.checkdata.Children.Add(b7);

        //}
        #endregion

        private void tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_barcode.Text.Trim())) return;
            if (e.Key == Key.Enter)
            {
                _wrench = Wrench.selectByBarcode(this.tb_barcode.Text.Trim());
                    //new wrench() {id=1, createDate =DateTime .Now.ToString ("yyyy-MM-dd hh:mm:ss") ,factory ="动车",IP="12334454",port ="90",wrenchBarCode ="12345",status ="维护",wrenchCode="23333"};
               // if (_wrench.guid  == null) { MessageBox.Show("工具不存在！"); return; }
                this.tb_targetvalue.Text =_wrench .targetvalue .ToString ();
                this.lb_barcode.Content ="工具条码：" +_wrench.wrenchBarCode;
                this.lb_wrenchid.Content ="工具编号：" +_wrench.wrenchCode;
                this.lb_rangvalue.Content = "量程范围：" + _wrench.rangeMin + _wrench.unit + "~" + _wrench.rangeMax + _wrench.unit;
                isstart = true;
            }
        }

        private void tb_print_Click(object sender, RoutedEventArgs e)
        {
            if (!issave) { MessageAlert.Alert("请先提交数据！"); return; }
            HandleData hd = new HandleData();
          //  hd.Checkdatashow =_historycheckdatalist;
            List<CheckDatashow> ptintcds = new List<CheckDatashow>();
            if (hd.filterdata())
            {
               // if (hd._successcheckdatashow.Count > 0) { ptintcds = hd._successcheckdatashow; }
               // else { ptintcds = hd.getprint(); }
            }
            
          //  PrintSingleCheckdata psc = PrintSingleCheckdata.GetPrintSingleCheckdata(_wrench, ptintcds,_juser ,_zuser );
          //  psc.Show();
        }

        private void cb_errorrang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _historycheckdatalist = new List<CheckDatashow>();
            updata();
            issave = false;
            isend = false;
                  count = 0;
         arry = 0;
         currentsuccess =true;
         successcount = 0;
        }

    }
}
