using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    class SystemChecksetModel
    {
    }
    public class systemcheckset {
        public int id { get; set; }
        public int? count { get; set; }//次数
        public int? arry { get; set; }//组数
        public decimal boundaryvalue { get; set; }
        public decimal? throwvalue { get; set; }//抛弃值
        public string comment { get; set; }//备注
        public string systemname { get; set; }
        public bool? ishavejuser { get; set; }//是否需要校验员
        public string noticetime { get; set; }//提示时间
        public string noticedays { get; set; }//提前提示天数
        public bool noticeshow { get; set; }//显示提示

    }
}
