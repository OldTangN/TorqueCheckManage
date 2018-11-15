using LT.Comm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public  App()
        {


            try
            {
                //if (!KeyCheck.CheckKeyFile())
                //{
                //    MessageAlert.Alert("没有注册信息");
                //    Environment.Exit(0);
                //}
                //if (!KeyCheck.CheckWithFileKey())
                //{
                //    MessageAlert.Alert("注册信息不匹配！");
                //    Environment.Exit(0);
                //}
            }
            catch
            {
                LogUtil.WriteLog(null, "出错");
            }
        }
  
    }
}
