using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
 public  interface IBorrow
    {
     bool add(borrow borrowmodel);
     string addreturnid(borrow borrowmodel);
     bool AddMany(List <borrow > borrowlist);
     bool Update(borrow  borrowmodel);
     bool Del(borrow borrowmodel);
     List<borrow> SelectWrenchOrBUser(string stime, string etime);
     List<borrow> SelectWrenchOrBUser(string stime,string etime, string guid);
     List<borrow> SelectWrenchOrBUser(string stime,string etime, string guid,bool? isreturn=false);
     List<borrow> SelectWrenchOrBUser(string stime, string etime,bool isreturn = false);
     List<borrow> SelectByUser(string user);
     List<borrow> SelectByWrench(string wrench);
     List<borrow> SelectByUser(string user,bool isreturn=true);
     List<borrow> SelectByWrench(string wrench,bool isreturn=true);
     List<borrow> SelectByWrench(string wrench,string user, bool isreturn = true);
     List<borrow> SelectByCondition(Dictionary <string ,string >condition);
    }
}
