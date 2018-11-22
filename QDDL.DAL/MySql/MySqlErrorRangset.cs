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
  public   class MySqlErrorRangset:IErrorRangset 
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.errorrangset errorrangsetmodel)
        {
            return ServerHelp.addSingleInfoNotReturnID<errorrangset>(errorrangsetmodel ,_webip );
        }

        public bool update(Model.errorrangset errorrangsetmodel)
        {
            string tableName = typeof(errorrangset).Name;
            string sql = string.Format("id='{0}',speciesID='{1}',rangmax='{2}',rangmin='{3}',unit='{4}',errorrangMax='{5}',comment='{6}',errorrangMin='{7}'",errorrangsetmodel.id ,errorrangsetmodel .speciesID ,errorrangsetmodel .rangmax ,errorrangsetmodel .rangmin ,errorrangsetmodel .unit ,errorrangsetmodel .errorrangMax ,errorrangsetmodel .comment,errorrangsetmodel.errorrangMin  );
            string contion = string.Format("id='{0}'",errorrangsetmodel .id );
            return ServerHelp.updateByWhere(tableName ,_webip ,contion,sql);
        }

        public List<Model.errorrangset> selectByspeciesid(string guid)
        {
            string contion = string.Format("speciesID_id='{0}'",guid );
            return ServerHelp.findDataByCondition<errorrangset>(contion ,_webip );
        }

        public List<Model.errorrangset> select()
        {
            List <errorrangset > el=new List<errorrangset> ();
             ServerHelp.findList<errorrangset>(out el,_webip );
             return el;
        }
    }
}
