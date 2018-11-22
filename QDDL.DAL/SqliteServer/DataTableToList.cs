using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QDDL.DAL.SqliteServer
{
  public static  class DataTableToList
    {
      public static List<T> GetList<T>(DataTable table)
      {
          
          List<T> list = new List<T>();
          T t = default(T);
          PropertyInfo[] propertypes = null;
          string tempName = string.Empty;
          foreach (DataRow row in table.Rows)
          {
              t = Activator.CreateInstance<T>();
              propertypes = t.GetType().GetProperties();
              foreach (PropertyInfo pro in propertypes)
              {
                  tempName = pro.Name;
                  if (table.Columns.Contains(tempName))
                  {
                      object value = row[tempName];
                      if (!value.ToString().Equals(""))
                      {
                          
                              if (value.GetType() == typeof(Int64))
                              {                                                                 
                                  pro.SetValue(t, Convert.ToInt32(value), null);
                              }
                              else
                              {
                                  pro.SetValue(t, value, null);
                              }
                      }
                  }
                  else if (table.Columns.Contains(tempName + "_id"))
                  {

                      object value = row[tempName+"_id"];
                      if (!value.ToString().Equals(""))
                      {
                          pro.SetValue(t, value, null);
                      }
                  }
              }
              list.Add(t);
          }
          return list.Count == 0 ? null : list;
      }



      public static  DataSet ConvertToDataSet<T>(IList<T> list)
      {
          if (list == null || list.Count <= 0)
          {
              return null;
          }

          DataSet ds = new DataSet();
          DataTable dt = new DataTable(typeof(T).Name);
          DataColumn column;
          DataRow row;

          System.Reflection.PropertyInfo[] myPropertyInfo = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

          foreach (T t in list)
          {
              if (t == null)
              {
                  continue;
              }

              row = dt.NewRow();

              for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
              {
                  System.Reflection.PropertyInfo pi = myPropertyInfo[i];

                  string name = pi.Name;

                  if (dt.Columns[name] == null)
                  {
                      column = new DataColumn(name, pi.PropertyType);
                      dt.Columns.Add(column);
                  }

                  row[name] = pi.GetValue(t, null);
              }

              dt.Rows.Add(row);
          }

          ds.Tables.Add(dt);

          return ds;
      }

				
    }
 
					       
				
}
