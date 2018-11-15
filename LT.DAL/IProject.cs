using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
  public   interface IProject
    {
      bool add(system addsystem);
      string addReturnID(system addsystem);
      List <system > selectByname(string projectname);
      List <system > select();
      bool updata(system updatasystem);
      bool addmany(List <system > syslist);
    }
}
