using QDDL.Comm;
using QDDL.DAL.Service;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.MySql
{
  public   class MySqlWrenchSpecies:IWrenchSpecies
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");

        public bool add(wrenchspecies species)
        {
           return  ServerHelp.addSingleInfoNotReturnID <wrenchspecies>(species ,_webip );
        }

        public bool updata(Model.wrenchspecies species)
        {
            string tableName=typeof (wrenchspecies ).Name ;
            string sql=string .Format ("speciesName='{0}',speciesCode='{1}',parentSpecies='{3}',comment='{2}'",species .speciesName ,species .speciesCode ,species .comment,species.parentSpecies);
         string contion=string .Format ("guid='{0}'",species .guid );
         return ServerHelp.updateByWhere(tableName, _webip, contion, sql);
        }

        public List<wrenchspecies> select()
        { 
            List <wrenchspecies > wp=new List<wrenchspecies> ();
          ServerHelp.findList <wrenchspecies >( out wp,_webip );
          return wp;
        }


        public bool addmany(List<wrenchspecies> listwc)
        {
            return ServerHelp.addManyInfoReturnBool<wrenchspecies>(listwc ,_webip );
        }


        public List<wrenchspecies> selectbyname(string name)
        {
            string contion = string.Format("speciesName='{0}'",name );
            return ServerHelp.findDataByCondition<wrenchspecies>(contion ,_webip );
        }


        public string addreturnid(wrenchspecies species)
        {
            return ServerHelp.addSingleInfoReturnID<wrenchspecies>(species,_webip ).ToString ();
        }


        public wrenchspecies selectByGuid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid );
         return ServerHelp.findDataByCondition<wrenchspecies>(contion,_webip ).FirstOrDefault ();
        }
        public bool Del(wrenchspecies wrenchspecies)
        {
            string contion = string.Format("id='{0}'", wrenchspecies.id);
            return ServerHelp.deleteDataByWhere<wrenchspecies>(_webip, contion);
        }
    }
}
