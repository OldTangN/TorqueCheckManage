using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DAL.Mysql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace LT.DAL.Mysql.Tests
{
    [TestClass()]
    public class MySqlUserTests
    {
        [TestMethod()]
        public void AddTest()
        {
            //List<Model.UserModel> userlist = new List<Model.UserModel>();
           //Model .UserModel  users= new Model.UserModel() { username ="张三",password ="123",is_staff =false ,is_superuser =false ,empID ="1",cardID ="1234567",departmentID ="213123",roleID ="撒旦撒旦",phoneNumber ="123",comment =""};
           // MySqlUser user = new 
            Model.UserRoleModel usermodel = new Model.UserRoleModel() { };
            MySqlUserRole my = new MySqlUserRole();
            my.Add(usermodel);
        }
    }
}
