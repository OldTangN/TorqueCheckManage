using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Comm
{
    public static class OperationConfig
    {

        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch 
            { return ""; }
        }

        public static string GetString()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["sqlite"].ToString();
            }
            catch
            {
                return "";
            }
        
        }
        public static string GetNlbsString()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["sqlitenlbs"].ToString();
            }
            catch
            {
                return "";
            }
        }

    }
}

