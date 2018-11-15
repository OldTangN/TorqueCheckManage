using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
  public   interface ISystemCheckset
    {
      bool add(systemcheckset systemchecksetmodel);
      bool update(systemcheckset systemchecksetmodel);
      List<systemcheckset> selectBySystemname(string systemname);
    }
}
