using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class CheckDatashow
    {
        public int id { get; set; }
        public double targetvalue { get; set; }
        public double checkvalue { get; set; }
        public double rate { get; set; }
        public double resultrate { get; set; }
        public string isgood { get; set; }
        public int iseffect { get; set; }//0无效1有效
        public string guid { get; set; }
    }
}
