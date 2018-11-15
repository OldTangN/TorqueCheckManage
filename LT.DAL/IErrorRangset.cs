using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
  public   interface IErrorRangset
    {
      bool add(errorrangset errorrangsetmodel);
      bool update(errorrangset errorrangsetmodel);
    List <  errorrangset> selectByspeciesid(string guid);
    List<errorrangset> select();
    }
}
