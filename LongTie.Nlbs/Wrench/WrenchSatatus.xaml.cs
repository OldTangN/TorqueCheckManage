using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LongTie.Nlbs.Wrench
{
    /// <summary>
    /// Interaction logic for WrenchSatatus.xaml
    /// </summary>
    public partial class WrenchSatatus
    {
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        wrenchstatus _wrenchstatus = new wrenchstatus();
        bool isadd = true;
        public WrenchSatatus()
        {
            InitializeComponent();
            databind(getsatus ());
        }

          List <wrenchstatus> getsatus()
        {
            return WrenchStatus.selectAll();
        }

          void databind(List <wrenchstatus > wslist) {

              this.dt_showdata.ItemsSource = null;
              this.dt_showdata.ItemsSource = wslist;

          }
          void showdata(wrenchstatus ws) {
              this.tb_statuscode.Text  = ws.statusDM;
              this.tb_statusname.Text = ws.statusName;

          }

          private void bt_editer_Click(object sender, RoutedEventArgs e)
          {
              if (this.dt_showdata.SelectedIndex < 0) { return; }
              _wrenchstatus = dt_showdata.SelectedItem as wrenchstatus;
              showdata(_wrenchstatus);
              isadd = false;
          }
          bool IsExit(wrenchstatus w)
          {
              wrenchstatus wl = WrenchStatus.selectByName(w.statusName);
              if (wl != null) { MessageAlert.Alert("该状态已经存在！"); return true ; }
              wl = WrenchStatus.selectByStatusDM(w.statusDM);
              if (wl != null) { MessageAlert.Alert("该工具编号已经存在！"); return true ; }
              return false;
          }
          bool IsUpdataRepeat(wrenchstatus w)
          {
            
              wrenchstatus wl = WrenchStatus.selectByName(w.statusName);
              if (wl != null&&wl.guid !=w.guid ) { MessageAlert.Alert("该状态已经存在！"); return true; }
              wl = WrenchStatus.selectByStatusDM(w.statusDM);
              if (wl!= null&&wl.guid !=w.guid ) { MessageAlert.Alert("该工具编号已经存在！"); return true; }
              return false;
          }
          bool save(wrenchstatus  w) {       
              return WrenchStatus.add (w);
          }

          private void Button_Click(object sender, RoutedEventArgs e)
          {
              if (this.tb_statusname.Text.Trim() == "")
              { MessageAlert.Warning("请填写状态名称"); return; }
              if (this.tb_statuscode.Text.Trim() == "") { MessageAlert.Warning("请填写状态编号！"); return; }
              try
              {
                  if (isadd)
                  {
                      wrenchstatus w = new wrenchstatus();
                      w.statusDM = this.tb_statuscode.Text.Trim();
                      w.statusName = this.tb_statusname.Text.Trim();
                      w.guid = Guid.NewGuid().ToString();
                      if (IsExit(w))
                          return;
                      if (save(w))
                      {
                          MessageAlert.Alert("保存成功！");
                          databind(getsatus());
                          clear();
                          return;
                      }
                  }
                  else
                  {
                      _wrenchstatus.statusDM = this.tb_statuscode.Text.Trim();
                      _wrenchstatus.statusName = this.tb_statusname.Text.Trim();
                      wrenchstatus w = new wrenchstatus();
                         w= _wrenchstatus;
                      if (IsUpdataRepeat(w))
                          return;
                      if (WrenchStatus.update(w))
                      {
                          MessageAlert.Alert("保存成功！");
                          databind(getsatus());
                          clear();
                          return;
                      }
                  }
                  MessageAlert.Alert("保存失败！");
              }
                  
              catch { MessageAlert.Alert("保存失败！"); }

          }

          private void Button_Click_1(object sender, RoutedEventArgs e)
          {
              clear();
          }
          void clear() {
              _wrenchstatus = null;
              isadd = true;
              this.tb_statuscode.Clear();
              this.tb_statusname.Clear();
          }

          private void bt_del_Click(object sender, RoutedEventArgs e)
          {
              if (this.dt_showdata.SelectedIndex >= 0)
              {
                  wrenchstatus w = this.dt_showdata.SelectedItem as wrenchstatus;
                  if (w != null)
                  {
                      if (WrenchStatus.Del(w))
                      {
                          MessageAlert.Alert("删除成功！");
                      }
                      else 
                      {
                          MessageAlert.Alert("该条记录不能删除！");
                      }
                  }
                  databind(getsatus());
              }

          }
    }
}
