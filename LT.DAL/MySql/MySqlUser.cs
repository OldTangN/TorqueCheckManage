using LT.Comm;
using LT.DAL.Service;
using LT.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL.MySql
{
   public  class MySqlUser:IUser
    {
       string _webserviceip = OperationConfig.GetValue("WebServiceIp");
       IDepartment Department = DataAccess.CreateDepartment();
       IUserRole UserRole = DataAccess.CreateUserRole();

      public   List<users> Select()
        {
            List<Model .users> userlist = new List<Model .users >();
            ServerHelp.findList  <Model.users >(out userlist ,_webserviceip);
            return userlist;
        }

       /// <summary>
       /// 根据卡号查询
       /// </summary>
       /// <param name="CardId"></param>
       /// <returns></returns>
        public users Select(string CardId)
        {
            string contion = string.Format("cardID='{0}'",CardId );
           return  ServerHelp.findDataByCondition<users>(contion ,_webserviceip ).FirstOrDefault ();
        }

        public users Select(string name, string password)
        {

            string contion = string.Format("username='{0}' and password='{1}' and is_staff={2}",name ,password ,1);
           return  ServerHelp.findDataByCondition<users>(contion,_webserviceip ).FirstOrDefault();

        }

        public List<users> SelectByName(string Name)
        {
            string conction=string.Format ("username='{0}'",Name );
            return ServerHelp.findDataByCondition<users >(conction ,_webserviceip);
        }


       /// <summary>
       /// /添加用户时需要验证用户名 和卡号  是否存在
       /// 
       /// </summary>
       /// <param name="user"></param>
       /// <returns></returns>
        public bool Add(users user)
        {
            return ServerHelp.addSingleInfoNotReturnID<users>(user,_webserviceip);
        }

        public bool Add(List<users> userlist)
        {
            throw new NotImplementedException();
        }

        public bool Update(users user)
        {  
            string tablename=typeof (users ).Name ;
      
            string strsql = string.Format("username='{0}',password='{1}',is_staff={2},empID='{3}',cardID='{4}',phoneNumber='{5}',IDNum='{6}',mail='{7}',department_id='{8}',comment='{9}',is_superuser={10},duties_id='{11}'",user.username ,user.password ,user.is_staff ,user.empID ,user.cardID ,user.phoneNumber ,user.IDNum ,user.mail ,user.department    ,user.comment ,user .is_superuser,user.duties );
           string contation = string.Format("guid='{0}'",user.guid );
            return ServerHelp.updateByWhere(tablename,_webserviceip ,contation ,strsql);
        }


        public string addreturnid(users user)
        {
            return ServerHelp.addSingleInfoReturnID<users>(user, _webserviceip).ToString();
        }


        public List<users> SelectByContion(Dictionary<string, string> contion)
        {
            string tablename = typeof(users).Name;
            string str = "";
            foreach (var d in contion) {
                str += string.Format("{0}='{1}'"+" and ",d.Key  ,d.Value );
            }
            string scontion =str .Count()>4? str.Remove(str.Count ()-4):"";
            return ServerHelp.findDataByCondition<users>(scontion ,_webserviceip);

        }


        public users SelectByguid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid );
            return ServerHelp.findDataByCondition<users>(contion ,_webserviceip ).FirstOrDefault ();
        }


        public bool Delete(users user)
        {
            string sql = string.Format("guid='{0}'", user.guid);
            return ServerHelp.deleteDataByWhere<users>(_webserviceip ,sql);
        }


        public List <users> SelectByCard(string cardid, string password)
        {
            string contion = string.Format("cardID='{0}' and password='{1}'",cardid ,password );
            return ServerHelp.findDataByCondition<users>(contion,_webserviceip);
        }


        public List<users> SelectByCode(string empID)
        {
            string conction = string.Format("empID='{0}'", empID);
            return ServerHelp.findDataByCondition<users>(conction, _webserviceip);
        }


        public List<users> SelectByCBcode(string cardid, string empID)
        {
            string conction = string.Format("empID='{0}' and cardID='{1}'", empID,cardid );
            return ServerHelp.findDataByCondition<users>(conction, _webserviceip);
        }


        public List<users> SelectNameOrCardid(string contion)
        {
            string conction = string.Format("username='{0}' or cardID='{1}'", contion, contion);
            return ServerHelp.findDataByCondition<users>(conction, _webserviceip);
        }
    }
    
}
