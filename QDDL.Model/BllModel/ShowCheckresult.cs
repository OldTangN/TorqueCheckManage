using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Model.BllModel
{
    /// <summary>
    /// 显示校验值
    /// </summary>
   public  class ShowCheckresult
    {
       public int id { get; set; }
       public decimal checkdata { get; set; }//读取实际值
       public decimal setdata { get; set; }///校验设定值
       public string normalrang { get; set; }//正常
       public decimal errorrang { get; set; }//实际
       public string error { get; set; }//百分比比
       public string  normalmin { get; set; }
       public string normalmax { get; set; }
       public string  result { get; set; }///单次结果
       public bool iseffect { get; set; } ///是否是有效结果
       public bool isturn { get; set; }
       public string testerid { get; set; }
                                  
    }
}
