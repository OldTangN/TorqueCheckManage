using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL
{
 public    interface IWrenchSpecies
    {
        bool add(wrenchspecies species);
        string addreturnid(wrenchspecies species);
        bool addmany(List <wrenchspecies > listwc);
        bool updata(wrenchspecies  species);
        List<wrenchspecies> select();
        List<wrenchspecies> selectbyname(string name);
     
        wrenchspecies selectByGuid(string guid);
        bool Del(wrenchspecies wrenchspecies);
    }
}
