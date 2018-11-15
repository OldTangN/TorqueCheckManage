using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class department {
        public int id { get; set; }
        public string departmentName { get; set; }
        public string parentDepartment{ get; set; }
        public string comment { get; set; }
        public bool delDepartment { get; set; }
        public string guid { get; set; }
    }


   public  class DepartmentModel:department 
    {
       public string parentname { get; set; }
    }
}
