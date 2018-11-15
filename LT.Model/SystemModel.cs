using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class system
    {
        public int id { get; set; }
        public string systemName { get; set; }
        public string systemDM { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }

    }
   public  class SystemModel:system 
    {
    }
}
