using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.DAL.Sqlite
{
   public  class SqliteSystemCheckset:ISystemCheckset
    {
        public bool add(Model.systemcheckset systemchecksetmodel)
        {
            throw new NotImplementedException();
        }

        public bool update(Model.systemcheckset systemchecksetmodel)
        {
            throw new NotImplementedException();
        }

        public List<Model.systemcheckset> selectBySystemname(string systemname)
        {
            throw new NotImplementedException();
        }
    }
}
