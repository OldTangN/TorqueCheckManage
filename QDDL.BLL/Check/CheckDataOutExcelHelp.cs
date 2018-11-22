using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace QDDL.BLL.Check
{
  public   class CheckDataOutExcelHelp
    {
      ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();
      IWrench Wrench = DataAccess.CreateWrench();
        private Excel.Application _excelApp = null;
        private Excel.Workbooks _books = null;
        private Excel._Workbook _book = null;
        private Excel.Sheets _sheets = null;
        private Excel._Worksheet _sheet = null;
        private Excel.Range _range = null;
        private Excel.Font _font = null;
        // Optional argument variable
        private object _optionalValue = Missing.Value;

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="pPath"></param>
        /// <returns></returns>
        public System.Data.DataTable LoadExcel(string pPath)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                            "Extended Properties=Excel 8.0;" +
                            "data source=" + pPath;
            OleDbConnection myConn = new OleDbConnection(connString);
            string sheetName = this.GetExcelSheetName(pPath);
            string sql = "select * from [" + sheetName.Replace('.', '#') + "$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(sql, myConn);
            DataSet ds = new DataSet();
            try
            {
                myCommand.Fill(ds);
                return ds.Tables[0];
            }

            catch (Exception x)
            {
                ds = null;
                throw new Exception("从Excel文件中获取数据时发生错误！");
            }
            finally
            {
                myCommand.Dispose();
                myCommand = null;

                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
                myConn = null;
            }
        }
        private string GetExcelSheetName(string pPath)
        {
            //打开一个Excel应用

            _excelApp = new Excel.Application();
            if (_excelApp == null)
            {
                throw new Exception("打开Excel应用时发生错误！");
            }
            _books = _excelApp.Workbooks;
            //打开一个现有的工作薄
            _book = _books.Add(pPath);
            _sheets = _book.Sheets;
            //选择第一个Sheet页
            _sheet = (Excel._Worksheet)_sheets.get_Item(1);
            string sheetName = _sheet.Name;

            ReleaseCOM(_sheet);
            ReleaseCOM(_sheets);
            ReleaseCOM(_book);
            ReleaseCOM(_books);
            _excelApp.Quit();
            ReleaseCOM(_excelApp);
            return sheetName;
        }
        /// <summary>
        /// 释放COM对象
        /// </summary>
        /// <param name="pObj"></param>
        private void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("释放资源时发生错误！");
            }
            finally
            {
                pObj = null;
            }
        }


        /// <summary>
        /// 保存到Excel
        /// </summary>
        /// <param name="excelName"></param>
        public void SaveToExcel(string excelName, System.Data.DataTable Header, List<WrenchCheckOut> wrenchchecklist, string title)
        {
            try
            {
           
                
                    
                        //  Mouse.SetCursor(Cursors.Wait);
                        CreateExcelRef();
                        FillSheet(Header, title, wrenchchecklist);
                        SaveExcel(excelName);
                        // Mouse.SetCursor(Cursors.Arrow);
                    
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while generating Excel report");


            }
            finally
            {
                ReleaseCOM(_sheet);
                ReleaseCOM(_sheets);
                ReleaseCOM(_book);
                ReleaseCOM(_books);
                ReleaseCOM(_excelApp);
            }
        }

        /// <summary>
        /// 将内存中Excel保存到本地路径
        /// </summary>
        /// <param name="excelName"></param>
        private void SaveExcel(string excelName)
        {
            _excelApp.Visible = false;
            //保存为Office2003和Office2007都兼容的格式
            _book.SaveAs(excelName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _excelApp.Quit();

        }

        /// <summary>
        /// 创建表头
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private object[] CreateHeader(DataTable dataTable, string startRage)
        {

            List<object> objHeaders = new List<object>();
            for (int n = 0; n < dataTable.Columns.Count; n++)
            {
                objHeaders.Add(dataTable.Columns[n].ColumnName);
            }

            var headerToAdd = objHeaders.ToArray();
            //工作表的单元是从“A1”开始
            AddExcelRows(startRage, 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle(startRage);

         //   Title("A1", 1, header.Length, title);

            return headerToAdd;
        }

        private void CreateTitle(string startRange, int rowCount, int colCount, string titlevalue)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Merge(Missing.Value);
            _range.set_Value(_optionalValue, titlevalue);
            _range.RowHeight = 40;
            _range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            _font = _range.Font;
            _font.Bold = true;
            _font.Size = 15;
            _range.Borders.LineStyle = 1;
        }
        /// <summary>
        /// 将表头加粗显示
        /// </summary>
        private void SetHeaderStyle(string startRange)
        {
            _font = _range.Font;
            _font.Bold = true;
            _font.Size = 10;
            _range = _sheet.get_Range(startRange, Type.Missing);
            _range.RowHeight = 25;
            _range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            _range.WrapText = true;
        }
        /// <summary>
        /// 将数据填充到内存Excel的工作表
        /// </summary>
        /// <param name="dataTable"></param>
        private void FillSheet(System.Data.DataTable dataTable, string title, List<WrenchCheckOut> wrenchchecklist)
        {
            object[] header = CreateHeader(dataTable, "A2");
            CreateTitle("A1",1,header .Length ,title );
            WriteData(header, dataTable, wrenchchecklist);
        }

        private void WriteData(object[] header, DataTable dataTable, List<WrenchCheckOut> wrenchchecklist)
        {
            try {
   
            int j = 3;
            foreach (WrenchCheckOut w in wrenchchecklist)
            {
               
                List<torquecheckrecord> cd = CheckTargetRecord.SelectByCheckTargetID(w.guid);
                if (cd == null)
                { continue; }
                    wrench wc = Wrench.selectByBarcode(w.wrenchbarcode);
                    List<string> recordwrech = new List<string>();
                    foreach (torquecheckrecord t in cd)
                    {
                        if (!recordwrech.Contains(t.torqueTargetValue.ToString()))
                        {
                            recordwrech.Add(t.torqueTargetValue.ToString());
                        }
                    }
                
                if (recordwrech.Count <= 0)
                    continue;
                ///合并前两列
                _range = _sheet.get_Range("B"+j.ToString (), _optionalValue);
                _range = _range.get_Resize(recordwrech.Count , 1);
                _range.Merge(Missing.Value);
                _range.set_Value(_optionalValue,w.wrenchbarcode);
               
                ///合并前两列
                _range = _sheet.get_Range("C"+j.ToString (), _optionalValue);
                _range = _range.get_Resize(recordwrech.Count, 1);
                _range.Merge(Missing.Value);
                _range.set_Value(_optionalValue, (wc.rangeMin.ToString ("f1")+"~"+wc.rangeMax.ToString ("f1")));


                _range = _sheet.get_Range("D" + j.ToString(), _optionalValue);
                _range = _range.get_Resize(recordwrech.Count, 1);
                _range.Merge(Missing.Value);
                _range.set_Value(_optionalValue, w.is_good==true?"合格":"不合格");
             
                ////填充详细
                foreach (string s in recordwrech)
                {                    
                    decimal ds = Convert.ToDecimal(s);
                    List<torquecheckrecord> tc = cd.FindAll(p=>p.torqueTargetValue==ds&&p.isEffective ==true);
                    _sheet.Cells[j, 1] = j-2;
                    if (tc != null && tc.Count > 0)
                    {
                        _sheet.Cells[j, 5] = s;
                        int m=0;
                        for (int i = 6; i <=header.Length - 4; i++)
                        {
                            
                            if(m<tc.Count())
                            _sheet.Cells[j, i] = tc[m++].analyserValue;
                          
                        }
                        _sheet.Cells[j, header.Length - 3] = "合格";
                        _sheet.Cells[j, header.Length - 2] = w.jusername ?? "";
                        _sheet.Cells[j, header.Length - 1] = w.zusername ?? "";
                        _sheet.Cells[j, header.Length ] = w.checkdate;
                
                    }

                    else
                    {

                        List<torquecheckrecord> tc1 = cd.FindAll(p => p.torqueTargetValue == ds);
                        if (tc1 != null && tc1.Count > 0)
                        {
                            _range = _sheet.get_Range("E"+j.ToString (), _optionalValue);
                            _range.set_Value(_optionalValue, s);

                            for (int i = 6,m=0; i <=header.Length - 4; i++)
                            {
                              //  _range = _sheet.get_Range(_sheet.Cells[j, i], _optionalValue);
                                if (m < tc1.Count)
                                    _sheet.Cells[j, i] = tc1[m++].analyserValue;
                               // _range.set_Value(_optionalValue, tc[m++].analyserValue);
                            }
                            _sheet.Cells[j, header.Length - 3] = "不合格";
                            _sheet.Cells[j, header.Length - 2] = w.jusername ?? "";
                            _sheet.Cells[j, header.Length - 1]= w.zusername ?? "";
                            _sheet.Cells[j, header.Length] = w.checkdate;

                       
                        }
  
                    }

                    j++;
                }
            }

            AutoFitColumns("A2", j+1, header.Length);
            }
            catch (Exception e) { MessageAlert.Alert(e.ToString ()); }
        }

  

        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Borders.LineStyle = 1;
            _range.ColumnWidth = 12;
            _range.Columns.AutoFit();
            _range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
          
        }

        private void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
            _range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            _range.Borders.LineStyle = 1;
            _range.WrapText = true;
            _range.RowHeight = 13.5;

        }
        /// <summary>
        /// 创建一个Excel程序实例
        /// </summary>
        private void CreateExcelRef()
        {
            _excelApp = new Excel.Application();
            _books = (Excel.Workbooks)_excelApp.Workbooks;
            _book = (Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Excel.Sheets)_book.Worksheets;
            _sheet = (Excel._Worksheet)(_sheets.get_Item(1));
        }
    }
}
