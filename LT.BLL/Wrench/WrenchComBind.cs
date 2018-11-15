using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LT.BLL.Wrench
{
  public   class WrenchComBind
    {
      IWrenchStatus Wrench = DataAccess.CreateWrenchStatus();
      public void StatusBind(ComboBox cb) 
      {
          List<wrenchstatus> status = Wrench.selectAll();
          cb.ItemsSource = status;
          cb.DisplayMemberPath = "statusName";
          cb.SelectedValuePath = "id";
      }
      public void AlterBind(ComboBox cb)
      {
       
          List<wrenchstatus> status = Wrench.selectAll();
          List<wrenchstatus> ws = new List<wrenchstatus>();
          foreach (wrenchstatus w in status)
          {
              if (w.statusDM == "001" || w.statusDM == "002")
                  ws.Add(w);
          }
          cb.ItemsSource =ws;
          cb.DisplayMemberPath = "statusName";
          cb.SelectedValuePath = "id";
      }

    }
  public class statusalter
  {
      public string Name { get; set; }
      public string Code { get; set; }//0正常到维护//1 维护到正常
  }
}
