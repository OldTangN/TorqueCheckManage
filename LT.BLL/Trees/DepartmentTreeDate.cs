using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace LT.BLL.Trees
{
   public  class DepartmentTreeDate
    {
       IDepartment Department = DataAccess .CreateDepartment ();

       private List <department> GetParent(string parentguid="")
       {
           List<department> ds = new List<department>();
           ds = Department.SelectByDelFlag(parentguid, false);
           return ds;
       }
       private List<TreeData> GetDepartmentList()
       {
           List<department> departmentlist = GetParent();
           List<TreeData> td = new List<TreeData>();
           if (departmentlist == null || departmentlist.Count <= 0)
               return null;          
           foreach (department d in departmentlist)
           {
               td.Add(new TreeData() { id = d.id, ParentId = 0, title = d.departmentName });
               List<TreeData> t= GetChildren(Department.SelectByDelFlag(d.guid, false));
               foreach (TreeData tt in t)
               {
                   td.Add(tt);
               }
           }
           return td;       
       }

       private List<TreeData> GetDepartment()
       {
           try
           {
               List<department> dpl = Department.SelectByFlag(false);
               List<TreeData> td = new List<TreeData>();
               foreach (department d in dpl)
               {
                   if (d != null)
                   {
                       if (string .IsNullOrEmpty(d.parentDepartment))
                       {
                           td.Add(new TreeData() { id = d.id, ParentId = 0, title = d.departmentName });
                       }
                       else
                       {
                           department pd = Department.SelectByGuid(d.parentDepartment);
                           td.Add(new TreeData() { id = d.id, ParentId = pd.id, title = d.departmentName });

                       }
                   }
               }
               return td;
           }
           catch
           {
               return new List<TreeData>();
           }
       }

       private List<TreeData> GetChildren(List<department> dp)
       {
           List<TreeData> td =new List<TreeData> ();
           if (dp == null || dp.Count <= 0)
               return td;
           foreach (department d in dp)
           {
               department pd = Department.SelectByGuid(d.parentDepartment);
               td.Add(new TreeData() { id = d.id, title = d.departmentName, ParentId = pd.id });
               List<TreeData> temptd =new List<TreeData> ();
               temptd = GetChildren(Department.SelectByDelFlag(d.guid, false));
               if (temptd == null || temptd.Count <=0)
               {
                   continue;
               }
               foreach (TreeData rt in temptd)
               {
                   td.Add(rt);
               }             
           }
           return td;
       }

       public  DataTable ToDatatable()
       {
           List<TreeData> lt = GetDepartment();
           DataTable dt = new DataTable();
           return ListToDataTable.ToDataTable<TreeData>(lt, null);               
       }    
    }
   public class TreeData
   {
       public Int64 id { get; set; }
       public string title { get; set; }
       public Int64 ParentId { get; set; }
   }
}
