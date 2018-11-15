using LT.Comm;
using LT.DAL.Service;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL.MySql
{
  public   class MySqlWrench:IWrench 
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");
        public bool add(Model.wrench wrenchtool)
        {
            return ServerHelp.addSingleInfoNotReturnID<wrench>(wrenchtool ,_webip );
        }
        public bool addlist(List<wrench> wrenchlist)
        {
            return ServerHelp.addManyInfoReturnBool<wrench>(wrenchlist ,_webip);
        }

        public bool updata(Model.wrench wrenchtool)
        {
            string tablename = typeof(wrench).Name;
            string contion = string.Format("guid='{0}'",wrenchtool.guid);
            string updatestr = string.Format(
                "wrenchCode='{0}',wrenchBarCode='{1}',rangeMin='{2}',rangeMax='{3}',factory='{4}',species_id='{5}',status_id='{6}',comment='{7}',targetvalue='{8}',unit='{9}',isallowcheck={10},cycletime={11},createDate='{12}' , targetvalue1='{13}',targetvalue2='{14}' , lastrepair='{15}'", wrenchtool.wrenchCode
                ,wrenchtool .wrenchBarCode ,wrenchtool.rangeMin ,wrenchtool .rangeMax ,wrenchtool .factory ,wrenchtool .species ,wrenchtool.status,wrenchtool .comment,wrenchtool.targetvalue ,wrenchtool .unit ,wrenchtool.isallowcheck ,wrenchtool .cycletime,wrenchtool.createDate ,wrenchtool.targetvalue1 ,wrenchtool.targetvalue2,wrenchtool .lastrepair  );
            return ServerHelp.updateByWhere(tablename ,_webip ,contion ,updatestr );
        }

        public List<wrench> select()
        {
            List<wrench> wrechlist = new List<wrench>();
            ServerHelp.findList<wrench>(out wrechlist,_webip );
            return wrechlist;
        }

        public Model.wrench selectByBarcode(string barcode)
        {
            try
            {
                string contion = string.Format("wrenchBarCode='{0}'", barcode);
                return ServerHelp.findDataByCondition<wrench>(contion, _webip).FirstOrDefault();
            }
            catch { return new wrench(); }
        }

        public List<Model.wrench> selectBycode(string code)
        {
            string contion = string.Format("wrenchCode='{0}'", code);
            return ServerHelp.findDataByCondition<wrench>(contion, _webip);
        }

        public List<Model.wrench> selectByid(int id)
        {
            string contion = string.Format("wrenchCode='{0}'", id);
            return ServerHelp.findDataByCondition<wrench>(contion, _webip);
        }

        public List<Model.wrench> selectBystatus(string status)
        {
            throw new NotImplementedException();
        }


        public List<wrench> selectByContion(Dictionary<string, string> dict)
        {
            string str = "";
            foreach (var  d in dict) { 
            str +=(string .Format ("{0}='{1}'"+" and ",d.Key ,d.Value ));
            }
         string contion=str.Count ()>4? str.Remove(str .Count()-4):"";
            return ServerHelp.findDataByCondition<wrench>(contion ,_webip );
        }


        public wrench selectByguid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid);
            return ServerHelp.findDataByCondition<wrench>(contion ,_webip ).FirstOrDefault ();
        }

        public List<wrench> SelectBarorcode(string contion)
        {
            string condition = string.Format("wrenchBarCode='{0}' or wrenchCode='{1}'", contion,contion);
            return ServerHelp.findDataByCondition<wrench>(condition, _webip);
        }

        public bool Del(wrench wrenchmodel)
        {
            string contion = string.Format("guid='{0}'", wrenchmodel.guid);
            return ServerHelp.deleteDataByWhere<wrench>(_webip, contion);
        }


        public List<wrench> selectPage(int page, int pageNo)
        {
            string condition = string.Format(" id <=(SELECT id FROM wrench ORDER BY id desc LIMIT {0}, {1}) ORDER BY id desc LIMIT {2}", (pageNo-1)*page,1, page);
            
            return ServerHelp.findDataByCondition<wrench>(condition, _webip);
        }


        public int SelectCount()
        {
            string condition = "";
            return ServerHelp.findListWhereCount(condition, _webip, typeof(wrench).Name); 

        
          //  return Convert.ToInt16(ServerHelp.findDataByContion<int>(condition, "", _webip, typeof(wrench).Name).FirstOrDefault());
        }
    }
}
