using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.Comm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace LT.Comm.Tests
{
    [TestClass()]
    public class OperationConfigTests
    {
        [TestMethod()]
        public void GetValueTest()
        {
            OperationConfig oc = new OperationConfig();
            string key = "dbtype";
            string value = oc.GetValue(key);
            string target = "MySql";
            Assert.AreEqual(target,value );
         
        }
    }
}
