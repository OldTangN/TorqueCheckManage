using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.Wrench
{

    public class GetWrenchInfo
    {
        IWrench Wrench = DataAccess.CreateWrench();
        IWrenchSpecies WrenchSpecies = DataAccess.CreateWrenchSpecies();
        IWrenchStatus WrenchStatus = DataAccess.CreateWrenchStatus();
        public wrenchinfo GetWrenchinfo(string barcode)
        {
            wrench w = Wrench.selectByBarcode(barcode);
            if (w == null)
                return null;
            wrenchspecies ws = WrenchSpecies.selectByGuid(w.species);
            wrenchstatus wss = WrenchStatus.selectByguid(w.status);
            wrenchinfo wi = new wrenchinfo();
            wi.wrench = w;
            wi.species = ws;
            wi.status = wss;
            return wi;
        }

    }
}
