using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
   public  interface ICheckTargetRecord
    {
        string AddReturnGuid(Model .torquecheckrecord torquecheckrecord);
        bool AddNotReturn(torquecheckrecord checkrecord);
        bool AddMany(List <Model.torquecheckrecord >listtorquecheckrecord);
        bool Del(torquecheckrecord checkrecord);
        List<Model.torquecheckrecord> SelectByCheckTargetID(string checktargetguid);
    }
}
