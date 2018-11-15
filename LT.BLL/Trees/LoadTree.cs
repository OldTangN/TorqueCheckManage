using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LT.BLL.Trees
{
  public  class LoadTree
    {
      TreeView _treeview = null;
      DepartmentTreeDate dtd = new DepartmentTreeDate();
      public LoadTree(TreeView treeview)
      {
          this._treeview = treeview;
      }
      public  void TreeLoad()
      {
        
          //_treeview.Items.Clear();//加载根节点前先清除Treeview控件项
          List<PropertyNodeItem> itemList = new List<PropertyNodeItem>();
          DataTable   dt = dtd.ToDatatable ();// webDict.GetArticles();
          if (dt == null || dt.Rows.Count <= 0)
              return;
          DataView dv = dt.DefaultView;
          dv.RowFilter = "ParentId=0";
          for (int i = 0; i < dv.Count; i++)
          {
              PropertyNodeItem node = new PropertyNodeItem()
              {
                  DisplayName = dv[i].Row["title"].ToString(),
                  Name = dv[i].Row["title"].ToString(),
                  id = Convert.ToInt32(dv[i].Row["id"].ToString()),
                  parentId = Convert.ToInt32(dv[i].Row["ParentId"].ToString()),
                  IsExpanded = true,
                  Icon = "/LongTie.Nlbs;component/Images/department.png"

              };
              int id = Convert.ToInt32(dv[i]["id"].ToString());
              int pid = Convert.ToInt32(dv[i]["ParentId"].ToString());
              ForeachPropertyNode(node, id);
              itemList.Add(node);
          }
     
          this._treeview.ItemsSource = itemList;
      }
      //无限接循环子节点添加到根节点下面
      private void ForeachPropertyNode(PropertyNodeItem node, int pid)
      {
          DataTable dtDict = dtd .ToDatatable ();// webDict.GetArticles();
          DataView dvDict = dtDict.DefaultView;
          dvDict.RowFilter = " ParentId=" + pid;

          if (dvDict.Count > 0)
          {
              foreach (DataRowView view in dvDict)
              {
                  int id = Convert.ToInt32(view["id"].ToString());
                  string name = view["title"].ToString();
                  int parentId = Convert.ToInt32(view["ParentId"].ToString());
                  PropertyNodeItem childNodeItem = new PropertyNodeItem()
                  {
                      DisplayName = name,
                      Name = name,
                      id = id,
                      parentId = parentId,
                      IsExpanded = false,
                      Icon = "/LongTie.Nlbs;component/Images/department.png"
                  };
                  ForeachPropertyNode(childNodeItem, id);
                  node.Children.Add(childNodeItem);
              }
          }
      }
    }


}
