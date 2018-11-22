using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL
{
 public    interface IBorrowRecord
    {
     List<borrowrecord> selectByIsreturn(bool isreturn);
     List<borrowrecord> selectall();
     List<borrowrecord> select(string wrenchid,bool is_return);
     bool add(borrowrecord  borrowrecordmodel);
     bool addmany(List <borrowrecord > borrowrecordlist);
     bool update(borrowrecord borrowrecordmodel);
    }
}
