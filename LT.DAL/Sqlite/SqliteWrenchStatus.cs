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
  public   class SqliteWrenchStatus:IWrenchStatus
    {
      string con = OperationConfig.GetNlbsString();
        public bool add(Model.wrenchstatus wrenchstatusmodel)
        {
            try
            {
                string sql =
                string.Format
                (
                "INSERT INTO wrenchstatus(statusName,statusDM,comment,guid) VALUES('{0}','{1}','{2}','{3}') ;select last_insert_rowid()",
                 wrenchstatusmodel.statusName,wrenchstatusmodel.statusDM,wrenchstatusmodel.comment,wrenchstatusmodel.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch
            {
                return false;

            }
        }

        public List<Model.wrenchstatus> selectAll()
        {
            try
            {
                string sql = string.Format("select * from wrenchstatus");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchstatus>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.wrenchstatus selectByguid(string guid)
        {
            try
            {
                string sql = string.Format("select * from wrenchstatus where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchstatus>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.wrenchstatus selectByName(string name)
        {
            try
            {
                string sql = string.Format("select * from wrenchstatus where statusName='{0}'", name);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchstatus>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.wrenchstatus selectByStatusDM(string statusDM)
        {
            try
            {
                string sql = string.Format("select * from wrenchstatus where statusDM='{0}'", statusDM);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchstatus>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool update(Model.wrenchstatus wrenchstatusmodel)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", wrenchstatusmodel.guid);
                string strsql = string.Format("statusName='{0}',statusDM='{1}',comment='{2}'", wrenchstatusmodel.statusName, wrenchstatusmodel.statusDM, wrenchstatusmodel.comment);
                string sql = "update wrenchstatus set " + strsql + contation;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Del(Model.wrenchstatus wrenchstatus)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", wrenchstatus.guid);
                string sql = "delete from wrenchstatus " + contation;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
