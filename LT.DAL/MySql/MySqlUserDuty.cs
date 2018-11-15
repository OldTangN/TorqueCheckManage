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
  public   class MySqlUserDuty:IUserDuty 
    {

      string _webip = OperationConfig.GetValue("WebServiceIp");
        public List<Model.duties> Select()
        {
            List<duties> dl = new List<duties>();
            ServerHelp.findList<duties>(out dl,_webip);
            return dl;
        }

        public List < Model.duties> SelectByName(string name)
        {
            
            string contion = string.Format("dutiesName='{0}'",name);
            return ServerHelp.findDataByCondition<duties >(contion,_webip);
        }


        public List<Model.duties> SelectByDepartment(string department)
        {
            string contion = string.Format("department_id='{0}'", department);
            return ServerHelp.findDataByCondition<duties>(contion, _webip);
        }

        public bool Update(Model.duties duty)
        {
            string tableName=typeof (duties).Name ;
            string contion = string.Format("guid='{0}'",duty.guid);
            string sql = string.Format("department_id='{0}',dutiesName='{1}',comment='{2}'",duty.department,duty .dutiesName ,duty.comment);
            return ServerHelp.updateByWhere(tableName ,_webip ,contion ,sql);
        }

        public bool Add(duties duty)
        {
            return ServerHelp.addSingleInfoNotReturnID(duty,_webip);
        }


        public duties SelectByGuid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid);
            List<duties> duty = new List<duties>();
            duty= ServerHelp.findDataByCondition <duties >(contion,_webip);
            if (duty.Count > 0) { return duty.FirstOrDefault(); }
            return new duties();
        }


        public bool Del(duties duty)
        {
            string contion = string.Format("id='{0}'", duty.id);
            return ServerHelp.deleteDataByWhere<duties>(_webip, contion);
        }
    }
}
