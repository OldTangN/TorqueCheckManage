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
  public  class SqliteCheckTargetRecord:ICheckTargetRecord
    {
        string con = OperationConfig.GetNlbsString();
        public string AddReturnGuid(Model.torquecheckrecord torquecheckrecord)
        {
            try
            {
                string sqldep = string.Format("select * from torquechecktarget where id='{0}'", torquecheckrecord.TorqueCheckTargetID);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    torquechecktarget dep = DataTableToList.GetList<torquechecktarget>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        torquecheckrecord.TorqueCheckTargetID = dep.guid;
                }
                string sql =
                string.Format
                (
                "INSERT INTO torquecheckrecord(TorqueCheckTargetID_id,TesterID,errorRangeMax,errorRangeMin,analyserValue,torqueTargetValue,checkTime,passedFlag,isEffective,isTurn,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;select last_insert_rowid()",
                torquecheckrecord.TorqueCheckTargetID, torquecheckrecord.TesterID, torquecheckrecord.errorRangeMax, torquecheckrecord.errorRangeMin, torquecheckrecord.analyserValue, torquecheckrecord.torqueTargetValue, torquecheckrecord.checkTime, torquecheckrecord.passedFlag==true?1:0, torquecheckrecord.isEffective==true?1:0, torquecheckrecord.isTurn==true?1:0, torquecheckrecord.comment, torquecheckrecord.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                return ds.ToString();
            }
            catch
            {
                return "-1";

            }
        }

        public bool AddNotReturn(Model.torquecheckrecord checkrecord)
        {
            try
            {
                string sqldep = string.Format("select * from torquechecktarget where id='{0}'", checkrecord.TorqueCheckTargetID);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    torquechecktarget dep = DataTableToList.GetList<torquechecktarget>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        checkrecord.TorqueCheckTargetID = dep.guid;
                }
                string sql =
                string.Format
                (
                "INSERT INTO torquecheckrecord(TorqueCheckTargetID_id,TesterID,errorRangeMax,errorRangeMin,analyserValue,torqueTargetValue,checkTime,passedFlag,isEffective,isTurn,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;select last_insert_rowid()",
                 checkrecord.TorqueCheckTargetID, checkrecord.TesterID, checkrecord.errorRangeMax, checkrecord.errorRangeMin, checkrecord.analyserValue, checkrecord.torqueTargetValue, checkrecord.checkTime, checkrecord.passedFlag == true ? 1 : 0, checkrecord.isEffective == true ? 1 : 0, checkrecord.isTurn == true ? 1 : 0, checkrecord.comment, checkrecord.guid
                );
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

        public bool AddMany(List<Model.torquecheckrecord> listtorquecheckrecord)
        {
            try
            {
                string sql = "";
                foreach (torquecheckrecord checkrecord in listtorquecheckrecord)
                {
                    string sqldep = string.Format("select * from torquechecktarget where id='{0}'", checkrecord.TorqueCheckTargetID);
                    DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        torquechecktarget dep = DataTableToList.GetList<torquechecktarget>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            checkrecord.TorqueCheckTargetID = dep.guid;
                    }
                     sql +=
                    string.Format
                    (
                    "INSERT INTO torquecheckrecord(TorqueCheckTargetID_id,TesterID,errorRangeMax,errorRangeMin,analyserValue,torqueTargetValue,checkTime,passedFlag,isEffective,isTurn,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}') ;",
                     checkrecord.TorqueCheckTargetID, checkrecord.TesterID, checkrecord.errorRangeMax, checkrecord.errorRangeMin, checkrecord.analyserValue, checkrecord.torqueTargetValue, checkrecord.checkTime, checkrecord.passedFlag == true ? 1 : 0, checkrecord.isEffective == true ? 1 : 0, checkrecord.isTurn == true ? 1 : 0, checkrecord.comment, checkrecord.guid
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

        public bool Del(Model.torquecheckrecord checkrecord)
        {
            throw new NotImplementedException();
        }

        public List<Model.torquecheckrecord> SelectByCheckTargetID(string checktargetguid)
        {
            try
            {
                string sql = string.Format("select * from torquecheckrecord where TorqueCheckTargetID_id='{0}' ", checktargetguid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<torquecheckrecord>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
