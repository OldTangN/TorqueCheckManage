using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.Model.BllModel
{
  public   class BorrowReportInfo
    {
      public string wrenchbarcode { get; set; }
      public string borrowuser { get; set; }
      public string borrowoper { get; set; }
      public string borrowtime { get; set; }
      public string returnuser { get; set; }
      public string returnoper { get; set; }
      public string returntime { get; set; }
      public string common { get; set; }

  }
}
