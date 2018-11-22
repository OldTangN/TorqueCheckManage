using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
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

namespace QDDL.Nlbs.Check
{
    /// <summary>
    /// Interaction logic for CheckdataDetail.xaml
    /// </summary>
    public partial class CheckdataDetail : Window
    {
        public static CheckdataDetail checkdatadetial = null;
        public static CheckdataDetail GetCheckdataDetail(WrenchCheckOut wrenchcheckout) 
        {
            if (checkdatadetial == null || !checkdatadetial.IsLoaded)
                return new CheckdataDetail(wrenchcheckout);
            return checkdatadetial ;                   
        }

        ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();
        private  CheckdataDetail(WrenchCheckOut wrenchcheckout)
        {
            InitializeComponent();
            datagridbind(getcheckoutdetail(wrenchcheckout));
        }
        List<CheckOutDetail> getcheckoutdetail(WrenchCheckOut wrenchcheckout)
        {
            List<CheckOutDetail> codlist = new List<CheckOutDetail>();
            if (wrenchcheckout == null)
                return codlist;
            List<torquecheckrecord> tempcheckrecorklist = new List<torquecheckrecord>();
            tempcheckrecorklist = CheckTargetRecord.SelectByCheckTargetID(wrenchcheckout.guid );
            if(tempcheckrecorklist==null)
            return codlist ;
            foreach (torquecheckrecord t in tempcheckrecorklist) {
                codlist.Add(new CheckOutDetail() {                 
                wrenchbarcode=wrenchcheckout.wrenchbarcode  ,
                torquetargetvalue = t.torqueTargetValue.ToString ("f2"),
                checkdata =t.analyserValue .ToString (),
                errorrang=(t.torqueTargetValue *(1+ Convert.ToDecimal (t.errorRangeMin))).ToString ("f2")+"~"+(t.torqueTargetValue *(1+ Convert.ToDecimal (t.errorRangeMax))).ToString ("f2"),
                checktime =t.checkTime .ToString ().Replace ('T',' '),
                iseffect =t.passedFlag
                });
            }
            return codlist;
        }
        void datagridbind(List<CheckOutDetail> list) 
        {
           
            this.dt_datadetail.ItemsSource = null;
            this.dt_datadetail.ItemsSource = list;
        }
        
    }
}
