using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LT.BLL.Borrow
{
  public   class BorrowExcel
    {
    }

  public class BorrowOutToExcel
  {
      DataTable Header(DataTable dt1)
      {
          DataTable dt = new DataTable();
          dt = dt1;
          List<string> headlist = new List<string>() { "序 号", "扳手条码", "扳手编号", "借用人卡号", "借用人姓名", "生产厂家", "量程(N.m)", "借出日期","借出操作人" ,"归还人","归还日期","归还操作人"};

          foreach (string s in headlist)
          {
              dt.Columns.Add(s);
          }
          return dt;
      }
      public DataTable ToTable(List<BorrowHistory> bh)
      {
          DataTable dt = new DataTable();
          dt = Header(dt);
          int count = 0;
          object[] values = new object[12];
          foreach (BorrowHistory b in bh)
          {
              count++;
              values[0] = count.ToString();
              values[1] = b.wrenchbarcode ;
              values[2] = b.wrenchcode+"\t" ;
              values[3] = b.borrowusercard ;
              values[4] = b.borrowusername ;
              values[5] = b.factory ;
              values[6] = b.rang ;
              values[7] = b.borrowdate  + "\t";
             values[8] = b.borrowoperator ;
              values [9]=b.returnuser ;
              values [10]=b.returndate+"\t";
              values [11]=b.returnoperator ;
              dt.Rows.Add(values);
          }
          return dt;
      }
  }
}
