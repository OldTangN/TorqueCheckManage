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
  public   class SqliteWrenchSpecies:IWrenchSpecies
    {
      string con = OperationConfig.GetNlbsString();
        public bool add(Model.wrenchspecies species)
        {
            try
            {
                string sql =
                string.Format
                (
                "INSERT INTO wrenchspecies(speciesName,speciesCode,parentSpecies,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}') ;select last_insert_rowid()",
                species.speciesName,species.speciesCode,species.parentSpecies,species.comment,species.guid
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

        public string addreturnid(Model.wrenchspecies species)
        {
            try{
                string sql =
               string.Format
               (
               "INSERT INTO wrenchspecies(speciesName,speciesCode,parentSpecies,comment,guid) VALUES('{0}','{1}','{2}','{3}','{4}') ;select last_insert_rowid()",
               species.speciesName, species.speciesCode, species.parentSpecies, species.comment, species.guid
               );
                var ds = SqliteHelper.ExecuteScalar(con, sql, CommandType.Text);
                return ds.ToString ();
            }
            catch
            {
                return "-1";

            }
        }

        public bool addmany(List<Model.wrenchspecies> listwc)
        {
            try
            {
                string sql = "";
                foreach (wrenchspecies species in listwc)
                {
                    sql +=
                    string.Format
               (
               "INSERT INTO wrenchspecies(speciesName,speciesCode,parentSpecies,comment,guid) VALUES('{0}',{1},'{2}',{3},'{4}');",
               species.speciesName, species.speciesCode, species.parentSpecies, species.comment, species.guid
               ) + "\r";
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

        public bool updata(Model.wrenchspecies species)
        {
            try
            {
                string strsql = string.Format("speciesName='{0}',speciesCode='{1}',parentSpecies='{3}',comment='{2}'", species.speciesName, species.speciesCode, species.comment, species.parentSpecies);
                string contation = string.Format("where guid='{0}'", species.guid);
                string sql = "update wrenchspecies set " + strsql + contation;
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

        public List<Model.wrenchspecies> select()
        {
            try
            {
                string sql = string.Format("select * from wrenchspecies");
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchspecies>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<Model.wrenchspecies> selectbyname(string name)
        {
            try
            {
                string sql = string.Format("select * from wrenchspecies where speciesName='{0}'",name);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchspecies>(ds.Tables[0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Model.wrenchspecies selectByGuid(string guid)
        {
            try
            {
                string sql = string.Format("select * from wrenchspecies where guid='{0}'", guid);
                DataSet ds = SqliteHelper.ExecuteDataSet(con, sql, CommandType.Text);
                if (ds != null && ds.Tables.Count > 0)
                {
                    return DataTableToList.GetList<wrenchspecies>(ds.Tables[0]).FirstOrDefault();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool Del(Model.wrenchspecies wrenchspecies)
        {
            try
            {
                string contation = string.Format("where guid='{0}'", wrenchspecies.guid);
                string sql = "delete from wrenchspecies " + contation;
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
