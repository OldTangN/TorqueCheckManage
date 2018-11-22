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
using System.Net;
using System.IO;
using QDDL.Model;
using QDDL.DAL.Service;
using QDDL.DAL.MySql;
using QDDL.DAL;
using QDDL.Comm;
using QDDL.BLL;

namespace Manager
{
    /// <summary>
    /// AddDepart.xaml 的交互逻辑
    /// </summary>
    public partial class winEditorRole
    {
        IUserRole UserRole = DataAccess.CreateUserRole();
        IProject Project = DataAccess.CreateProject();
        role _role = new role();
        bool isadd = true;

        public  winEditorRole()
        {
            InitializeComponent();
            BindRole();           
        }

        void BindRole()
        {
            try
            {
                this.dataGrid1.ItemsSource = null;
                List<system> sl = Project.selectByname("智能扭矩校验台");
                if (sl.Count > 0)
                {
                    List<role> rl = UserRole.SelectBySysGuid(sl.FirstOrDefault().guid);

                    this.dataGrid1.ItemsSource = rl;
                }
            }
            catch { }
        
        }

        void ShowRole(role r)
        {
            if (r == null)
                return;
            this.tbox_roleName.Text = r.roleName;
            this.tbox_dm.Text = r.roleDM;
            this.tbox_comment.Text = r.comment;
        }

         role  GetEditerRole(role r)
        {
            r.roleName = tbox_roleName.Text.Trim();
            r.roleDM = tbox_dm.Text.Trim();
            r.comment = tbox_comment.Text.Trim();
            return r;
        }

         bool IsEmpty()
         {
             if (string.IsNullOrEmpty(this.tbox_roleName .Text .Trim ()) || string.IsNullOrEmpty(this.tbox_dm .Text .Trim ()))
             {
                 MessageAlert.Alert("请填写必要信息！");
                 return true ;
             }
             return false ;
         }
         bool IsExit(role r,int count=0)
         {
             GetRole gr = new GetRole();
             List<role> rl = gr.getrole();        
             if (rl.FindAll (p => p.roleName == r.roleName ).Count>count)
             {
                 MessageAlert.Alert("该名称已经存在！");
                 return true ;
             }
        
             if (rl.FindAll (p => p.roleDM == r.roleDM ).Count >count)
             {
                 MessageAlert.Alert("该编号已经存在！");
                 return true ;
             }
             return false ;
         }
         bool IsUpdataRepeat(role r)
         {

             GetRole gr = new GetRole();
             List<role> rl = gr.getrole();
             List <role >temprl=rl.FindAll(p => p.roleName == r.roleName);
             foreach (role re in temprl )
             {
             if (re!=null&&re.guid !=r.guid )
             {
                 MessageAlert.Alert("该名称已经存在！");
                 return true;
             }
             }
             temprl =rl.FindAll(p => p.roleDM == r.roleDM);
             foreach (role re in temprl )
             {
             if (re!=null&&re.guid !=r.guid)
             {
                 MessageAlert.Alert("该编号已经存在！");
                 return true;
             }
             }
             return false;
         }

         bool Save(role r)
         {
             List<system> ss = Project.selectByname("智能扭矩校验台");
             if (ss == null || ss.Count <= 0)
             {
             
                 MessageAlert.Error("该系统名称尚未添加到数据库！\n   请联系管理员!");
                 return false;
             }                     
             r.system = ss.FirstOrDefault().id.ToString();
             r.guid = Guid.NewGuid().ToString();
             return UserRole.Add(r); 
         }

         bool Update(role r)
         {
             return UserRole.Update(r);
         }

         void SetEmpty()
         {
             this.tbox_comment.Clear();
             this.tbox_dm.Clear();
             this.tbox_roleName.Clear();
             isadd = true;
             _role = null;
         }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsEmpty())
                    return;
                if (isadd)
                {
                    role r = new role();
                       r= GetEditerRole(r);
                    if (IsExit(r,0))
                        return;
                    if (!Save(r))
                    {
                        MessageAlert.Error("添加失败！");                   
                        return;
                    }
                    MessageAlert.Alert("添加成功！");
                }
                else
                { 
                    _role =GetEditerRole (_role);
                    if (IsUpdataRepeat(_role))
                        return;
                    if (!Update(_role))
                    {
                        MessageAlert.Error("修改失败！");                    
                        return;
                    }
                    MessageAlert.Alert("修改成功！");                 
                }
                BindRole();
                SetEmpty();
            }
            catch 
            {
                MessageAlert.Error("出现错误无法添加信息！");
            }
                 
        }



        
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }

        }

        #region
        void cbrolebinding() {
            List<string> role = new List<string>() { "校验员","质检员"};
            cb_rolename.ItemsSource = role;
           // cb_rolename.SelectedIndex = 1;
        }
        private void cb_rolename_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_rolename.SelectedIndex > 0) {
                
                    string s= cb_rolename.SelectedItem.ToString();
                    this.tbox_roleName.Text = s;
                    
            }
        }
        #endregion

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            SetEmpty();
        }


        void clear()
        {
            this.tbox_comment.Clear();
            this.tbox_dm.Clear();
            this.tbox_roleName.Clear();
            isadd = true;
            _role = null;
        }
        private void editerButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex <0)
                return;
            _role = this.dataGrid1.SelectedItem as role;
            ShowRole(_role);
            isadd = false;
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid1.SelectedIndex >= 0)
            {
                role r = this.dataGrid1.SelectedItem as role;
                if (r != null)
                {
                    if (UserRole.Del(r))
                    {
                        MessageAlert.Alert("删除成功！");
                    }
                    else
                    {
                        MessageAlert.Alert("该条信息不能删除！");
                    }
                }
                BindRole();  
            }
        }

    }
}

