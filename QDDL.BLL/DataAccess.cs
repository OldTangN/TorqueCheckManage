using LT.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LT.BLL
{
 public class DataAccess
    {
        private static readonly string AssemblyName = "LT.DAL";
        private static readonly string db = "MySql";

        public static IUser CreateUser() {
            string classname = AssemblyName + "." + db + "User";
            return (IUser)Assembly.Load(AssemblyName).CreateInstance(classname);
        }
        public static IDepartment CreateDepartment() {
            string classname = AssemblyName + "." + db + "Department";
            return (IDepartment)Assembly.Load(AssemblyName).CreateInstance(classname);
        }
        public static IUserRole CreateUserRole() {
            string classname = AssemblyName + "." + "UserRole";
            return (IUserRole)Assembly.Load(AssemblyName).CreateInstance(classname);
        }

    }
}
