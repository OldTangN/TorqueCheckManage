using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL
{
   public  interface IUserRole
    {
       List<Model.role > Select();
       bool Add(Model .role  userroler);
       bool Add(List<Model .role > rolelist);
       bool Update(Model .role  userrole);
       List<Model.role> SelectBySysGuid(string  guid);
       List<Model.role> selectSysidandguid(string systemid,string guid);
       Model.role SelectByGuid(string guid);
       Model.role SelectById(string id);
       bool Del(Model.role role);
    }
}
