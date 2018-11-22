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
  public   class MySqlSystemCheckset:ISystemCheckset
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.systemcheckset systemchecksetmodel)
        {
            return ServerHelp.addSingleInfoNotReturnID<systemcheckset>(systemchecksetmodel ,_webip );
        }

        public bool update(Model.systemcheckset systemchecksetmodel)
        {
            string contion = string.Format("systemname='{0}'",systemchecksetmodel .systemname );
            string sql = string.Format("id='{0}',count='{1}',arry='{2}',boundaryvalue='{3}',throwvalue='{4}',comment='{5}'",systemchecksetmodel .id ,systemchecksetmodel .count ,systemchecksetmodel .arry ,systemchecksetmodel .boundaryvalue ,systemchecksetmodel .throwvalue ,systemchecksetmodel .comment );
          string tablename=typeof (systemcheckset ).Name ;
          return ServerHelp.updateByWhere(tablename ,_webip ,contion ,sql );
            
        }

        public List<Model.systemcheckset> selectBySystemname(string systemname)
        {
            string contion=string .Format ("systemname='{0}'",systemname );
            return ServerHelp.findDataByCondition<systemcheckset>(contion ,_webip );
        }
    }
}