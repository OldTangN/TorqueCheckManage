using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.BllModel
{
  public   class TorqueTestModel
    {
      public string testername { get; set;}
      public string portname { get; set;}
      public int databit { get; set;}
      public int baundrate { get; set;}
      public decimal minvalue { get; set; }
      public decimal maxvalue { get; set;}
    }
}
