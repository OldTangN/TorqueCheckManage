using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongTie.Domain.Model
{
    public class role
    {
        public int id { get; set; }
        public string roleName { get; set; }      
        public string roleDM { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }
        public string systemguid { get; set; }
        public system system { get; set; }
    }
}
