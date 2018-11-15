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
  public   class SqliteUserRole:IUserRole
    {
      string con = OperationConfig.GetString();
        public List<Model.role> Select()
        {
            try
            {
                string sql = string.Format("select * from role");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<role>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool Add(Model.role userroler)
        {
            try
            {
                string sqldep = string.Format("select * from system where id='{0}'", userroler.system);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    system system = DataTableToList.GetList<system>(dps.Tables[0]).FirstOrDefault();
                    if (system != null)
                        userroler.system = system.guid;
                }

                string sql =
                string.Format
                (
                "INSERT INTO role(roleName,system_id,roleDM,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}') ;select last_insert_rowid()",
                 userroler.roleName,userroler .system ,userroler .roleDM,userroler.comment ,userroler.guid 
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

        public bool Add(List<Model.role> rolelist)
        {
            try
            {
                string sql = "";
                foreach (role userroler in rolelist)
                {
                    sql +=
                      string.Format
                (
                "INSERT INTO role(roleName,system_id,roleDM,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}')",
                 userroler.roleName, userroler.system, userroler.roleDM, userroler.comment, userroler.guid
                ) +"\r";
                }
                sql += ";select last_insert_rowid()";
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

        public bool Update(Model.role userrole)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", userrole.guid);
                string strsql = string.Format("roleName='{0}',system_id='{1}',roleDM='{2}',comment='{3}' ", userrole.roleName, userrole.system, userrole.roleDM, userrole.comment);
                string sql = "update role set " + strsql + contation;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
        }

        public List<Model.role> SelectBySysGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from role where system_id='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<role>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public List<Model.role> selectSysidandguid(string systemid, string guid)
        {
            try
            {
                string sql = string.Format("select * from role where system_id='{0}' and guid='{1}'",systemid, guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<role>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.role SelectByGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from role where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<role>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public Model.role SelectById(string id)
        {
            try
            {
                string sql = string.Format("select * from role where id='{0}'", id);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<role>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public bool Del(Model.role role)
        {
            try
            {
                string contation = string.Format(" where guid='{0}'", role.guid);
                string sql = "delete from role " + contation;
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
