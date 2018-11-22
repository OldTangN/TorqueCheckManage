using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Model.BllModel
{
  public   class WrenchCheckOut
    {

        public string wrenchcode { get; set; }
        public string wrenchbarcode { get; set; }
        public string jusername { get; set; }
        public string zusername { get; set; }
        public userinfo juserinfo { get; set;}
        public userinfo zuserinfo { get; set; }
        public string torquetargetvalue { get; set; }
        public string errorrange { get; set; }
        public string errormin { get; set; }
        public string errormax { get; set; }
        public bool is_good { get; set; }
        public string checkdate { get; set; }
        public string guid { get; set; }     
    }

  public class CheckOutDetail {
      public string wrenchbarcode { get; set; }
      public string checkdata { get; set; }
      public string checktime { get; set; }
      public bool iseffect { get; set; }
      public string torquetargetvalue { get; set; }
      public string errorrang { get; set; }
  }
}
