using LT.BLL.Trees;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
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

namespace LongTie.Nlbs.User
{
    /// <summary>
    /// Interaction logic for DepartmentEditer.xaml
    /// </summary>
    public partial class DepartmentEditer
    {
        LoadTree lt = null;
        IDepartment Department = DataAccess.CreateDepartment();
        public DepartmentEditer()
        {
            InitializeComponent();

        }

        private List<DepartmentModel> GetDepartmentModel()
        {
            List<DepartmentModel> departmentmodellist = new List<DepartmentModel>();
            List<department> departmentlist = Department.SelectByFlag(false);
            if (departmentlist != null && departmentlist.Count > 0)
            {
                foreach (department d in departmentlist)
                {
                    department pd = new department();
                    pd.departmentName = "";
                    if (d.parentDepartment != "")
                        pd = Department.SelectByGuid(d.parentDepartment);

                    departmentmodellist.Add(new DepartmentModel()
                    {
                        delDepartment = false,
                        parentname = pd == null ? "" : pd.departmentName,
                        id = d.id,
                        guid = d.guid,
                        departmentName = d.departmentName,
                        comment = d.comment
                    });
                }

            }
            return departmentmodellist;
        }


        private void tvProperties_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //MessageBox.Show("选择该变");

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var c = tvProperties.SelectedItem as PropertyNodeItem;
            if (c == null)
            {
                c = new PropertyNodeItem();
                if (MessageAlert.Alter("是否添加顶级部门！"))
                    c.id = 0;
                else
                {
                    MessageAlert.Alert("请选则上级部门");
                    return;
                }
            }


            winDepartment wd = new winDepartment(c.id, c.id, true);
            wd.ShowDialog();
            this.tvProperties.ItemsSource = null;
            lt.TreeLoad();
            this.dataGrid1.ItemsSource = GetDepartmentModel();

        }

        private void editer_Click(object sender, RoutedEventArgs e)
        {
            var c = tvProperties.SelectedItem as PropertyNodeItem;
            if (c == null)
            {
                MessageAlert.Alert("请先选择要操作的部门");
                return;
            }
            winDepartment wd = new winDepartment(c.parentId, c.id, false);
            wd.ShowDialog();
            this.tvProperties.ItemsSource = null;
            lt.TreeLoad();
            this.dataGrid1.ItemsSource = GetDepartmentModel();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.dataGrid1.ItemsSource = GetDepartmentModel();
            lt = new LoadTree(this.tvProperties);
            lt.TreeLoad();
        }



        private void del_Click(object sender, RoutedEventArgs e)
        {
            var c = tvProperties.SelectedItem as PropertyNodeItem;
            if (c == null)
            {
                MessageAlert.Alert("请先选择要操作的部门");
                return;
            }

            department d = new department();

            if (c.Children.Count > 0)
            {
                MessageAlert.Alert("该部门存在子部门\n不能删除！");
                return;
            }
            if (MessageAlert.Alter("是否删除！"))
            {
                d = Department.SelectById(c.id.ToString());
                d.delDepartment = true;
                if (Department.Update(d))
                {
                    MessageAlert.Alert("删除成功！");
                }
            }
            lt.TreeLoad();
            this.dataGrid1.ItemsSource = GetDepartmentModel();
        }
    }
}
