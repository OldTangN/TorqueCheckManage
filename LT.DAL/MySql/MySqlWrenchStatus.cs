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
 public    class MySqlWrenchStatus :IWrenchStatus 
    {
        string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.wrenchstatus wrenchstatusmodel)
        {
            return ServerHelp.addSingleInfoNotReturnID<wrenchstatus>(wrenchstatusmodel,_webip);
        }

        public List<Model.wrenchstatus> selectAll()
        {
            List <wrenchstatus >wrenchstatuslist=new List<wrenchstatus> ();
             ServerHelp.findList<wrenchstatus>(out wrenchstatuslist,_webip  );
             return wrenchstatuslist;
        }

        public Model.wrenchstatus selectByguid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid );
            return ServerHelp.findDataByCondition<wrenchstatus>(contion ,_webip ).FirstOrDefault ();
        }

        public bool update(Model.wrenchstatus wrenchstatusmodel)
        {
            string tableName = typeof(wrenchstatus).Name;
            string contion = string.Format("guid='{0}'",wrenchstatusmodel .guid );
            string sql = string.Format("statusName='{0}',statusDM='{1}',comment='{2}'",wrenchstatusmodel.statusName ,wrenchstatusmodel .statusDM ,wrenchstatusmodel .comment );
            return ServerHelp.updateByWhere(tableName ,_webip ,contion ,sql);
        }


        public wrenchstatus selectByName(string name)
        {
            string contion = string.Format("statusName='{0}'",name );
            return ServerHelp.findDataByCondition<wrenchstatus>(contion ,_webip ).FirstOrDefault ();
        }


        public wrenchstatus selectByStatusDM(string statusDM)
        {
            string contion = string.Format("statusDM='{0}'", statusDM);
            return ServerHelp.findDataByCondition<wrenchstatus>(contion, _webip).FirstOrDefault();
        }
        public bool Del(wrenchstatus wrenchstatus)
        {
            string contion = string.Format("id='{0}'", wrenchstatus.id);
            return ServerHelp.deleteDataByWhere<wrenchstatus>(_webip, contion);
        }
    }
}
