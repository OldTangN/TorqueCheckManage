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
   public  class SqliteCheckTarget:ICheckTarget
    {
        string con = OperationConfig.GetNlbsString();
        public string AddReturnGuid(Model.torquechecktarget torquechecktarget)
        {
            try
            {
                string sqldep = string.Format("select * from wrench where id='{0}'", torquechecktarget.wrenchID);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    wrench dep = DataTableToList.GetList<wrench>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                    torquechecktarget.wrenchID = dep.guid;
                }
                string sql =
                string.Format
                (
                "INSERT INTO torquechecktarget(wrenchID_id,checkDate,qaID,operatorID,torqueTargetValue,errorRangeMax,errorRangeMin,count,arry,is_good,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;select last_insert_rowid()",
                torquechecktarget.wrenchID, torquechecktarget.checkDate, torquechecktarget.qaID, torquechecktarget.operatorID, torquechecktarget.torqueTargetValue, torquechecktarget.errorRangeMax, torquechecktarget.errorRangeMin, torquechecktarget.count, torquechecktarget.arry, torquechecktarget.is_good==true?1:0, torquechecktarget.comment, torquechecktarget.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                return ds.ToString();
            }
            catch
            {
                return "-1";

            }
        }

        public bool Add(List<Model.torquechecktarget> listtorquechecktarget)
        {
            try
            {
                string sql = "";
                foreach (torquechecktarget torquechecktarget in listtorquechecktarget)
                {
                    string sqldep = string.Format("select * from wrench where id='{0}'", torquechecktarget.wrenchID);
                    DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        wrench dep = DataTableToList.GetList<wrench>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            torquechecktarget.wrenchID = dep.guid;
                    }
                    sql +=
                   string.Format
                   (
                   "INSERT INTO torquechecktarget(wrenchID_id,checkDate,qaID,operatorID,torqueTargetValue,errorRangeMax,errorRangeMin,count,arry,is_good,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;",
                   torquechecktarget.wrenchID, torquechecktarget.checkDate, torquechecktarget.qaID, torquechecktarget.operatorID, torquechecktarget.torqueTargetValue, torquechecktarget.errorRangeMax, torquechecktarget.errorRangeMin, torquechecktarget.count, torquechecktarget.arry, torquechecktarget.is_good==true?1:0, torquechecktarget.comment, torquechecktarget.guid
                   ) + "\r";
                }
                sql += " select last_insert_rowid()";
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
        }

        public bool Update(Model.torquechecktarget torquechecktarget)
        {
            try
            {
                string contion = string.Format("where guid='{0}'", torquechecktarget.guid);
                string strsql = string.Format("wrenchID_id='{0}',checkDate='{1}',qaID='{2}',operatorID='{3}',torqueTargetValue='{4}',errorRangeMax='{5}',is_good='{6}',comment='{7}',errorRangeMin='{8}'", torquechecktarget.wrenchID, Convert.ToDateTime(torquechecktarget.checkDate), torquechecktarget.qaID, torquechecktarget.operatorID, torquechecktarget.torqueTargetValue, torquechecktarget.errorRangeMax, torquechecktarget.is_good ? 1 : 0, torquechecktarget.comment, torquechecktarget.errorRangeMin);
                string sql = "update torquechecktarget set " + strsql + contion;
                var ds = SqliteHelper.ExecuteNonQuery(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch { return false; }
        }

        public bool Del(Model.torquechecktarget torquechecktarget)
        {
            throw new NotImplementedException();
        }

        public Model.torquechecktarget SelectByGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from torquechecktarget where guid='{0}' ",guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquechecktarget>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.torquechecktarget> SelectByWrench(string wrenchguid)
        {
            try
            {
                string sql = string.Format("select * from torquechecktarget where wrenchID_id='{0}' ", wrenchguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquechecktarget>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.torquechecktarget> SelectByDate(DateTime first, DateTime last, string wrenchID)
        {
            try
            {
                string sql = string.Format("select * from torquechecktarget where checkDate>='{0}' and checkDate<='{1}' and wrenchID_id='{2}' ",first,last,wrenchID);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquechecktarget>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public List<Model.torquechecktarget> SelectByContion(Dictionary<string, string> dict)
        {
            try
            {
                string temp = "";
                foreach (var d in dict)
                {
                    if (d.Key == "starttime")
                    {
                        temp += string.Format("checkDate>='{0}' and ", d.Value);
                        continue;
                    }
                    if (d.Key == "endtime")
                    {
                        temp += string.Format("checkDate<'{0}' and ", d.Value);
                        continue;
                    }
                    temp += string.Format("{0}='{1}' and ", d.Key, d.Value);
                }
                string contion = temp.Count() > 4 ? temp.Remove(temp.Count() - 4) : "";
                string sql = "select * from torquechecktarget where " + contion;
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquechecktarget>(ds.Tables[0]);
                }
                return null;
            }
            catch { return null; }
        }
        public List<torquechecktarget> SelectByContion(Dictionary<string, string> dict, int pagesize, int pageno)
        {
            try
            {
                string temp = "";
                foreach (var d in dict)
                {
                    if (d.Key == "starttime")
                    {
                        temp += string.Format("checkDate>='{0}' and ", d.Value);
                        continue;
                    }
                    if (d.Key == "endtime")
                    {
                        temp += string.Format("checkDate<'{0}' and ", d.Value);
                        continue;
                    }
                    temp += string.Format("{0}='{1}' and ", d.Key, d.Value);
                }
                string contion = temp.Count() > 4 ? temp.Remove(temp.Count() - 4) : "";
                contion += string.Format("LIMIT {0},{1}", (pageno * pagesize), pagesize);
                string sql = "select * from torquechecktarget where " + contion;
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquechecktarget>(ds.Tables[0]);
                }
                return null;
            }
            catch { return null; }
        }

        public int SelectCount(Dictionary<string, string> dict)
        {
            string temp = "";
            try
            {
            foreach (var d in dict)
            {
                if (d.Key == "starttime")
                {
                    temp += string.Format("checkDate>='{0}' and ", d.Value);
                    continue;
                }
                if (d.Key == "endtime")
                {
                    temp += string.Format("checkDate<'{0}' and ", d.Value);
                    continue;
                }
                temp += string.Format("{0}='{1}' and ", d.Key, d.Value);
            }
            string contion = temp.Count() > 4 ? temp.Remove(temp.Count() - 4) : "";
            string sql = "select COUNT(*) AS count  from torquechecktarget  where " + contion;
         
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);                  
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
