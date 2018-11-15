using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
  public   interface IWrenchStatus
    {
       bool add(wrenchstatus wrenchstatusmodel );
       List<wrenchstatus> selectAll();
       wrenchstatus selectByguid(string guid);
       wrenchstatus selectByName(string name);
       wrenchstatus selectByStatusDM(string statusDM);
       bool update(wrenchstatus wrenchstatusmodel);
       bool Del(wrenchstatus wrenchstatus);
    }
}
