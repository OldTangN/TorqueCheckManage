using LongTie.Nlbs.Common;
using LongTie.Nlbs.User;
using LongTie.Nlbs.Wrench;
using LT.BLL;
using LT.BLL.Borrow;
using LT.BLL.Check;
using LT.BLL.Wrench;
using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LongTie.Nlbs.Borrow
{
    /// <summary>
    /// WrenchReturn.xaml 的交互逻辑
    /// </summary>
    public partial class WrenchReturn 
    {

        IWrench Wrench = DataAccess.CreateWrench();
        IUser User = DataAccess.CreateUser();
        IBorrow Borrow = new MySqlBorrow();
        ReadUserCard ruc = null;
       // System.Timers.Timer aTimer = null;
        FilterData filterdata = new FilterData();
        GetUser getuser = new GetUser();
        
        userinfo borrowuser = null;
        string backcard = "";
        List<BorrowWrench> borrowwrenchlist = new List<BorrowWrench>();
        List<ReturnWrench> _returnwrench = new List<ReturnWrench>();
        private delegate void TimerDispatcherDelegate();
        public WrenchReturn(ReadUserCard r)
        {
            InitializeComponent();
            ruc = r;
      
            //aTimer = new System.Timers.Timer(1000);
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 10;
            //aTimer.Enabled = true;
            ruc.HandDataBack += new ReadUserCard.DeleteDataBack(BackCardID);
        }
        void BackCardID(object sender, EventArgs e)
        {
            ReadUserCard ruc = (ReadUserCard)sender;
            backcard = ruc.BackString();
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(UpdateCardInfo));
        }
   
        void UpdateCardInfo()
        {
            try
            {
                filterdata.CardId = backcard;
                filterdata.resetCard();
                string cardid = filterdata.getcardid();
                if (cardid == "")
                    return;
                filterdata.resetid("");
                this.cardid.Text = cardid; 

                borrowuser = getuser.getuserinfo(getuser.getusers(cardid));
                if (borrowuser != null && borrowuser.user != null)
                {
                    this.username.Text = borrowuser.user.username;
                    this.telphone.Text = borrowuser.user.phoneNumber;
                    this.cb_user.IsEnabled = true;
                    _returnwrench = GetBorrowInfo(borrowuser.user.guid);
                    BorrowdgBind(_returnwrench);
                }
                else
                {
                    this.username.Text = "";
                    this.telphone.Text ="";
                }
            }
            catch { }
        }


        private void cb_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_user.SelectedIndex == 1)
            {
                UserShow us = new UserShow(borrowuser);
                us.ShowDialog();
            }
            if (cb_user.SelectedIndex == 2)
            {
                WrenchBorrowHistory wbh = new WrenchBorrowHistory();
                List<BorrowHistory> bhl = wbh.GetByUser(Borrow.SelectByUser(SystData.userInfo.user.guid));
                UserBorrow ub = new UserBorrow(bhl);
                ub.ShowDialog();
            }
            this.cb_user.SelectedIndex = 0;
        }
        private void cb_wrench_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cb_wrench.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
                {
                    GetWrenchInfo gwi = new GetWrenchInfo();
                    wrenchinfo wi = gwi.GetWrenchinfo(this.wrenchbarcode.Text.Trim());
                    if (wi == null)
                    {
                        MessageAlert.Alert("没有任何详细信息！");
                    }
                    else
                    {
                        WrenchShow us = new WrenchShow(wi);
                        us.ShowDialog();
                    }

                }
            }
            if (cb_wrench.SelectedIndex == 2)
            {
                if (string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
                {
                    return;
                }
                WrenchBorrowHistory wbh = new WrenchBorrowHistory();
                wrench w = Wrench.selectByBarcode(this.wrenchbarcode.Text.Trim());
                if (w == null)
                    return;
                List<BorrowHistory> bhl = wbh.GetByUser(Borrow.SelectByWrench(w.guid));
                UserBorrow ub = new UserBorrow(bhl);
                ub.ShowDialog();
            }
            this.cb_wrench.SelectedIndex = 0;
        }

        private void bt_card_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cardid.Text.Trim()))
            {
                MessageAlert.Alert("请填写卡号！");
                return;
            }
            borrowuser = getuser.getuserinfo(getuser.getusers(this.cardid.Text.Trim()));
            if (borrowuser != null &&borrowuser.user != null)
            {
                this.username.Text = borrowuser.user.username;
                this.telphone.Text = borrowuser.user.phoneNumber;
                cb_user.IsEnabled = true;
                _returnwrench = GetBorrowInfo(borrowuser.user.guid);
                BorrowdgBind(_returnwrench);
            }

        }
        private void bt_wrenchbarcode_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(this.wrenchbarcode.Text.Trim()))
            {
                MessageAlert.Alert("请填写扳手编号");
                return;
            }
          if(GetWrench(this.wrenchbarcode.Text.Trim()))
            DelReurnDg(this.wrenchbarcode.Text.Trim());
        }
        void DelReurnDg(string wrenchbarcode)
        {           
            if (_returnwrench == null || _returnwrench.Count <= 0)
                return;
            _returnwrench.RemoveAt(_returnwrench.FindIndex(p => p.wrenchbarcode == wrenchbarcode));
            BorrowdgBind(_returnwrench);
        }

        bool GetWrench(string wrenchbarcode)
        {
            if (borrowuser == null)
            {
                MessageAlert.Alert("没有借用人信息");
                return false ;
            }
            if (borrowwrenchlist.FindIndex(p => p.wrenchbarcode == wrenchbarcode) >= 0)
            {
                MessageAlert.Alert("不能重复添加！");
                return false ;
            }

            try
            {
                wrench w = Wrench.selectByBarcode(wrenchbarcode);
                if (w != null)
                {
                    List<borrow> bl = Borrow.SelectByWrench(w.guid, false);
                    if (bl == null || bl.Count <= 0)
                        return false;
                    borrowwrenchlist.Add(new BorrowWrench()
                    {
                        wrenchbarcode = w.wrenchBarCode,
                        wrenchcode = w.wrenchCode,
                        factory = w.factory,
                        borrowdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        userguid = borrowuser.user.guid,
                        username = borrowuser.user.username,
                        operatorguid = SystData.userInfo.user.guid,
                        options = SystData.userInfo.user.username,
                        wrenchid = w.id.ToString(),
                        wrenchguid = w.guid

                    });
                    ShowWrench(w);
                    DgBind(borrowwrenchlist);
                    return true;
                }
                return false;
            }
            catch { return false; }
     
        }

        void DgBind(List<BorrowWrench> bl)
        {
            this.gd_borrow.ItemsSource = null;
            this.gd_borrow.ItemsSource = bl;
            this.count.Text = bl.Count.ToString();
        }

        void ShowWrench(wrench e)
        {
            if (e == null)
                return;      
                this.wrenchcode.Text = e.wrenchCode;        
                this.cb_wrench.IsEnabled = true;
        }
        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            if (this.gd_borrow.SelectedIndex < 0)
                return;
            BorrowWrench bw = this.gd_borrow.SelectedItem as BorrowWrench;
            if (bw == null || bw.wrenchbarcode == null)
                return;
            borrowwrenchlist.RemoveAt(borrowwrenchlist.FindIndex(p => p.wrenchbarcode == bw.wrenchbarcode));
            DgBind(borrowwrenchlist);
            GetReturnWrench(bw);
            BorrowdgBind(_returnwrench);
        }
        ReturnWrench GetReturnWrench(BorrowWrench br)
        {
            ReturnWrench rw = new ReturnWrench();
            List<borrow> bl = Borrow.SelectByWrench(br.wrenchguid ,br.userguid,false );
            if (bl != null && bl.Count > 0)
            {

                borrow b = bl.FirstOrDefault();
                wrench w = Wrench.selectByguid(b.WrenchID);
                users u = User.SelectByguid(b.borrowUser);
                users operatos = User.SelectByguid(b.borrowOperator);
                _returnwrench.Add(new ReturnWrench()
                {
                    wrenchbarcode = w.wrenchBarCode,
                    wrenchcode = w.wrenchCode,
                    wrenchguid = w.guid,
                    cardid = u.cardID,
                    username = u.username,
                    factory = w.factory,
                    rang = w.rangeMin.ToString("f2") + "~" + w.rangeMin.ToString("f2"),
                    borrowdate = b.borrowDate.Replace('T', ' '),
                    operater = operatos.username,
                    wrenchcommit = w.comment
                });

                return rw;
            }
            return null;
    
        }
        private void wrenchbarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.wrenchbarcode.SelectAll();
                this.bt_wrenchbarcode_Click(this, e);
            }
        }

        private void bt_submit_Click(object sender, RoutedEventArgs e)
        {
            if (borrowwrenchlist == null || borrowwrenchlist.Count <= 0)
            {
                MessageAlert.Alert("没有数据");
                return ;
            }

            if (Update())
            {
                MessageAlert.Alert("提交成功！");
                Clear();
            }
            else
            {
                if (MessageAlert.Alter("是否清空数据！"))
                {
                    Clear();
                }
            }
        }
        List<ReturnWrench> GetBorrowInfo(string userguid)
        {
            try
            {
                List<ReturnWrench> rwl = new List<ReturnWrench>();
                List<borrow> bl = Borrow.SelectByUser(userguid, false);
                if (bl == null || bl.Count <= 0)
                    return null;

                foreach (borrow b in bl)
                {
                    wrench w = Wrench.selectByguid(b.WrenchID);
                    users u = User.SelectByguid(b.borrowUser);
                    users operatos = User.SelectByguid(b.borrowOperator);
                    rwl.Add(new ReturnWrench()
                    {
                        wrenchbarcode = w.wrenchBarCode,
                        wrenchcode = w.wrenchCode,
                        wrenchguid = w.guid,
                        cardid = u.cardID,
                        username = u.username,
                        factory = w.factory,
                        rang = w.rangeMin.ToString("f2") + "~" + w.rangeMin.ToString("f2"),
                        borrowdate = b.borrowDate.Replace('T', ' '),
                        operater = operatos.username,
                        wrenchcommit = w.comment
                    });
                }
                return rwl;
            }
            catch { return null; }
        }

        void BorrowdgBind(List<ReturnWrench> rwl)
        {
            this.gd_return.ItemsSource = null;
            this.gd_return.ItemsSource = rwl;
        }
        void Clear()
        {
            borrowwrenchlist.Clear();
            _returnwrench.Clear();
            DgBind(borrowwrenchlist);
            borrowuser = null;
            this.username.Text = "";
            this.telphone.Text = "";            
            this.wrenchcode.Text = "";
            this.wrenchbarcode.Clear();
            this.cardid.Clear();
            cb_user.IsEnabled = false;
            cb_wrench.IsEnabled = false;
            BorrowdgBind(null);
        }
        /// <summary>
        /// 只要找到
        /// </summary>
        /// <returns></returns>
        bool Update()
        {
       
            try
            {
                foreach (BorrowWrench b in borrowwrenchlist)
                {
                    //默认归还第一条
                   List <borrow> bl = Borrow.SelectByWrench(b.wrenchguid ,false);
                   if (bl != null && bl.Count > 0)
                   {
                       borrow bw = bl.FirstOrDefault();
                       bw.is_return = true;
                       bw.returnDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                       bw.returnOperator = SystData.userInfo.user.guid;
                       bw.returnUser = borrowuser.user.guid;
                       Borrow.Update(bw);
                   }
                }
            }
            catch { return false; }
            return true;
        }

        private void bt_returnall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_returnwrench == null || _returnwrench.Count <= 0)
                    return;
                foreach (ReturnWrench rw in _returnwrench)
                {
                    GetWrench(rw.wrenchbarcode);                       
                }
                _returnwrench.Clear();
                BorrowdgBind(_returnwrench);
            }
            catch { }
        }

        private void cardid_GotFocus(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void wrenchbarcode_GotFocus(object sender, RoutedEventArgs e)
        {
            this.wrenchcode.Text = "";
        }
        
    }
}
