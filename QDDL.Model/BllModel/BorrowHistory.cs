using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.Model.BllModel
{
  public class BorrowHistory
    {
      public string wrenchbarcode { get; set; }
      public string wrenchcode { get; set; }
      public string borrowusercard { get; set; }
      public string borrowusername { get; set; }
      public string factory { get; set; }
      public string rang { get; set; }
      public string borrowdate { get; set; }
      public string borrowoperator { get; set; }
      public string returnuser { get; set; }
      public string returnoperator { get;set;}
      public string returndate { get; set; }
      public string wrenchcommon { get; set; }
      public bool isreturn { get;set;}

    }
}
