using QDDL.Comm;
using QDDL.DAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.MySql
{
   public  class MySqlUserToRole:IUserToRole 
    {
       string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            return ServerHelp.addSingleInfoNotReturnID <QDDL.Model.UserToRoleModel.usertorole>(usertorolemodel ,_webip);
        }

        public List<Model.UserToRoleModel.usertorole> selectbyuserid(string userguid)
        {
            string contion = string.Format("user_id='{0}'",userguid);
            return ServerHelp.findDataByCondition<QDDL.Model.UserToRoleModel.usertorole>(contion,_webip );
        }

        public List<Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid)
        {
            string contion = string.Format("role_id='{0}'", roleguid);
            return ServerHelp.findDataByCondition<QDDL.Model.UserToRoleModel.usertorole>(contion, _webip);
        }


        public bool update(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            string sql = string.Format("role_id='{0}'",usertorolemodel.role);
            string contion = string.Format("user_id='{0}'",usertorolemodel .user );
            string tablename=typeof (QDDL.Model.UserToRoleModel.usertorole).Name ;
            return ServerHelp.updateByWhere(tablename ,_webip ,contion ,sql );
        }

        public List<Model.UserToRoleModel.usertorole> select()
        {
          
            List<QDDL.Model.UserToRoleModel.usertorole> url = new List<Model.UserToRoleModel.usertorole>();
             ServerHelp.findList(out url,_webip);
             return url;

        }


        public List<Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid, string userguid)
        {
            string contion = string.Format("role_id='{0}' and user_id='{1}'", roleguid,userguid );
            return ServerHelp.findDataByCondition<QDDL.Model.UserToRoleModel.usertorole>(contion, _webip);
        }


        public bool delete(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            string contion = string.Format("id='{0}'", usertorolemodel.id  );
            return ServerHelp.deleteDataByWhere<Model.UserToRoleModel.usertorole>(_webip, contion);
        
        }
    }
}
