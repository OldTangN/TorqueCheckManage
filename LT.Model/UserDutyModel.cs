using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class duties 
    {
        public int id { get; set; }
        public string department { get; set; }
        public string dutiesName { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }

    }
    public class ShowDutyModel
    {
        public int id { get; set; }
        public string department { get; set; }
        public string dutiesName { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }
        public string departmentName { get; set; }
    }
   
}
