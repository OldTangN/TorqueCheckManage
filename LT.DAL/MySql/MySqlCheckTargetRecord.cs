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
  public   class MySqlCheckTargetRecord:ICheckTargetRecord 
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");

        public string AddReturnGuid(Model.torquecheckrecord torquecheckrecord)
        {
            return ServerHelp.addSingleInfoReturnID<torquecheckrecord>(torquecheckrecord, _webip).ToString ();
        }

  

        public List<Model.torquecheckrecord> SelectByCheckTargetID(string checktargetguid)
        {

            string contion = string.Format("TorqueCheckTargetID_id='{0}'",checktargetguid);
            return ServerHelp.findDataByCondition<torquecheckrecord>(contion,_webip);
        }


        public bool AddMany(List<Model.torquecheckrecord> listtorquecheckrecord)
        {
            return ServerHelp.addManyInfoReturnBool<torquecheckrecord>(listtorquecheckrecord,_webip );
        }


        public bool AddNotReturn(torquecheckrecord checkrecord)
        {
            return ServerHelp.addSingleInfoNotReturnID<torquecheckrecord>(checkrecord ,_webip );
        }


        public bool Del(torquecheckrecord checkrecord)
        {
            string contion = string.Format("id='{0}'", checkrecord.id);
            return ServerHelp.deleteDataByWhere<torquecheckrecord>(_webip, contion);
        }
    }
}
