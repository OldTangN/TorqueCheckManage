using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{

    public class borrow
    {
        public int id { get; set; }
        public string WrenchID { get; set; }
        public string borrowUser { get; set; }
        public string borrowOperator { get; set; }
        public string borrowDate { get; set; }
        public bool is_return { get; set; }
        public string comment { get; set; }
        public string returnUser { get; set; }
        public string returnOperator { get; set; }
        public string returnDate { get; set; }
        public string guid { get; set; }
    }
}
