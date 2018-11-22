using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QDDL.Model;
using QDDL.DAL.Service;
using QDDL.DAL;
using QDDL.DAL.MySql;

namespace Manager
{
    /// <summary>
    /// zhouduantablexaml.xaml 的交互逻辑
    /// </summary>
    public partial class winRole:Window 
    {
        private List<role> listRole = new List<role>();
        IUserRole userrole = DataAccess.CreateUserRole();
        IProject project = DataAccess.CreateProject();
        public winRole()
        {
            InitializeComponent();
            loadroledata();
        }
        private void loadroledata() {
            try
            {
                List<QDDL.Model.system> syslist = new List<QDDL.Model.system>();
                syslist = project.selectByname("QDDL.nlbs");
                if (syslist != null || syslist.Count > 0)
                {
                    dataGrid1.ItemsSource = userrole.SelectBySysGuid(syslist.FirstOrDefault().guid);
                }
            }
            catch { }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
            //winEditorRole wed =winEditorRole.GetAddWinEditerRole  ();
            //wed.ShowDialog ();
           // this.Close();
        }
        /// <summary>
        /// 角色编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItems ==null||dataGrid1 .SelectedItems .Count <=0) {
                MessageBox.Show("请选择要修改的行");
                return;
            }

            role temprole = dataGrid1.SelectedItem as role;
            //winEditorRole wer = winEditorRole.GetWinEditerRole(temprole);
            //wer.ShowDialog ();
            //this.Close();
                
        }

        
    }
     
}
