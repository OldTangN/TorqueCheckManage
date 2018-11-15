using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
  public   interface IDepartment
    {
      bool Add(Model .department  department);
      bool Add(List <Model .department >listdepartment);
      List<Model .department > Select();
      List<Model.department> SelectByFlag(bool flag = false);
      List<Model.department> SelectByDelFlag(string parentid="", bool delflag=false);
      List<Model.department> Select(string name, bool delflag = false);
      Model .department  SelectById(string Did);
      Model.department SelectByGuid(string guid);
     
      bool Update(Model .department  department);
    }
}
