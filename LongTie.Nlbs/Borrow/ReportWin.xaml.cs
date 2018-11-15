using CodeReason.Reports;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace LongTie.Nlbs.Borrow
{
    /// <summary>
    /// Interaction logic for ReportWin.xaml
    /// </summary>
    public partial class ReportWin : Window
    {
        private bool _firstActivated = true;
        List<BorrowHistory> _borrowhistory;
        public ReportWin(List<BorrowHistory> bl)
        {
            InitializeComponent();
            _borrowhistory = bl;
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            if (!_firstActivated) return;

            _firstActivated = false;

            try
            {
                ReportDocument reportDocument = new ReportDocument();
                StreamReader reader = new StreamReader(new FileStream(@"Report\BorrowReport.xaml", FileMode.Open, FileAccess.Read));
                reportDocument.XamlData = reader.ReadToEnd();
                reportDocument.XamlImagePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Report\");
                reader.Close();

                ReportData data = new ReportData();

                // set constant document values
                data.ReportDocumentValues.Add("PrintDate", DateTime.Now); // print date is now

                // sample table "Ean"
                DataTable table = new DataTable("Ean");
                table.Columns.Add("wrenchbarcode", typeof(string));
                table.Columns.Add("borrowuser", typeof(string));
                table.Columns.Add("borrowoper", typeof(string));
                table.Columns.Add("borrowtime", typeof(string ));
                table.Columns.Add("returnuser", typeof(string));
                table.Columns.Add("returnoper", typeof(string));
                table.Columns.Add("returntime", typeof(string));
                //table.Columns.Add("common", typeof(string));
                // Random rnd = new Random(1234);
                foreach (BorrowHistory bh in _borrowhistory)
                {
                    table.Rows.Add(new object []{bh.wrenchbarcode ,bh.borrowusername,bh.borrowoperator,bh.borrowdate ,bh.returnuser,bh.returnoperator,bh.returndate });
                }
                //for (int i = 1; i <= 100; i++)
                //{
                //    // randomly create some articles
                //    table.Rows.Add(new object[] { i, "Item " + i.ToString("0000"), "123456790123", rnd.Next(9) + 1 ,"returnuser","returnoper","returntime","common"});
                //}
                data.DataTables.Add(table);

                DateTime dateTimeStart = DateTime.Now; // start time measure here

                XpsDocument xps = reportDocument.CreateXpsDocument(data);
                documentViewer.Document = xps.GetFixedDocumentSequence();

                // show the elapsed time in window title
                Title += " - generated in " + (DateTime.Now - dateTimeStart).TotalMilliseconds + "ms";
            }
            catch (Exception ex)
            {
                // show exception
                MessageBox.Show(ex.Message + "\r\n\r\n" + ex.GetType() + "\r\n" + ex.StackTrace, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}
