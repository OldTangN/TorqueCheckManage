using QDDL.Comm;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace QDDL.Nlbs.SystemSet
{
    /// <summary>
    /// Interaction logic for TesterSet.xaml
    /// </summary>
    public partial class TesterSet
    {
        SerialPort port = new SerialPort();
        List<TorqueTestModel> ttmlist = new List<TorqueTestModel>();
        public TesterSet()
        {
            InitializeComponent();
            getport();
            getbanudrate();
            showtester();
        }

        void gettestersetlist()
        {

        }
        void getport()
        {
            string[] portnames = SerialPort.GetPortNames();
            this.cb_2name.Items.Add("");
            this.cb_name.Items.Add("");
            foreach (string s in portnames)
            {
                this.cb_name.Items.Add(s);
                this.cb_2name.Items.Add(s);
            }
            if (portnames.Length > 0)
            {

                this.cb_name.SelectedIndex = 0;
                this.cb_2name.SelectedIndex = 0;
            }
        }
        void getbanudrate()
        {
            List<int> rate = new List<int>() {9600, 115200 };
            foreach (int i in rate)
            {
                this.cb_2baundrate.Items.Add(i);
                this.cb_baundrate.Items.Add(i);
            }
        }

        void gettester1content()
        {
            TorqueTestModel t = new TorqueTestModel();
            t.testername = this.tb_name.Text.Trim();
            t.portname = this.cb_name.SelectedItem == null ? "" : this.cb_name.SelectedItem.ToString();
            t.databit = Convert.ToInt32(this.tb_databit.Text.Trim());
            t.minvalue = Convert.ToDecimal(this.tb_1min.Text.Trim());
            t.maxvalue = Convert.ToDecimal(this.tb_1max.Text.Trim());
            t.baundrate = Convert.ToInt32(this.cb_baundrate.SelectedItem);
            if (ttmlist.FindIndex(p => p.testername == t.testername) >= 0)
                ttmlist.RemoveAt(ttmlist.FindIndex(p => p.testername == t.testername));
            // List<TorqueTestModel> ltt = new List<TorqueTestModel>();
            TorqueTestModel ltt = ttmlist.Find(p => p.testername == "校验仪2");
            if (ltt != null)
            {

                if ((t.maxvalue <= ltt.maxvalue && t.maxvalue > ltt.minvalue) || (t.minvalue < ltt.maxvalue && t.minvalue >= ltt.minvalue))
                {
                    MessageAlert.Alert("校验仪1量程不能与校验仪2量程重合");
                    return;
                }
                if (t.minvalue <= ltt.minvalue && t.maxvalue >= ltt.maxvalue)
                {
                    MessageAlert.Alert("校验仪1量程不能与校验仪2量程重合");
                    return;
                }

            }
            ttmlist.Add(t);
            MessageAlert.Alert("校验仪1设置成功！");
        }
        void gettester2content()
        {
            TorqueTestModel t = new TorqueTestModel();
            t.testername = this.tb_2name.Text.Trim();
            t.portname = this.cb_2name.SelectedItem == null ? "" : this.cb_2name.SelectedItem.ToString();
            t.databit = Convert.ToInt32(this.tb_2databit.Text.Trim());
            t.minvalue = Convert.ToDecimal(this.tb_2min.Text.Trim());
            t.maxvalue = Convert.ToDecimal(this.tb_2max.Text.Trim());
            t.baundrate = Convert.ToInt32(this.cb_2baundrate.SelectedItem);
            if (ttmlist.FindIndex(p => p.testername == t.testername) >= 0)
                ttmlist.RemoveAt(ttmlist.FindIndex(p => p.testername == t.testername));

            TorqueTestModel ltt = ttmlist.Find(p => p.testername == "校验仪1");
            if (ltt != null)
            {

                if ((t.maxvalue <= ltt.maxvalue && t.maxvalue >ltt.minvalue) || (t.minvalue < ltt.maxvalue && t.minvalue >= ltt.minvalue))
                {
                    MessageAlert.Alert("校验仪2量程不能与校验仪1量程重合");
                    return;
                }
                if (t.minvalue <= ltt.minvalue && t.maxvalue >= ltt.maxvalue)
                {
                    MessageAlert.Alert("校验仪2量程不能与校验仪1量程重合");
                    return;
                }

            }
            ttmlist.Add(t);
            MessageAlert.Alert("校验仪2设置成功！");

        }
        void showtester()
        {
            try
            {
                ttmlist = SerializeXML<TorqueTestModel>.Getlist();
                if (ttmlist.Count > 0)
                {
                    TorqueTestModel t1 = ttmlist.Find(p => p.testername == "校验仪1");
                    TorqueTestModel t2 = ttmlist.Find(p => p.testername == "校验仪2");
                    if (t1 != null)
                    {
                        this.tb_1max.Text = t1.maxvalue.ToString();
                        this.tb_1min.Text = t1.minvalue.ToString();
                        this.tb_databit.Text = t1.databit.ToString();
                        foreach (string s in cb_name.Items)
                        {
                            if (s == t1.portname)
                            {
                                cb_name.SelectedItem = t1.portname;
                            }
                        }

                        foreach (int i in cb_baundrate.Items)
                        {
                            if (i == t1.baundrate) { cb_baundrate.SelectedItem = t1.baundrate; }
                        }
                    }
                    if (t2 != null)
                    {
                        this.tb_2max.Text = t2.maxvalue.ToString();
                        this.tb_2min.Text = t2.minvalue.ToString();
                        this.tb_2databit.Text = t2.databit.ToString();
                        foreach (string s in cb_2name.Items)
                        {
                            if (s == t2.portname)
                            {
                                cb_2name.SelectedItem = t2.portname;
                            }
                        }

                        foreach (int i in cb_2baundrate.Items)
                        {
                            if (i == t2.baundrate) { cb_2baundrate.SelectedItem = t2.baundrate; }
                        }
                    }
                }
            }
            catch { }
        }
        void savetester()
        {
            if (SerializeXML<TorqueTestModel>.exit())
                SerializeXML<TorqueTestModel>.del();
            SerializeXML<TorqueTestModel>.SaveList(ttmlist);
        }

        private void bt_set1_Click(object sender, RoutedEventArgs e)
        {
            if (cb_name.SelectedIndex < 0)
            {
                MessageAlert.Warning("请选择端口号");
                return;
            }

            if (this.tb_databit.Text.Trim() == "")
            {
                MessageAlert.Warning("请填写数据位中的信息！");
                return;
            }
            if ((this.tb_1max.Text.Trim() == "") || (this.tb_1min.Text.Trim() == ""))
            {
                MessageAlert.Warning("请添写校验范围！");
                return;
            }
            try
            {
                if (Convert.ToDecimal(this.tb_1min.Text.Trim()) > Convert.ToDecimal(this.tb_1max.Text.Trim()))
                {
                    MessageAlert.Warning("填写量程范围有误！");
                    return;
                }
            }
            catch { MessageAlert.Warning("量程请填写数字！"); }

            gettester1content();
            savetester();

        }

        private void bt_set2_Click(object sender, RoutedEventArgs e)
        {
            if (cb_2name.SelectedIndex < 0)
            {
                MessageAlert.Warning("请选择端口号");
                return;
            }

            if (this.tb_2databit.Text.Trim() == "")
            {
                MessageAlert.Warning("请填写数据位中的信息！");
                return;
            }
            if ((this.tb_2max.Text.Trim() == "") || (this.tb_2min.Text.Trim() == ""))
            {
                MessageAlert.Warning("请添写校验范围！");
                return;
            }
            try
            {
                if (Convert.ToDecimal(this.tb_2min.Text.Trim()) > Convert.ToDecimal(this.tb_2max.Text.Trim()))
                {
                    MessageAlert.Warning("填写量程范围有误！");
                    return;
                }
            }
            catch { MessageAlert.Warning("量程请填写数字！"); }

            gettester2content();
            savetester();


        }
    }
    
}
