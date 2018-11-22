using QDDL.Comm;
using QDDL.DAL.SqliteServer;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QDDL.DAL.Sqlite
{
  public  class SqliteProject:IProject
    {
      string con = OperationConfig.GetString();
        public bool add(Model.system addsystem)
        {
            try
            {
                string sql =
                string.Format
                (
                "INSERT INTO system (systemName,systemDM,comment,guid) VALUES('{0}','{1}','{2}','{3}') ;select last_insert_rowid()",
                addsystem.systemName, addsystem.systemDM, addsystem.comment, addsystem.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
        }

        public string addReturnID(Model.system addsystem)
        {
            try
            {
                string sql =
                string.Format
                (
                "INSERT INTO system(systemName,systemDM,comment,guid) VALUES('{0}','{1}','{2}','{3}') ;select last_insert_rowid()",
                addsystem.systemName, addsystem.systemDM, addsystem.comment, addsystem.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                return ds.ToString();
            }
            catch { return "-1"; }

        }

        public List<Model.system> selectByname(string projectname)
        {
           
         
            try
            {
                string sql = string.Format("select * from system  where systemName='{0}'", projectname);
              
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                   
                    return DataTableToList.GetList<system>(ds.Tables[0]);
                }
                return null;
            }
            catch(Exception ex)
            {
               
                LogUtil.WriteLog(typeof(string), "执行查询语句异常！"+ex);
                return null;
            }
        }

        public List<Model.system> select()
        {
            try
            {
                string sql = string.Format("select * from system");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<system>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool updata(Model.system updatasystem)
        {
            throw new NotImplementedException();
        }

        public bool addmany(List<Model.system> syslist)
        {
            try
            {
                string sql = "";
                foreach (system addsystem in syslist)
                {
                    sql +=
                   string.Format
                   (
                   "INSERT INTO system(systemName,systemDM,comment,guid) VALUES('{0}','{1}','{2}','{3}') ",
                   addsystem.systemName, addsystem.systemDM, addsystem.comment, addsystem.guid
                   );
                }
                sql += ";select last_insert_rowid()";
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
            
        }
    }
}
