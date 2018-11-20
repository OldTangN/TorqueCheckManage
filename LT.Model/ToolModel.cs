using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class wrench
    {
        public int id { get; set; }
        public string wrenchCode { get; set; }
        public string wrenchBarCode { get; set; }
        public decimal rangeMin { get; set; }
        public decimal rangeMax { get; set; }
        public decimal targetvalue { get; set; }
        public decimal targetvalue1 { get; set; }
        public decimal targetvalue2 { get; set; }
        public string unit { get; set; }
        public string factory { get; set; }
        public DateTime createDate { get; set; }
        public string IP { get; set; }
        public string port { get; set; }
        public string species { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
        public DateTime lastrepair { get; set; }
        public decimal cycletime { get; set; } = 0;
        public bool isallowcheck { get; set; }//1允许2不允许
        public string guid { get; set; }
    }
    public class ToolModel : wrench
    {

        // public wrench wrenchdata { get; set; }
        public string speciesName { get; set; }
        public string statusName { get; set; }
        public string statusDM { get; set; }
    }
    public class Toolinfo
    {
        public wrench wrench { get; set; }
        public string speciesName { get; set; }
        public string statusName { get; set; }

    }
    public class wrenchinfo
    {
        public wrench wrench { get; set; }
        public wrenchspecies species { get; set; }
        public wrenchstatus status { get; set; }

    }
}
