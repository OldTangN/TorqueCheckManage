using LT.BLL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            ExcelHelp _excelHelper = new ExcelHelp();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Excel (*.XLS)|*.xls";
            int i = 0, j = 0;
            if ((bool)(openFileDialog.ShowDialog()))
            {
                try
                {

                    DataTable dt = _excelHelper.LoadExcel(openFileDialog.FileName);
                    DataTable di=new DataTable ();
                    List<listPerson> listper = new List<listPerson>();
                    foreach (DataRow dr in dt.Rows)
                    {
                      
                       // listper.Add (new listPerson(){name=dr[3].ToString (),card=dr[4].ToString ()})  
                        dr[4] = Convert.ToInt32(dr[4].ToString (),16).ToString ();
                    }
                    if ((bool)(saveFileDialog.ShowDialog()))
                    _excelHelper.SaveToExcel(saveFileDialog.FileName, dt);
                  
                }
                catch { }
            }
        }
    }
    public class listPerson
    {
        public string name { get; set; }
        public string card { get; set; }
    }
}
