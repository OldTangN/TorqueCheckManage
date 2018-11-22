using QDDL.BLL;
using QDDL.BLL.Wrench;
using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QDDL.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for WrenchList.xaml
    /// </summary>
    public partial class WrenchList
    {
        List<wrench> _wrenchlist = new List<wrench>();
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();

        private ToolModel printModel { set; get; }
        public WrenchList()
        {
            InitializeComponent();
            getstatuslist();
            //List<ToolModel> toolmodellist = Gettoolmodel(Wrench.select());
            //if (toolmodellist != null && toolmodellist.Count > 0)
            //    getwrenchlist(toolmodellist);
        }

        void getstatuslist()
        {
            List<wrenchstatus> status = WrenchStatus.selectAll();
            wrenchstatus w = new wrenchstatus();
            if (status != null)
                status.Insert(0, w);
            this.cb_status.ItemsSource = status;
            this.cb_status.DisplayMemberPath = "statusName";
            this.cb_status.SelectedValuePath = "guid";
        }
        private void getwrenchlist(List<ToolModel> toolmodellist)
        {
            dataGrid1.ItemsSource = toolmodellist;
        }
        private List<ToolModel> Gettoolmodel(List<wrench> wrenchlist)
        {
            List<ToolModel> toolmodellist = new List<ToolModel>();
            try
            {
                if (wrenchlist != null && wrenchlist.Count > 0)
                {
                    int count = 0;

                    foreach (wrench w in wrenchlist)
                    {
                        count++;
                        wrenchspecies ws = WrenchSpecies.selectByGuid(w.species);
                        wrenchstatus wss = WrenchStatus.selectByguid(w.status);
                        toolmodellist.Add(new ToolModel()
                        {
                            comment = w.comment,
                            createDate = w.createDate,
                            factory = w.factory,
                            guid = w.guid,
                            id = count,
                            IP = w.IP,
                            port = w.port,
                            rangeMax = Convert.ToDecimal(w.rangeMax.ToString("f1")),
                            rangeMin = Convert.ToDecimal(w.rangeMin.ToString("f1")),
                            targetvalue = Convert.ToDecimal(w.targetvalue.ToString("f1")),
                            targetvalue1 = Convert.ToDecimal(w.targetvalue1.ToString("f1")),
                            targetvalue2 = Convert.ToDecimal(w.targetvalue2.ToString("f1")),
                            unit = w.unit,
                            species = w.species,
                            speciesName = ws.speciesName,
                            status = w.status,
                            statusDM = wss.statusDM,
                            statusName = wss.statusName,
                            lastrepair = w.lastrepair,
                            cycletime = Convert.ToDecimal(w.cycletime.ToString("f0")),
                            wrenchBarCode = w.wrenchBarCode,
                            wrenchCode = w.wrenchCode
                        });
                    }
                }
                else
                {
                    MessageAlert.Alert("没有工具信息！");
                }
            }
            catch { }
            return toolmodellist;

        }
        #region
        //private void btn_add_Click(object sender, RoutedEventArgs e)
        //{
        //    //editerWrench ew = editerWrench.GetAddediterwrench();
        //    //ew.Owner = _mw;
        //    //if ((bool)ew.ShowDialog())
        //    //{
        //    //    getwrenchlist(Gettoolmodel(Wrench.select()));
        //    //}
        //}

        //private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (dataGrid1.SelectedItem == null) { MessageBox.Show("请选择要修改的行！"); return; }
        //    //editerWrench ew = editerWrench.GetUpdateediterwrench(dataGrid1 .SelectedItem  as wrench );
        //    //ew.Owner = _mw;
        //    //if ((bool)ew.ShowDialog()) {
        //    //    getwrenchlist(Gettoolmodel(Wrench.select()));
        //    //}
        //}

        //private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dataGrid1.SelectedItem == null) { MessageBox.Show("请选择要删除的行！"); return; }
        //    wrench w = dataGrid1.SelectedItem as wrench;
        //    wrenchstatus ws=WrenchStatus.selectByStatusDM ("0");
        //    w.status = ws.guid;
        //    if (Wrench.updata(w))
        //    { MessageBox.Show("删除成功！"); }
        //    else { MessageBox.Show("删除失败！"); }
        //    getwrenchlist(Gettoolmodel(Wrench.select()));

        //}
        #endregion


 

        private void bt_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(this.tb_wrenchcode.Text.Trim()))
                    dict.Add("wrenchCode", this.tb_wrenchcode.Text.Trim());
                if (!string.IsNullOrEmpty(this.tb_wrenchbarcode.Text.Trim()))
                    dict.Add("wrenchBarCode", this.tb_wrenchbarcode.Text.Trim());
                if ((cb_status.SelectedItem as wrenchstatus) != null && (cb_status.SelectedItem as wrenchstatus).guid != null)
                    dict.Add("status_id", (cb_status.SelectedItem as wrenchstatus).guid);
                if (dict.Count > 0)
                    getwrenchlist(Gettoolmodel(Wrench.selectByContion(dict).OrderBy(p => p.createDate).ToList()));
                else
                {
                    getwrenchlist(Gettoolmodel(Wrench.select().OrderBy(p => p.createDate).ToList()));
                }
            }
            catch
            {
                dataGrid1.ItemsSource = null;
            }
        }

        private void infoout_Click(object sender, RoutedEventArgs e)
        {
            List<ToolModel> tl = (List<ToolModel>)this.dataGrid1.ItemsSource;
            if (tl == null || tl.Count <= 0)
                return;
            WrenchExcelOut weo = new WrenchExcelOut();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            ExcelHelp _excelHelper = new ExcelHelp();
            saveFileDialog.Filter = "Excel (*.XLS)|*.xls";
            if ((bool)(saveFileDialog.ShowDialog()))
            {
                try
                {

                    _excelHelper.SaveToExcel(saveFileDialog.FileName, weo.ToTable(tl));
                    MessageBox.Show("导出成功");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！");
                    return;
                }

            }
        }
        private void tb_wrenchcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }


        private void tb_wrenchbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.bt_search_Click(sender, e);
            }
            if (e.Key == Key.B)
            {
                MessageAlert.Alert("BBB");
            }
        }

        private void Bt_print_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItems.Count > 0)
            {
                printModel = (ToolModel)dataGrid1.SelectedItem;

                PrintItem();

            }
            else
            {
                MessageBox.Show("请选中要打印条码的工具行！");
            }


        }

        private void PrintItem()
        {

            var printDocument = new PrintDocument();
            printDocument.PrintPage += printDocument_PrintPage;
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("票据", 200, 120);//设置纸张的大小
            printDocument.PrinterSettings.PrinterName = OperationConfig.GetValue("PrintName");
            printDocument.Print();
        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {


            float x, y;
            float leftMargin = 10, topMargin = 5;
            Font font = new Font("黑体", 12);

            x = leftMargin;
            y = topMargin;
            //画二维码
            System.Drawing.Image image;
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.BackColor = System.Drawing.Color.White;//设置图片背景
            b.ForeColor = System.Drawing.Color.Black;//设置字体颜色
            b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            font = new Font("宋体", 1);
            b.LabelFont = font;
            b.Height = 40;
            b.Width = 180;
            image = b.Encode(BarcodeLib.TYPE.CODE128, printModel.wrenchBarCode);
            e.Graphics.DrawImage(image, x, y, b.Width, b.Height);

            x = leftMargin + 50;
            y = y + b.Height + 5;
            font = new Font("宋体", 10);
            e.Graphics.DrawString(printModel.wrenchBarCode, font, Brushes.Black, x, y, new StringFormat());


            x = leftMargin + 5;
            y = y + font.GetHeight(e.Graphics) + 10;
            font = new Font("宋体", 10);
            e.Graphics.DrawString(string.Format("类型:{0}{1}N.m~{2}N.m", printModel.factory, printModel.rangeMin, printModel.rangeMax), font, Brushes.Black, x, y, new StringFormat());

            ////////画标题

            ////////   x = leftMargin + ip.TitleNameX;
            //////y = topMargin;
            ////////e.Graphics.DrawString(ip.TitleName, new Font("黑体", 20), System.Drawing.Brushes.Black, x, y, new StringFormat());
            ////////画第二行 副标题
            //////x = leftMargin;//+ 70;
            //////y += font.GetHeight(e.Graphics);//返回此字体的行距
            ////////font = new Font("黑体", 15);
            ////////e.Graphics.DrawString("抽血排号凭证", font, Brushes.Black, x, y, new StringFormat());

            ////////画排队号
            //////x = leftMargin;
            //////y += font.GetHeight(e.Graphics) + 15;
            //////string str = "777";
            //////e.Graphics.DrawString(str, font, Brushes.Black, x, y, new StringFormat());

            ////////画线
            //////Pen pen = new Pen(Color.Black);
            //////pen.Width = 2;
            //////e.Graphics.DrawLine(pen, 280, 65, 10, 65);

            //////x = leftMargin + 10;
            //////y = y + font.GetHeight(e.Graphics) + 20;
            //////font = new Font("黑体", 20);
            //////str = "简简";
            //////e.Graphics.DrawString(str, font, Brushes.Black, x, y, new StringFormat());


            ////////画你所想
            //////x = leftMargin + 115;
            //////y = y + font.GetHeight(e.Graphics) - 25;
            //////font = new Font("宋体", 12);
            //////str = "女";
            //////e.Graphics.DrawString(str, font, Brushes.Black, x, y, new StringFormat());




            //////x = leftMargin + 110;
            //////y = y + font.GetHeight(e.Graphics) - 5 + 20;
            //////font = new Font("宋体", 12);
            //////str = "17";
            //////e.Graphics.DrawString(str, font, Brushes.Black, x, y, new StringFormat());





        }
        //static byte[] GetBarcode(int height, int width, BarcodeLib.TYPE type,
        //                                  string code)
        //{
        //    System.Drawing.Image image = null;
        //    BarcodeLib.Barcode b = new BarcodeLib.Barcode();
        //    b.BackColor = System.Drawing.Color.White;
        //    b.ForeColor = System.Drawing.Color.Black;
        //    b.IncludeLabel = true;
        //    b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
        //    b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
        //    b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        //    System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);
        //    b.LabelFont = font;

        //    b.Height = height;
        //    b.Width = width;

        //    image = b.Encode(type, code);
        //    //SaveImage(image, Guid.NewGuid().ToString("N") + ".png");
        //    //byte[] buffer = b.GetImageData(SaveTypes.GIF);
        //    //return buffer;
        //}
    }
}
