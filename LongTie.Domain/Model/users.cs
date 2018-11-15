using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongTie.Domain.Model
{
    public class users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool is_superuser { get; set; }
        public bool is_staff { get; set; }
        public string joinDate { get; set; }
        public string duties { get; set; }
        public string empID { get; set; }
        public string cardID { get; set; }
        public string phoneNumber { get; set; }
        public string IDNum { get; set; }
        public string mail { get; set; }
        public string department { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }

    }
}
