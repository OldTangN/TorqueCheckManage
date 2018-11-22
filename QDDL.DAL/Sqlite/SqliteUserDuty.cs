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
   public  class SqliteUserDuty:IUserDuty
    {
        string con = OperationConfig.GetString();
        public bool Add(Model.duties duty)
        {
            try
            {
                string sqldep = string.Format("select * from department where id='{0}'",duty.department);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        duty.department = dep.guid;
                }

                string sql =
                string.Format
                (
                "INSERT INTO duties(department_id,dutiesName,comment,guid) VALUES('{0}','{1}','{2}','{3}') ;select last_insert_rowid();",
                duty.department,duty.dutiesName,duty.comment,duty.guid
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

        public List<Model.duties> Select()
        {
            try
            {
                string sql = string.Format("select * from duties");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<duties>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public List<Model.duties> SelectByName(string name)
        {
            try
            {
                string sql = string.Format("select * from duties where dutiesName='{0}'",name);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<duties>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public List<Model.duties> SelectByDepartment(string departmentguid)
        {
            try
            {
                string sql = string.Format("select * from duties where department_id='{0}'", departmentguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<duties>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.duties SelectByGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from duties where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<duties>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(Model.duties duty)
        {

            try
            {
                string contation = string.Format("where guid='{0}'", duty.guid);
                string strsql = string.Format("department_id='{0}',dutiesName='{1}',comment='{2}'", duty.department, duty.dutiesName, duty.comment);
                string sql = "update duties set " + strsql + contation;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch 
            { 
            return false ;
            }
        }

        public bool Del(Model.duties duty)
        {
            try
            {
                string contation = string.Format(" where id='{0}'", duty.id);
                string sql = "delete from  duties " + contation;
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
