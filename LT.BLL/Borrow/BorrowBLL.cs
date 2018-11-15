using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.BLL.Borrow
{
 public    class BorrowBLL
    {

//借入借出需要分开重构

     users _user = new users();
     users _opertaor = new users();
     List<ToolModel > _wrenchlist = new List<ToolModel >();
     IBorrow Borrow = new MySqlBorrow();
     IBorrowRecord BorrowRecord = new MySqlBorrowRecord();
     IWrench Wrench = DataAccess.CreateWrench();
     IWrenchStatus WrenchSataus = DataAccess.CreateWrenchStatus();
     public BorrowBLL(users user,users operation,List <ToolModel  > wrenchlist) {
         _user = user;
         _opertaor = operation;
         _wrenchlist = wrenchlist;
     }
     string  addborrow() {
         borrow b = new borrow() { borrowUser =_user.guid  ,borrowOperator =_opertaor.guid ,borrowDate =DateTime .Now.ToString ("yyyy-MM-dd hh:mm:ss"),guid =Guid .NewGuid().ToString ()};
        return  Borrow.addreturnid (b);    
     }
      bool  addborrowrecode(string borrowid) {
        // string id = addborrow();
         List <borrowrecord > borrowrecorlist=new List<borrowrecord> ();
         foreach (ToolModel  w in _wrenchlist) {
             borrowrecord br = new borrowrecord() { 
                 //BorrowID =id,
                 WrenchID =w.id.ToString () ,
                 is_return =false ,guid =Guid .NewGuid ().ToString ()};
             borrowrecorlist.Add(br );
         
         }
       return   BorrowRecord.addmany(borrowrecorlist);
     }
      bool updaterecode() {
          foreach (wrench w in _wrenchlist) {
            List <borrowrecord>bc = BorrowRecord.select(w.guid ,false);
            foreach (borrowrecord b in bc) {
                b.is_return = true;
                b.returnUser = _user.guid;
                b.returnOperator = _opertaor.guid;
                b.returnDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                if (!BorrowRecord.update(b))
                    return false;
            }
          }
          return true;
      }


   bool updatewrench(string status) {
   
         List<wrench> wr = new List<wrench>();
         wrenchstatus ws = WrenchSataus.selectByName(status);
         foreach (wrench w in _wrenchlist) {
             w.status = ws.guid;
             if (!Wrench.updata(w))
                 return false;
         }
         return true;
       
     }

   public bool   BorrowWrench() {
       try
       {
           string borrowid = addborrow();
           if (!string.IsNullOrEmpty(borrowid))
           {
               if (addborrowrecode(borrowid))
               {

                 return   updatewrench("借出");

               }
           }
           return false;
       }
       catch { return false; }
   }

   public bool RreturnWrench() {
       if (updaterecode()) {
          return  updatewrench("正常");
       }
       return false;
   }

    }
}
