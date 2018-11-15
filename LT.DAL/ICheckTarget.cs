using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
   public  interface ICheckTarget
    {
       string AddReturnGuid(Model.torquechecktarget torquechecktarget);
       bool Add(List <Model.torquechecktarget >listtorquechecktarget);
       bool Update(Model .torquechecktarget torquechecktarget);
       bool Del(Model .torquechecktarget torquechecktarget);
       Model.torquechecktarget  SelectByGuid(string guid);
       List<Model.torquechecktarget> SelectByWrench(string wrenchguid);
       List <Model.torquechecktarget> SelectByDate(DateTime first, DateTime last,string wrenchID);
       List<Model.torquechecktarget> SelectByContion(Dictionary<string, string> dict);
       List<torquechecktarget> SelectByContion(Dictionary<string, string> dict, int pagesize, int pageno);
       int SelectCount(Dictionary<string, string> dict);
    }
}
