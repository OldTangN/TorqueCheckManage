using QDDL.Comm;
using QDDL.DAL.Service;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.MySql
{
   public  class MySqlBorrow:IBorrow 
    {
       string _webip = OperationConfig.GetValue("WebServiceIp");


       public bool add(Model.borrow borrowmodel)
       {
           return ServerHelp.addSingleInfoNotReturnID<borrow>(borrowmodel ,_webip );
       }

       public string addreturnid(Model.borrow borrowmodel)
       {
           return ServerHelp.addSingleInfoReturnID<borrow>(borrowmodel ,_webip ).ToString ();
       }


       public bool AddMany(List<borrow> borrowlist)
       {
           throw new NotImplementedException();
       }

       public bool Update(borrow borrowmodel)
       {
           string tablename = typeof(borrow ).Name;
           string strsql = string.Format("WrenchID_id='{0}',borrowUser='{1}',borrowOperator='{2}',returnUser='{3}',returnOperator='{4}',borrowDate='{5}',returnDate='{6}',comment='{7}',is_return={8}", borrowmodel.WrenchID ,borrowmodel.borrowUser ,borrowmodel.borrowOperator ,borrowmodel.returnUser ,borrowmodel.returnOperator ,borrowmodel.borrowDate ,borrowmodel .returnDate,borrowmodel .comment ,borrowmodel.is_return );
           string contation = string.Format("guid='{0}'", borrowmodel.guid);
           return ServerHelp.updateByWhere(tablename, _webip, contation, strsql);
       }

       public List<borrow> SelectByUser(string user,bool isreturn=true )
       {
           string contion = string.Format("borrowUser='{0}' and is_return={1}", user ,isreturn );
           return ServerHelp.findDataByCondition<borrow>(contion, _webip);
       }

       public List<borrow> SelectByWrench(string wrench,bool isreturn=true)
       {
           string contion = string.Format("WrenchID_id='{0}' and is_return={1}", wrench,isreturn );
           return ServerHelp.findDataByCondition<borrow>(contion, _webip); 
       }
       public List<borrow> SelectByWrench(string wrench, string user, bool isreturn = true)
       {
           string contion = string.Format("WrenchID_id='{0}' and is_return={1} and borrowUser='{2}'", wrench, isreturn,user);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip);
       }
       public List<borrow> SelectByCondition(Dictionary<string, string> condition)
       {
           string tablename = typeof(borrow ).Name;
           string str = "";
           foreach (var d in condition)
           {
               str += string.Format("{0}='{1}'" + " and ", d.Key, d.Value);
           }
           string scontion = str.Count() > 4 ? str.Remove(str.Count() - 4) : "";
           return ServerHelp.findDataByCondition<borrow>(scontion, _webip);
       }


       public List<borrow> SelectByUser(string user)
       {
           string contion = string.Format("borrowUser='{0}'", user);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip);
       }

       public List<borrow> SelectByWrench(string wrench)
       {
           string contion = string.Format("WrenchID_id='{0}'", wrench);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip); 
       }


       public List<borrow> SelectWrenchOrBUser(string stime,string etime, string guid)
       {
           string contion = string.Format("(borrowDate>='{0}' and borrowDate<='{1}') and ( WrenchID_id='{2}' or borrowUser='{3}')", stime ,etime , guid, guid);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip);  
       }


       public List<borrow> SelectWrenchOrBUser(string stime,string etime,string guid, bool? isreturn=false)
       {
           string contion = string.Format(" (is_return={0} and borrowDate>='{1}' and borrowDate<='{2}') and (WrenchID_id='{3}' or borrowUser='{4}')", isreturn??false, stime ,etime ,guid, guid);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip);  
       }


       public List<borrow> SelectWrenchOrBUser(string stime, string etime, bool isreturn = false)
       {
           string contion = string.Format(" (is_return={0} and borrowDate>='{1}' and borrowDate<='{2}')", isreturn, stime, etime);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip); 
       }


       public List<borrow> SelectWrenchOrBUser(string stime, string etime)
       {
           string contion = string.Format(" (borrowDate>='{0}' and borrowDate<='{1}')", stime, etime);
           return ServerHelp.findDataByCondition<borrow>(contion, _webip); 
       }


       public bool Del(borrow borrowmodel)
       {
           string contion = string.Format("id='{0}'", borrowmodel.id);
           return ServerHelp.deleteDataByWhere<borrow>(_webip, contion);
       }
    }
}
