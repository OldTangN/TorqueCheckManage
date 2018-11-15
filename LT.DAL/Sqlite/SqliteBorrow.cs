using LT.Comm;
using LT.DAL.SqliteServer;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LT.DAL.Sqlite
{
   public  class SqliteBorrow:IBorrow
    {
        string con = OperationConfig.GetNlbsString();
        public bool add(Model.borrow borrowmodel)
        {
            throw new NotImplementedException();
        }

        public string addreturnid(Model.borrow borrowmodel)
        {
            throw new NotImplementedException();
        }

        public bool AddMany(List<Model.borrow> borrowlist)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.borrow borrowmodel)
        {
            throw new NotImplementedException();
        }

        public bool Del(Model.borrow borrowmodel)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectWrenchOrBUser(string stime, string etime)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectWrenchOrBUser(string stime, string etime, string guid)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectWrenchOrBUser(string stime, string etime, string guid, bool? isreturn = false)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectWrenchOrBUser(string stime, string etime, bool isreturn = false)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectByUser(string user)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectByWrench(string wrench)
        {
            try
            {
                string sql = string.Format("select * from borrow where WrenchID_id='{0}' ", wrench);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<borrow>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.borrow> SelectByUser(string user, bool isreturn = true)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectByWrench(string wrench, bool isreturn = true)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectByWrench(string wrench, string user, bool isreturn = true)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrow> SelectByCondition(Dictionary<string, string> condition)
        {
            throw new NotImplementedException();
        }
    }
}
