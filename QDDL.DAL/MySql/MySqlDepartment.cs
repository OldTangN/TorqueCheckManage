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
   public  class MySqlDepartment:IDepartment
    {

       string _webIp = OperationConfig.GetValue("WebServiceIp");
        public bool Add(Model.department department)
        {
          return   ServerHelp.addSingleInfoNotReturnID<Model .department>(department,_webIp);
        }

        public bool Add(List<Model.department> listdepartment)
        {
            throw new NotImplementedException();
        }

        public List<Model.department> Select()
        {
            List<Model.department> dep = new List<Model.department>();
              ServerHelp.findList<Model.department>( out dep,_webIp);
              return dep;
        }

        public List<Model.department> Select(string name, bool delflag = false)
        {
            string contion = string.Format("departmentName='{0}' and parentDepartment='{1}'", name,delflag );
            return ServerHelp.findDataByCondition<Model.department>(contion, _webIp);
        }

        public Model.department SelectById(string Did)
        {
            string contion = string.Format("id='{0}'",Did );
            return ServerHelp.findDataByCondition<Model.department>(contion ,_webIp ).FirstOrDefault ();
        }

        public bool Update(Model.department departmentmodel)
        {
            string tableName = typeof(department).Name;
            string contion = string.Format("guid='{0}'",departmentmodel.guid );
            string sql = string.Format("departmentName='{0}',parentDepartment='{1}',comment='{2}',delDepartment={3}",departmentmodel .departmentName ,departmentmodel.parentDepartment ,departmentmodel .comment ,departmentmodel.delDepartment );
            return ServerHelp.updateByWhere(tableName ,_webIp ,contion ,sql);
        }

        public Model.department SelectByGuid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid );
            return ServerHelp.findDataByCondition<Model .department >(contion,_webIp).FirstOrDefault ();
        }
    
        public List<department> SelectByDelFlag(string parentid = "", bool delflag = false)
        {
            string contion = string.Format("delDepartment={0} and parentDepartment='{1}'", delflag,parentid );
            return ServerHelp.findDataByCondition<Model.department>(contion, _webIp); 
        }


        public List<department> SelectByFlag(bool flag = false)
        {
            string contion = string.Format("delDepartment={0}", flag);
            return ServerHelp.findDataByCondition<Model.department>(contion, _webIp); 
        }
    }
}
