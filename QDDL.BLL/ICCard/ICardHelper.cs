using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.ICCard
{
    public interface ICardHelper
    {


         

        event EventHandler<CardEventArgs> HandDataBack;

        bool IsOpen();

        void Read();

        void Close();

    }
}
