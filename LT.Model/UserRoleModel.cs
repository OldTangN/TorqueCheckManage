using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class role {
        public int id { get; set; }
        public string roleName { get; set; }
        public string system{ get; set; }
        public string   roleDM { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }
    }
   public  class UserRoleModel:role 
    {
    }
}
