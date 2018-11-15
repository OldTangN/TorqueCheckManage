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
   public  class MySqlProject:IProject 
    {
       string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.system addsystem)
        {
            return ServerHelp.addSingleInfoNotReturnID<system>(addsystem,_webip);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="projectname">where 后面的查询条件</param>
       /// <returns></returns>
        public List<system> selectByname(string projectname)
        {
            string contion = string.Format("systemName='{0}'",projectname );
            return ServerHelp.findDataByCondition<system>(contion, _webip);
        }

        public List <system > select()
        {
            throw new NotImplementedException();
        }

        public bool updata(Model.system updatasystem)
        {
            throw new NotImplementedException();
        }


        public string addReturnID(system addsystem)
        {
            return ServerHelp.addSingleInfoReturnID <system>(addsystem, _webip).ToString ();
        }


        public bool addmany(List<system> syslist)
        {
            return ServerHelp.addManyInfoReturnBool<system>(syslist ,_webip );
        }
    }
}
