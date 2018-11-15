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
  public  class SqliteDepartment:IDepartment
    {
      string con = OperationConfig.GetString();
        public bool Add(Model.department department)
        {
            try
            {
                string sql =
                string.Format
                (
                "INSERT INTO department(departmentName,parentDepartment,delDepartment,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}') ;select last_insert_rowid()",
                department.departmentName,department.parentDepartment,department.delDepartment==true?1:0,department.comment,department.guid
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

        public bool Add(List<Model.department> listdepartment)
        {
            try
            {
                string sql = "";
                foreach (department department in listdepartment)
                {
                    sql +=
                     string.Format
                (
                "INSERT INTO department(departmentName,parentDepartment,delDepartment,comment,guid) VALUES('{0}',{1},'{2}',{3},'{4}') ;",
                department.departmentName, department.parentDepartment, department.delDepartment==true?1:0, department.comment, department.guid
                )+"\r";
                }
                sql += "select last_insert_rowid()";
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

        public List<Model.department> Select()
        {
            try
            {
                string sql = string.Format("select * from department");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.department> SelectByFlag(bool flag = false)
        {
            try
            {
                string sql = string.Format("select * from department where delDepartment={0}" ,flag==true?1:0);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.department> SelectByDelFlag(string parentid = "", bool delflag = false)
        {
            try
            {
                string sql = string.Format("select * from department where parentDepartment='{0}' and delDepartment={1}",parentid ,delflag==true?1:0);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.department> Select(string name, bool delflag = false)
        {
            try
            {
                string sql = string.Format("select * from department where departmentName='{0}' and delDepartment={1}", name, delflag==true?1:0);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.department SelectById(string Did)
        {
            try
            {
                string sql = string.Format("select * from department where id='{0}'",Did);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.department SelectByGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from department where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<department>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(Model.department department)
        {
            try
            {
                string contion = string.Format(" where guid='{0}'", department.guid);
                string strsql = string.Format("departmentName='{0}',parentDepartment='{1}',comment='{2}',delDepartment={3}", department.departmentName, department.parentDepartment, department.comment, department.delDepartment==true?1:0);
                string sql = "update department set " + strsql + contion;
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
