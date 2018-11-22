using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.BLL.Check
{
    public class CheckTools
    {
        //  CheckDatashow _checkdata;
        private int successno = 0;
        int count = 0;
        List<CheckDatashow> checkdatalist = new List<CheckDatashow>();
        //  string checkuserid;
        public CheckTools()
        {

        }
        public void funcheck(CheckDatashow checkdata)
        {

        }
        public void change(CheckDatashow checkdata)
        {

            checkdatalist.Add(checkdata);
            successno = 0;
            count++;

        }




    }
}
