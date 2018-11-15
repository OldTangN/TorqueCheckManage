using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.DAL.Sqlite
{
   public  class SqliteErrorRangset:IErrorRangset
    {
        public bool add(Model.errorrangset errorrangsetmodel)
        {
            throw new NotImplementedException();
        }

        public bool update(Model.errorrangset errorrangsetmodel)
        {
            throw new NotImplementedException();
        }

        public List<Model.errorrangset> selectByspeciesid(string guid)
        {
            throw new NotImplementedException();
        }

        public List<Model.errorrangset> select()
        {
            throw new NotImplementedException();
        }
    }
}
