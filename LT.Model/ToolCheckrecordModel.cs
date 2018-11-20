using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class torquecheckrecord
    {
        public int id { get; set; }
        public string TorqueCheckTargetID { get; set; }
        public string TesterID { get; set; }
        public decimal analyserValue { get; set; }
        public decimal torqueTargetValue { get; set; }
        public string errorRangeMax { get; set; }
        public string errorRangeMin { get; set; }
        public DateTime checkTime { get; set; }
        public bool passedFlag { get; set; }//0不通过1 通过
        public bool isEffective { get; set; }//0失败的数据1成功的数据
        public bool isTurn { get; set; }//扭矩方向
        public string comment { get; set; }
        public string guid { get; set; }
    }

}
