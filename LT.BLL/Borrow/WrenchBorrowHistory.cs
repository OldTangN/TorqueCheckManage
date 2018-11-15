using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.BLL.Borrow
{
  public   class WrenchBorrowHistory
    {
      IBorrow Borrow = new MySqlBorrow();
      IUser User = DataAccess.CreateUser();
      IWrench Wrench = DataAccess.CreateWrench();

      public List<BorrowHistory> GetByUser(List<borrow> bl)
      {
          List<BorrowHistory> bhl = new List<BorrowHistory>();        
          if (bl != null && bl.Count > 0)
          {
              foreach (borrow b in bl)
              {
                  users  bu = User.SelectByguid(b.borrowUser);
                  users bop = User.SelectByguid(b.borrowOperator);
                  users rop = User.SelectByguid(b.returnOperator);
                  users ru = User.SelectByguid(b.returnUser);
                  wrench w = Wrench.selectByguid(b.WrenchID);
                  if (w != null)
                  {
                      bhl.Add(new BorrowHistory()
                      {
                          wrenchbarcode = w.wrenchBarCode,
                          wrenchcode = w.wrenchCode,
                          borrowusercard =bu==null?"":bu.cardID,
                          borrowusername =bu==null?"":bu.username,
                          factory =w.factory ,
                          rang =w.rangeMin .ToString ("f2")+"~"+w.rangeMax .ToString ("f2"),
                          borrowdate =b.borrowDate .Replace ('T',' '),
                          borrowoperator =bop.username,
                          returnuser =ru==null?"":ru.username ,
                          returnoperator =rop==null?"":rop.username ,
                          returndate =rop==null?"":b.returnDate.Replace ('T',' '),
                          wrenchcommon =w.comment,
                          isreturn =b.is_return
                      });
                  }
              }
       
          }
          return bhl;
      }
    }
}
