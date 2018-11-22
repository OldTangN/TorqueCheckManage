using QDDL.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.DAL.Sqlite
{
   public  class SqliteBorrowRecord:IBorrowRecord
    {
        string con = OperationConfig.GetNlbsString();
        public List<Model.borrowrecord> selectByIsreturn(bool isreturn)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrowrecord> selectall()
        {
            throw new NotImplementedException();
        }

        public List<Model.borrowrecord> select(string wrenchid, bool is_return)
        {
            throw new NotImplementedException();
        }

        public bool add(Model.borrowrecord borrowrecordmodel)
        {
            throw new NotImplementedException();
        }

        public bool addmany(List<Model.borrowrecord> borrowrecordlist)
        {
            throw new NotImplementedException();
        }

        public bool update(Model.borrowrecord borrowrecordmodel)
        {
            throw new NotImplementedException();
        }
    }
}
