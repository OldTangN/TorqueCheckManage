using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.ICCard
{
    public class CardEventArgs : EventArgs
    {
        public string data { set; get; }

        public CardEventArgs(string _data) : base()
        {
            this.data = _data;
        }
    }
}
