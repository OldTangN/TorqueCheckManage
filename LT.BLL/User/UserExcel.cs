using LT.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.BLL.User
{

    public class UserOutExcel
    {
        DataTable Header(DataTable dt1)
        {
            DataTable dt = new DataTable();
            dt = dt1;
            List<string> headlist = new List<string>() { "序 号", "员工姓名", "员工编号", "员工卡号", "部 门", "角 色", "联系方式", "入职时间" };

            foreach (string s in headlist)
            {
                dt.Columns.Add(s);
            }
            return dt;
        }
        public DataTable ToTable(List<UserModel> tm)
        {
            DataTable dt = new DataTable();
            dt = Header(dt);
            int count = 0;
            object[] values = new object[8];
            foreach (UserModel t in tm)
            {
                count++;
                values[0] = count.ToString();
                values[1] = t.username;
                values[2] = t.empID + "\t";
                values[3] = t.cardID;
                values[4] = t.departName;
                values[5] = t.roleName;
                values[6] = t.phoneNumber;
                values[7] = t.joinDate + "\t";
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
