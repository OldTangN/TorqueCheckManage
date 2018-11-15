using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongTie.Domain.Model
{
    public class system
    {
        public int id { get; set; }
        public string systemName { get; set; }
        public string systemDM { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }
        public virtual List<role> roles { get; set;}
    }
}
