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
  public   class SqliteUserToRole:IUserToRole
    {
        string con = OperationConfig.GetString();
        public List<Model.UserToRoleModel.usertorole> select()
        {
            try
            {
                string sql = string.Format("select * from usertorole");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<QDDL.Model.UserToRoleModel.usertorole>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool add(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            try
            {
                string sqldep = string.Format("select * from users where id='{0}'", usertorolemodel.user);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    users dep = DataTableToList.GetList<users>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        usertorolemodel.user = dep.guid;
                }
                string sqlduty = string.Format("select * from role where id='{0}'", usertorolemodel.role);
                dps = SqliteHelper.ExecuteDataSet(con, sqlduty, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    role dep = DataTableToList.GetList<role>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                    usertorolemodel.role = dep.guid;
                }
                string sql =
                string.Format
                (
                "INSERT INTO usertorole(user_id,role_id) VALUES('{0}','{1}') ;select last_insert_rowid()",
                usertorolemodel.user,usertorolemodel.role
                );
             
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
               // MessageAlert.Alert(ex.ToString());
                return false;

            }
        }

        public List<Model.UserToRoleModel.usertorole> selectbyuserid(string userguid)
        {
            try
            {
                string sql = string.Format("select * from usertorole where user_id='{0}'",userguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<QDDL.Model.UserToRoleModel.usertorole>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid)
        {
            try
            {
                string sql = string.Format("select * from usertorole where role_id='{0}'", roleguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<QDDL.Model.UserToRoleModel.usertorole>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid, string userguid)
        {
            try
            {
                string sql = string.Format("select * from usertorole where user_id='{0}' and role_id='{1}'", userguid,roleguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<QDDL.Model.UserToRoleModel.usertorole>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool update(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            try
            {
                string strsql = string.Format(" role_id='{0}'", usertorolemodel.role);
                string contation = string.Format("where user_id='{0}'", usertorolemodel.user);
                string sql = "update usertorole set " + strsql + contation;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
        }

        public bool delete(Model.UserToRoleModel.usertorole usertorolemodel)
        {
            try
            {
                string contation = string.Format("where user_id='{0}'", usertorolemodel.user);
                string sql = "delete from usertorole " + contation;
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
