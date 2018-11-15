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
   public  class SqliteUser:IUser
    {
       string con = OperationConfig.GetString();
    //   IDepartment department = new SqliteDepartment();
        public List<Model.users> Select()
        {
            try
            {
                string sql = string.Format("select * from users");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch 
            {
                return null; 
            }
        }

        public Model.users Select(string CardId)
        {
            try
            {
                string sql = string.Format("select * from users where cardID='{0}'",CardId);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public List<Model.users> SelectByCBcode(string cardid, string empID)
        {
            try
            {
                string sql = string.Format("select * from users where cardID='{0}' and empID='{1}'", cardid,empID);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.users SelectByguid(string guid)
        {
            try
            {
                string sql = string.Format("select * from users where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.users> SelectByCode(string empID)
        {
            try
            {
                string sql = string.Format("select * from users where empID='{0}'", empID);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            } 
        }

        public Model.users Select(string name, string password)
        {
            try
            {
                string sql = string.Format("select * from users where username='{0}' and password='{1}'", name,password);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.users> SelectByCard(string cardid, string password)
        {
            try
            {
                string sql = string.Format("select * from users where cardID='{0}' and password='{1}' ", cardid,password);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.users> SelectByName(string Name)
        {
            try
            {
                string sql = string.Format("select * from users where username='{0}'", Name);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.users> SelectByContion(Dictionary<string, string> contion)
        {
            try
            {
                string str = "";
                foreach (var d in contion)
                {
                    str += string.Format("{0}='{1}'" + " and ", d.Key, d.Value);
                }
                string scontion = str.Count() > 4 ? str.Remove(str.Count() - 4) : "";
                string sql = "select * from users where " + scontion;
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch { return null; }
        }

        public List<Model.users> SelectNameOrCardid(string contion)
        {
            try
            {
                string sql = string.Format("select * from users where username='{0}' or cardID='{1}'", contion,contion);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<users>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool Add(Model.users user)
        {
            try
            {
                string sqldep = string.Format("select * from department where id='{0}'", user.department);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        user.department = dep.guid;
                }

                string sqlduty = string.Format("select * from duties where id='{0}'", user.duties);
                 dps = SqliteHelper.ExecuteDataSet(con, sqlduty, CommandType.Text);
                 if (dps != null && dps.Tables.Count > 0)
                {
                    duties dep = DataTableToList.GetList<duties>(dps.Tables[0]).FirstOrDefault();
                    if(dep != null)
                        user.duties = dep.guid;
                }


                string sql =
                string.Format
                (
                "INSERT INTO users(username,is_superuser,password,is_staff,joinDate,department_id,duties_id,empID,cardID,phoneNumber,IDNum,mail,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ;select last_insert_rowid()",
                user.username,user .is_superuser==true?1:0,  user.password,user.is_staff==true?1:0,user.joinDate,user.department,user.duties,user.empID ,user.cardID ,user.phoneNumber ,user.IDNum,user.mail ,user.comment,user.guid             
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                  return true ;
                return false ;
            }
            catch
            {
                return false;
                
            }
        }

        public string addreturnid(Model.users user)
        {
            try
            {
                string sqldep = string.Format("select * from department where id='{0}'", user.department);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        user.department = dep.guid;
                }

                string sqlduty = string.Format("select * from duties where id='{0}'", user.duties);
                dps = SqliteHelper.ExecuteDataSet(con, sqlduty, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    duties dep = DataTableToList.GetList<duties>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        user.duties = dep.guid;
                }

                string sql =
                string.Format
                (
                "INSERT INTO users(username,is_superuser,password,is_staff,joinDate,department_id,duties_id,empID,cardID,phoneNumber,IDNum,mail,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ;select last_insert_rowid()",
                user.username, user.is_superuser==true?1:0, user.password, user.is_staff==true?1:0, user.joinDate, user.department, user.duties, user.empID, user.cardID, user.phoneNumber, user.IDNum, user.mail, user.comment, user.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                return ds.ToString ();
            }
            catch
            {
                return "-1";

            }
        }

        public bool Add(List<Model.users> userlist)
        {
            try
            {
                string sql = "";
                foreach (users user in userlist)
                {
                    string sqldep = string.Format("select * from department where id='{0}'", user.department);
                    DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            user.department = dep.guid;
                    }

                    string sqlduty = string.Format("select * from duties where id='{0}'", user.duties);
                    dps = SqliteHelper.ExecuteDataSet(con, sqlduty, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        duties dep = DataTableToList.GetList<duties>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            user.duties = dep.guid;
                    }

                    sql +=
                    string.Format
                    (
                    "INSERT INTO users(username,is_superuser,password,is_staff,joinDate,department_id,duties_id,empID,cardID,phoneNumber,IDNum,mail,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}');",
                    user.username, user.is_superuser==true?1:0, user.password, user.is_staff==true?1:0, user.joinDate, user.department, user.duties, user.empID, user.cardID, user.phoneNumber, user.IDNum, user.mail, user.comment, user.guid
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

        public bool Update(Model.users user)
        {
            try
            {
                string strsql = string.Format("username='{0}',password='{1}',is_staff='{2}',empID='{3}',cardID='{4}',phoneNumber='{5}',IDNum='{6}',mail='{7}',department_id='{8}',comment='{9}',is_superuser='{10}',duties_id='{11}'", user.username, user.password, user.is_staff==true?1:0, user.empID, user.cardID, user.phoneNumber, user.IDNum, user.mail, user.department, user.comment, user.is_superuser==true?1:0, user.duties);
                string contation = string.Format("where guid='{0}'", user.guid);
                string sql = "update users set " + strsql + contation;
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

        public bool Delete(Model.users user)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", user.guid);
                string sql = "delete from users " + contation;
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
