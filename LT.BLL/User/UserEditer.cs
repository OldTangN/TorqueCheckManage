using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LT.BLL.User
{
 public   class UserEditer
    {
     IDepartment Department = DataAccess.CreateDepartment();
     IUserDuty UserDuty = DataAccess.CreateUserDuty();

     public void BindDepartment(ComboBox cb) {
         List<department> departmentlist = new List<department>();
         departmentlist = Department.SelectByFlag ();
         cb.ItemsSource = departmentlist;
         cb.DisplayMemberPath = "departmentName";
         cb.SelectedValuePath = "id";
     }
     public void BindRole(ComboBox cb)
     {
         GetRole gr = new GetRole();
         List<role> rl = new List<role>();
         rl = gr.getrole();
         cb.ItemsSource = rl;       
         cb.DisplayMemberPath = "roleName";
         cb.SelectedValuePath = "id";
     }
     public void BindDuty(ComboBox cb) 
     {
         List<duties> dl = new List<duties>();
         dl = UserDuty.Select();
         cb.ItemsSource = dl;
         cb.DisplayMemberPath = "dutiesName";
         cb.SelectedValuePath = "id";
     }

     public void BindDuty(ComboBox cb,string departmentguid)
     {
         List<duties> dl = new List<duties>();
         dl = UserDuty.SelectByDepartment(departmentguid);
         cb.ItemsSource = dl;
         cb.DisplayMemberPath = "dutiesName";
         cb.SelectedValuePath = "id";
     }
     public void BindUserModel(DataGrid dg) {
         dg.ItemsSource = null;
         GetUser gu = new GetUser();
         List<UserModel> um = new List<UserModel>();
         um = gu.GetUserModel();
         dg.ItemsSource = um;
        // dg.DataContext = um;
     }

    }
    public class UserAdd
    {
    
    }
}
