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
   public  class SqliteWrench:IWrench
    {
       string con = OperationConfig.GetNlbsString();
        public bool add(Model.wrench wrenchtool)
        {
            try
            {

                string sqldep = string.Format("select * from wrenchspecies where id='{0}'", wrenchtool.species);
                DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        wrenchtool.species = dep.guid;
                }


                 sqldep = string.Format("select * from wrenchstatus where id='{0}'", wrenchtool.status);
                 dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                if (dps != null && dps.Tables.Count > 0)
                {
                    wrenchstatus dep = DataTableToList.GetList<wrenchstatus>(dps.Tables[0]).FirstOrDefault();
                    if (dep != null)
                        wrenchtool.status = dep.guid;
                }

                string sql =
                string.Format
                (
                "INSERT INTO wrench(wrenchCode,wrenchBarCode,rangeMin,rangeMax,factory,createDate,IP,port,species_id,status_id,lastrepair,cycletime,isallowcheck,targetvalue,targetvalue1,targetvalue2,unit,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}') ;select last_insert_rowid()",
                 wrenchtool.wrenchCode, wrenchtool.wrenchBarCode, wrenchtool.rangeMin, wrenchtool.rangeMax, wrenchtool.factory, wrenchtool.createDate, wrenchtool.IP, wrenchtool.port, wrenchtool.species, wrenchtool.status, wrenchtool.lastrepair, wrenchtool.cycletime, wrenchtool.isallowcheck == true ? 1 : 0, wrenchtool.targetvalue, wrenchtool.targetvalue1, wrenchtool.targetvalue2, wrenchtool.unit, wrenchtool.comment, wrenchtool.guid
                );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                if (Convert.ToInt32(ds) > 0)
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                return false;

            }
        }

        public bool addlist(List<Model.wrench> wrenchlist)
        {
            try
            {
                string sql = "";
                foreach (wrench wrenchtool in wrenchlist)
                {
                    string sqldep = string.Format("select * from wrenchspecies where id='{0}'", wrenchtool.species);
                    DataSet dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        department dep = DataTableToList.GetList<department>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            wrenchtool.species = dep.guid;
                    }


                    sqldep = string.Format("select * from wrenchstatus where id='{0}'", wrenchtool.status);
                    dps = SqliteHelper.ExecuteDataSet(con, sqldep, CommandType.Text);
                    if (dps != null && dps.Tables.Count > 0)
                    {
                        wrenchstatus dep = DataTableToList.GetList<wrenchstatus>(dps.Tables[0]).FirstOrDefault();
                        if (dep != null)
                            wrenchtool.status = dep.guid;
                    }

                    sql +=
                    string.Format
                (
                "INSERT INTO wrench(wrenchCode,wrenchBarCode,rangeMin,rangeMax,factory,createDate,IP,port,species_id,status_id,lastrepair,cycletime,isallowcheck,targetvalue,targetvalue1,targetvalue2,unit,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}');",
                wrenchtool.wrenchCode, wrenchtool.wrenchBarCode, wrenchtool.rangeMin, wrenchtool.rangeMax, wrenchtool.factory, wrenchtool.createDate, wrenchtool.IP, wrenchtool.port, wrenchtool.species, wrenchtool.status, wrenchtool.lastrepair, wrenchtool.cycletime, wrenchtool.isallowcheck==true?1:0, wrenchtool.targetvalue, wrenchtool.targetvalue1, wrenchtool.targetvalue2, wrenchtool.unit, wrenchtool.comment, wrenchtool.guid
                ) +"\r";
                }
                sql += "select last_insert_rowid();";
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

        public bool updata(Model.wrench wrenchtool)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", wrenchtool.guid);
                string strsql = string.Format(
                "wrenchCode='{0}',wrenchBarCode='{1}',rangeMin='{2}',rangeMax='{3}',factory='{4}',species_id='{5}',status_id='{6}',comment='{7}',targetvalue='{8}',unit='{9}',isallowcheck='{10}',cycletime={11},createDate='{12}' , targetvalue1='{13}',targetvalue2='{14}' ,lastrepair='{15}' ", wrenchtool.wrenchCode
                , wrenchtool.wrenchBarCode, wrenchtool.rangeMin, wrenchtool.rangeMax, wrenchtool.factory, wrenchtool.species, wrenchtool.status, wrenchtool.comment, wrenchtool.targetvalue, wrenchtool.unit, wrenchtool.isallowcheck==true?1:0, wrenchtool.cycletime, wrenchtool.createDate, wrenchtool.targetvalue1, wrenchtool.targetvalue2, wrenchtool.lastrepair);
               string sql = "update wrench set " + strsql + contation;
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

        public bool Del(Model.wrench wrenchmodel)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", wrenchmodel.guid);
                string sql = "delete from wrench " + contation;
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

        public List<Model.wrench> select()
        {
            try
            {
                string sql = string.Format("select * from wrench");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrench> SelectBarorcode(string contion)
        {
            try
            {
                string sql = string.Format("select * from wrench where wrenchBarCode='{0}' or wrenchCode='{1}'",contion,contion);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.wrench selectByguid(string guid)
        {
            try
            {
                string sql = string.Format("select * from wrench where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrench> selectByContion(Dictionary<string, string> dict)
        {
            try
            {
            string str = "";
            foreach (var d in dict)
            {
                str += (string.Format("{0}='{1}'" + " and ", d.Key, d.Value));
            }
            string contion = str.Count() > 4 ? str.Remove(str.Count() - 4) : "";
            string sql = "select * from wrench where "+ contion;
            DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
            if (ds != null && ds.Tables.Count > 0)
           {
              return DataTableToList.GetList<wrench>(ds.Tables[0]);
           }
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Model.wrench selectByBarcode(string barcode)
        {
            try
            {
                string sql = string.Format("select * from wrench where wrenchBarCode='{0}'", barcode);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrench> selectBycode(string code)
        {
            try
            {
                string sql = string.Format("select * from wrench where  wrenchCode='{0}'", code);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrench> selectByid(int id)
        {
            try
            {
                string sql = string.Format("select * from wrench where  id='{0}'", id);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrench> selectBystatus(string status)
        {
            try
            {
                string sql = string.Format("select * from wrench where  status_id='{0}'", status);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public List<Model.wrench> selectPage(int page, int pageNo)
        {
            string condition = string.Format(" id <=(SELECT id FROM wrench ORDER BY id desc LIMIT {0} OFFSET {1}) ORDER BY id desc LIMIT {2}", (pageNo - 1) * page, 1, page);
            string sql = "select * from  wrench where " + condition;
            try
            {              
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrench>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }

        }

        public int SelectCount()
        {
            string sql = "select COUNT(*)  from wrench";
            try
            {
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
