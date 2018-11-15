using LT.DAL;
using LT.DAL.MySql;
using LT.DAL.Sqlite;
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

namespace LongTie.Nlbs
{
    /// <summary>
    /// Interaction logic for testdb.xaml
    /// </summary>
    public partial class testdb : Window
    {
        IUserRole UserRole = DataAccess.CreateUserRole();
        IProject Project = DataAccess.CreateProject();
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        IWrenchStatus Wrenchsatatus = DataAccess.CreateWrenchStatus();
        IUser User = DataAccess.CreateUser();
        IWrench Wrench = DataAccess.CreateWrench();
        ICheckTargetRecord CheckTargetRecode = DataAccess.CreateCheckTargetRecord();
        ICheckTarget ChceckTarget = DataAccess.CreateCheckTarget();
        IBorrow Borrow = new MySqlBorrow();
        IBorrowRecord BorrowRecord = new MySqlBorrowRecord();
        public testdb()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
           // system  s=Project.selectByname("LongTie.nlbs").FirstOrDefault ();

            List<role> _role = UserRole.Select();
            if (_role == null) return;
            MessageBox.Show("角色列表个数"+_role.Count );
              //UserRole .SelectBySysGuid (s.guid );
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.tb1.Text.Trim() == "系统名称（systemName）")
            { MessageBox.Show("请填写系统名成"); return; }
            system s = Project.selectByname(this.tb1 .Text .Trim()).FirstOrDefault();
            if (s == null) return;
            MessageBox.Show(s.systemName) ;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string mess="";
            system s = new system() { systemName =this.xtname .Text.Trim (),systemDM =xtid .Text .Trim (),guid =Guid .NewGuid ().ToString ()};
            if (Project.add(s))
            {
                mess = "添加成功";
            }
            else { mess = "添加失败！"; }
            MessageBox.Show(mess);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            torquechecktarget t = new torquechecktarget()
            {
                wrenchID ="1",
                checkDate =DateTime .Now,
                qaID ="1",operatorID ="1",
                torqueTargetValue =Convert .ToDecimal (32.99), 
                //errorRangeMax  =(decimal)0.5,
                //errorRangeMin =0,
                is_good =true ,
                guid =Guid.NewGuid ().ToString ()
            };
          MessageBox .Show ( CheckTarget.AddReturnGuid(t));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            role r = new role() { roleDM ="123", roleName = "test", system = "1", guid = Guid.NewGuid().ToString() };
            MessageBox.Show(UserRole.Add(r).ToString ());
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            wrenchspecies w = new wrenchspecies() { speciesName ="ww",speciesCode ="22",guid =Guid.NewGuid().ToString ()};
          MessageBox .Show (  WrenchSpecies.add(w).ToString ());

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            system s = new system() { systemName = this.xtname.Text.Trim(), systemDM = xtid.Text.Trim(), guid = Guid.NewGuid().ToString() };

            MessageBox.Show(Project.addReturnID(s));
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            List<system> list = new List<system>();
            for (int i = 0; i < 4; i++) { 
            system s = new system() { systemName = this.xtname.Text.Trim(), systemDM = xtid.Text.Trim(), guid = Guid.NewGuid().ToString() };
            list.Add(s);
            }
            MessageBox.Show(Project .addmany (list ).ToString ());
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            List<wrenchspecies> wc = new List<wrenchspecies>();
            for (int i = 0; i < 4; i++) {
                wrenchspecies w = new wrenchspecies() { speciesName = "ww", speciesCode = "22", guid = Guid.NewGuid().ToString() };
                wc.Add(w);
            }
           MessageBox .Show ( WrenchSpecies.addmany(wc).ToString ());
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (sp.Text.Trim() == "种类名称")
            { MessageBox.Show("请填写内容"); return; }
         List <wrenchspecies > ms= WrenchSpecies.selectbyname(this.sp.Text.Trim ());
         if (ms != null)
             MessageBox.Show(ms.Count.ToString());
         else
             MessageBox.Show("查询失败!");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            users u = new users() { username ="obm",password ="123456",is_staff =true  ,empID ="123",cardID ="123456",phoneNumber ="123455",department ="1",mail ="1229009876@qq.com", IDNum ="88", guid =Guid .NewGuid ().ToString ()};
            MessageBox .Show ( User.Add(u).ToString());
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            users u = new users() { username = "obm", password = "123456", is_staff = true , empID = "123", cardID = "123456", phoneNumber = "12345", department= "1", mail = "1229009876@qq.com", IDNum = "yyu", guid = Guid.NewGuid().ToString() };
            MessageBox.Show(User.addreturnid (u).ToString());
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            List<wrenchspecies> ws = WrenchSpecies.select();
            if (ws != null)
                MessageBox.Show(ws.Count.ToString());
            else
                MessageBox.Show("查询失败！");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            wrenchspecies w = new wrenchspecies() { speciesName = "ww", speciesCode = "22", guid = Guid.NewGuid().ToString() };
            MessageBox.Show(WrenchSpecies.addreturnid (w).ToString());
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            wrench w = new wrench() { createDate =DateTime.Now,factory ="nuba",rangeMax =Convert .ToDecimal (32.9),rangeMin =Convert .ToDecimal (0.5), species="1", status ="2",wrenchBarCode ="1234",wrenchCode ="2345"};
            MessageBox.Show(Wrench .add (w ).ToString ());
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            List<torquecheckrecord> tl = new List<torquecheckrecord>();
            for (int i = 0; i < 5; i++) {
            torquecheckrecord t = new torquecheckrecord() { analyserValue =Convert .ToDecimal (32.1),checkTime =DateTime .Now,isEffective =true ,TorqueCheckTargetID ="2",passedFlag =true ,guid =Guid .NewGuid ().ToString ()};
            tl.Add(t);
            }
          MessageBox.Show (  CheckTargetRecode.AddMany(tl).ToString ());
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            torquecheckrecord t = new torquecheckrecord() { analyserValue = Convert.ToDecimal(32.1), checkTime = DateTime.Now, isEffective = true , TorqueCheckTargetID = "2", passedFlag = true , guid = Guid.NewGuid().ToString() };
            MessageBox.Show(CheckTargetRecode.AddNotReturn (t).ToString ());
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {

            wrenchspecies w = new wrenchspecies() { speciesName =this.content .Text .Trim (), speciesCode = "123", guid = "e8ad22ee-1f00-4b2a-94ee-a84d3b692f7d" };
            WrenchSpecies.updata(w);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            wrench w= Wrench.selectByBarcode("222");
            w.rangeMax = 200;
            w.species = "e8ad22ee-1f00-4b2a-94ee-a84d3b692f7d";
            w.status = "123";
            Wrench.updata(w);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            wrenchstatus w = Wrenchsatatus.selectByguid("22");
            w.statusName = this.content.Text.Trim();
            Wrenchsatatus.update(w);
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            try
            {
                torquechecktarget t = CheckTarget.SelectByGuid("e54bd7d0-7b66-40cf-96ad-76b7cec237f3");
                t.torqueTargetValue = Convert.ToDecimal(this.content.Text.Trim());
              
                CheckTarget.Update(t);
            }
            catch { }

        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            borrow b = new borrow() { borrowDate =DateTime .Now .ToString ("yyyy-MM-dd hh:mm:ss"), borrowOperator ="222",borrowUser ="eee", guid =Guid .NewGuid ().ToString ()};
          MessageBox .Show (  Borrow.addreturnid (b).ToString() );
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            List<borrowrecord> br = new List<borrowrecord>();
            for (int i = 0; i < 3; i++) {
                borrowrecord b = new borrowrecord() { is_return =false ,returnOperator ="张三" ,returnUser="李四",WrenchID ="1",guid =Guid .NewGuid ().ToString ()};
                br.Add(b);
            }
          MessageBox .Show (  BorrowRecord.addmany(br).ToString ());
  
        }
       // IUser User = new MySqlUser();

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            users u = User.SelectByName("张三").FirstOrDefault();
            if (u == null) return;
            u.password = "000000";
            MessageBox.Show(User .Update (u).ToString ());
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            role r = UserRole.SelectByGuid("278c6576-8051-4d6d-afff-2b6cfb7dbfa3");
            r.roleName = "王八蛋";
            MessageBox.Show(UserRole.Update (r).ToString ());
        }

        /// <summary>
        /// 扳手借出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            IBorrow Borrow = new MySqlBorrow();
            borrow b = new borrow() { 
             WrenchID ="1",
            borrowDate =DateTime .Now .ToString ("yyyy-MM-dd HH:mm:ss"),
             borrowOperator = "374b4fde-f46d-446a-aa62-331764bc5f57",
             borrowUser = "374b4fde-f46d-446a-aa62-331764bc5f57",
            guid =Guid .NewGuid().ToString ()
            };

          MessageBox .Show (  Borrow.add(b).ToString ());
        }
        /// <summary>
        /// 扳手归还
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("guid", "f1e06654-dc4d-4d13-a549-837850b3a480");
            IBorrow Borrow = new MySqlBorrow();
         List <borrow >bl=Borrow.SelectByCondition(d);
            borrow b =bl.FirstOrDefault ();
            b.returnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            b.returnOperator = "374b4fde-f46d-446a-aa62-331764bc5f57";
            b.returnUser = "374b4fde-f46d-446a-aa62-331764bc5f57";
           MessageBox .Show ( Borrow.Update(b).ToString ());

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("guid", "f1e06654-dc4d-4d13-a549-837850b3a480");
            IBorrow Borrow = new MySqlBorrow();
            List<borrow> bl = Borrow.SelectByCondition(d);
            MessageBox.Show(bl.Count .ToString ());
        }
        /// <summary>
        /// 有条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            IUser User = DataAccess.CreateUser();
            MessageBox.Show(User.Select().FirstOrDefault().joinDate.ToString ());
        }

   
    }
}
