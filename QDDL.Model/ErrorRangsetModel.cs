using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Model
{

    public class errorrangset
    {
        public int id { get; set; }
        public string speciesID { get; set; }
        public string speciesname { get; set; }
        public decimal rangmax { get; set; }
        public decimal rangmin { get; set; }
        public string rangvalue { get; set; }
        public string unit { get; set; }
        public decimal errorrangMax { get; set; }
        public decimal errorrangMin { get; set; }
        public string errorrang { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }

    }
}
