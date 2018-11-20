using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class torquechecktarget
    {
        public int id { get; set; }
        public string wrenchID { get; set; }
        public DateTime checkDate { get; set; }
        public string qaID { get; set; }
        public string operatorID { get; set; }
        public decimal torqueTargetValue { get; set; }
        public string errorRangeMax { get; set; }
        public string errorRangeMin { get; set; }
        public int count { get; set; }
        public int arry { get; set; }
        public bool is_good { get; set; }//1 工具正常0不正常
        public string comment { get; set; }
        public string guid { get; set; }
    }


    public class ToolCheckExcel
    {
        public string wrenchcode { get; set; }
        public string jusername { get; set; }
        public string zusername { get; set; }
        public string torquetargetvalue { get; set; }
        public string errorrange { get; set; }
        public bool is_good { get; set; }
        public string checkdate { get; set; }
        public string guid { get; set; } 
    }
}
