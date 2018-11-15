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
    /// Interaction logic for WrenchErrorrang.xaml
    /// </summary>
    public partial class WrenchErrorrang 
    {
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        List<errorrangset> errorset = new List<errorrangset>();
        errorrangset _errorrangset = new errorrangset();
        bool isadd = true;
        public WrenchErrorrang()
        {
            InitializeComponent();
           
            BindSpecies();
            getErrorRanng();
        }
        void BindSpecies()
        {
            List<wrenchspecies> ws = new List<wrenchspecies>();
            ws = WrenchSpecies.select();
            this.cb_species.ItemsSource = ws;
            this.cb_species.DisplayMemberPath = "speciesName";
            this.cb_species.SelectedValuePath = "guid";
        }
        void getErrorRanng()
        {

            errorset = new List<errorrangset>();
            try
            {
                errorset = SerializeXML<errorrangset>.Getlist();
            }
            catch
            {
                //MessageAlert.Alert("没有误差方案数据！"); return;
            }
            if (errorset.Count >= 0)
            {
                this.dg_set.ItemsSource = null;
                this.dg_set.ItemsSource = errorset;
            }

        }
        bool IsRight()
        {

            if (cb_species.SelectedIndex < 0)
            {
                MessageAlert.Warning("请选择扳手种类！");
                return false ;
            }
            if (string.IsNullOrEmpty(this.tb_min.Text.Trim()) || (string.IsNullOrEmpty(this.tb_max.Text.Trim())) || string.IsNullOrEmpty(this.tb_errormax.Text.Trim()) || string.IsNullOrEmpty(this.tb_errormin.Text.Trim()))
            {
                MessageAlert.Warning("请填写必要信息！");
                return false ;
            }
            decimal rmax = 0, rmin = 0, max = 0, min = 0;
            try 
            {
                rmax = Convert.ToDecimal(this.tb_errormax .Text .Trim ());
                rmin = Convert.ToDecimal(this.tb_errormin .Text.Trim ());
                max = Convert.ToDecimal(this.tb_max.Text.Trim());
                min = Convert.ToDecimal(this.tb_min.Text .Trim ());
                if (rmax <= rmin)
                {
                    MessageAlert.Alert("误差范围有误！");
                    return false;
                }
                if (max <= min)
                {
                    MessageAlert.Alert("设置扭矩范围有误！");
                    return false;
                }
                if (rmin > 0)
                {
                    MessageAlert.Alert("允许最小误差不能大于0！");
                    return false;
                }
            }
            catch 
            {
                MessageAlert.Alert ("请输入正确的数字！");
                return false;

            }
            return true;
        }

        private void bt_sub_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRight())
                return;
            if (isadd)
                _errorrangset = new errorrangset();
               saveErrorRang(_errorrangset);
        }

        private void editer_Click(object sender, RoutedEventArgs e)
        {
            if (this.dg_set.SelectedIndex < 0)
            {
                return;
            }
            _errorrangset = dg_set.SelectedItem as errorrangset;
            isadd = false;
             showerrorset(_errorrangset);
        }


        void showerrorset(errorrangset e)
        {
            if (e != null)
            {
                foreach (wrenchspecies s in cb_species.Items)
                {
                    if (s.guid == e.speciesID)
                    {
                        cb_species.SelectedItem = s;
                        break;
                    }
                }
                this.tb_errormax.Text = e.errorrangMax.ToString();
                this.tb_errormin.Text = e.errorrangMin.ToString();
                this.tb_max.Text = e.rangmax.ToString();
                this.tb_min.Text = e.rangmin.ToString();
            }
        }

        void saveErrorRang(errorrangset et)
        {
               errorrangset  er = new errorrangset();
               er = et;
            try
            {
                er.speciesID = this.cb_species.SelectedValue.ToString();
                er.speciesname = (this.cb_species.SelectedItem as wrenchspecies).speciesName;
                er.rangmax = Convert.ToDecimal(this.tb_max.Text.Trim());
                er.rangmin = Convert.ToDecimal(this.tb_min.Text.Trim());
                er.errorrangMax = Convert.ToDecimal(this.tb_errormax.Text.Trim());
                er.errorrangMin = Convert.ToDecimal(this.tb_errormin.Text.Trim());
                er.errorrang = _errorrangset.errorrangMin.ToString() + "~" + _errorrangset.errorrangMax.ToString();
                er.rangvalue = this.tb_min.Text.Trim() + "~" + this.tb_max.Text.Trim();
            }
            catch 
            {
                MessageAlert.Error("请填写正确的数值！"); return; 
            }
            if (!string.IsNullOrEmpty(er.guid))
            {
                if (errorset.FindIndex(p => p.guid == er.guid) >= 0)
                    errorset.RemoveAt(errorset.FindIndex(p => p.guid == er.guid));             
            }
            else 
            {
                er.guid = Guid.NewGuid().ToString();
            }

            List<errorrangset> esl = errorset.FindAll(p => p.speciesname == er.speciesname);
            foreach (errorrangset e in esl)
            {
                if ((e.rangmin < er.rangmax && er.rangmax <= e.rangmax) || (e.rangmin <= er.rangmin && er.rangmin < e.rangmax))
                {
                    MessageAlert.Alert("设置数值范围重合！");
                    return;
                }
                if ((er.rangmax >= e.rangmax) && (er.rangmin <= e.rangmin))
                {
                    MessageAlert.Alert("设置数值范围重合！");
                    return;
                }
            }

            errorset.Add(er);
            if (SerializeXML<errorrangset>.exit())
                SerializeXML<errorrangset>.del();
            SerializeXML<errorrangset>.SaveList(errorset);
            getErrorRanng();
            Clear();
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            if (this.dg_set.SelectedIndex < 0)
            {
                return;
            }

            errorrangset er = this.dg_set.SelectedItem as errorrangset;
            errorset.RemoveAt(errorset.FindIndex(p => p.guid == er.guid));
            if (SerializeXML<errorrangset>.exit())
                SerializeXML<errorrangset>.del();
            SerializeXML<errorrangset>.SaveList(errorset);
            getErrorRanng();

        }

        void Clear()
        {
            _errorrangset = null;
            isadd = true;
            this.tb_errormax.Clear();
            this.tb_errormin.Clear();
            this.tb_max.Clear();
            this.tb_min.Clear();
        }
        private void bt_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}
