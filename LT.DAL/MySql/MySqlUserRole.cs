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
   public  class MySqlUserRole:IUserRole 
    {
       string _webip = OperationConfig.GetValue("WebServiceIp");
        public List<role> Select()
       {
           List<role> rolelist = new List<role>();
            ServerHelp.findList<role>(out  rolelist ,_webip);
            return rolelist;
        }

        public bool Add(role userroler)
        {
            return ServerHelp.addSingleInfoNotReturnID<role >(userroler ,_webip );
        }

        public bool Add(List<role> rolelist)
        {
            throw new NotImplementedException();
        }

        public bool Update(role userrole)
        {
           // return ServerHelp.updateSingleInfo<role>(userrole.id ,userrole ,_webip );
            string tbname = typeof(role).Name;
            string contion=string .Format ("guid='{0}'",userrole.guid );
            string strsql=string .Format ("roleName='{0}',system_id='{1}',roleDM='{2}',comment='{3}' ",userrole .roleName ,userrole .system ,userrole .roleDM ,userrole .comment );

            return ServerHelp.updateByWhere(tbname ,_webip,contion ,strsql);
        }

       /// <summary>
        /// SelectBySysGuid 根据项目的id
       /// </summary>
       /// <param name="guid"></param>
       /// <returns></returns>
        public List<role> SelectBySysGuid(string guid)
        {
            string condition = string.Format("system_id='{0}'", guid);
            return ServerHelp.findDataByCondition<role>(condition, _webip);
        }


        public role SelectByGuid(string guid)
        {
            string condition = string.Format("guid='{0}'",guid );
            return ServerHelp.findDataByCondition<Model.role>(condition,_webip ).FirstOrDefault ();
        }


        public role SelectById(string id)
        {
            string contion = string.Format("id='{0}'",id);
            return ServerHelp.findDataByCondition<role>(contion,_webip ).FirstOrDefault ();
        }


        public List<role> selectSysidandguid(string systemid, string guid)
        {
            string condition = string.Format("guid='{0}' and system_id='{1}'", guid,systemid);
            return ServerHelp.findDataByCondition<role>(condition ,_webip);
        }


        public bool Del(role role)
        {
            string contion = string.Format("id='{0}'", role.id);
            return ServerHelp.deleteDataByWhere<role>(_webip, contion);
        }
    }
}
