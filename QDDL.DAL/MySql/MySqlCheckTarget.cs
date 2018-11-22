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
  public   class MySqlCheckTarget:ICheckTarget 
    {
      string _webip = OperationConfig.GetValue("WebServiceIp");
        public string AddReturnGuid(torquechecktarget checktarget)
        {

         return    ServerHelp.addSingleInfoReturnID  <torquechecktarget>(checktarget ,_webip).ToString ();
        }

        public bool Add(List<Model.torquechecktarget> listtorquechecktarget)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.torquechecktarget torquechecktarget)
        {
           string tableName=typeof (torquechecktarget ).Name ;
           string contion = string.Format("guid='{0}'",torquechecktarget .guid );
           string sql = string.Format("wrenchID_id='{0}',checkDate='{1}',qaID='{2}',operatorID='{3}',torqueTargetValue='{4}',errorRangeMax='{5}',is_good='{6}',comment='{7}',errorRangeMin='{8}'",torquechecktarget .wrenchID ,Convert .ToDateTime ( torquechecktarget.checkDate),torquechecktarget .qaID ,torquechecktarget .operatorID ,torquechecktarget .torqueTargetValue ,torquechecktarget.errorRangeMax  ,torquechecktarget .is_good?1:0 ,torquechecktarget .comment ,torquechecktarget.errorRangeMin);

           return ServerHelp.updateByWhere(tableName ,_webip ,contion ,sql);
        }


        public Model.torquechecktarget SelectByGuid(string guid)
        {
            string contion = string.Format("guid='{0}'",guid );
            return ServerHelp.findDataByCondition<torquechecktarget >(contion,_webip).FirstOrDefault ();
        }
    

public List <torquechecktarget> SelectByContion(Dictionary<string,string> dict)
{
    string temp = "";
    foreach (var  d in dict ){
        if (d.Key == "starttime") {
            temp += string.Format("checkDate>='{0}' and ",d.Value );
            continue ;
        }
        if (d.Key == "endtime") {
            temp += string.Format("checkDate<'{0}' and ",d.Value );
            continue;
        }
        temp += string.Format("{0}='{1}' and ",d.Key ,d.Value );


    }

    string contion =temp.Count ()>4? temp.Remove(temp .Count()-4):"";
    return ServerHelp.findDataByCondition<torquechecktarget>(contion, _webip);
}


public List <torquechecktarget > SelectByDate(DateTime first,DateTime last, string wrenchID)
{
    //DateTime start=dt;
    //DateTime last=dt.AddDays (1).AddMilliseconds (-1);
    string condition = string.Format("checkDate>='{0}' and checkDate<='{1}' and wrenchID_id='{2}'",first ,last, wrenchID);
    return ServerHelp.findDataByCondition<torquechecktarget>(condition,_webip);
}



public bool Del(torquechecktarget torquechecktarget)
{
    string contion = string.Format("id='{0}'", torquechecktarget.id);
    return ServerHelp.deleteDataByWhere<torquechecktarget>(_webip, contion);
}


public List<torquechecktarget> SelectByWrench(string wrenchguid)
{
    string condition = string.Format("wrenchID_id='{0}'", wrenchguid);
    return ServerHelp.findDataByCondition<torquechecktarget>(condition, _webip);
}


public List<torquechecktarget> SelectByContion(Dictionary<string, string> dict, int pagesize, int pageno)
{
    string temp = "";
    foreach (var d in dict)
    {
        if (d.Key == "starttime")
        {
            temp += string.Format("checkDate>='{0}' and ", d.Value);
            continue;
        }
        if (d.Key == "endtime")
        {
            temp += string.Format("checkDate<'{0}' and ", d.Value);
            continue;
        }
        temp += string.Format("{0}='{1}' and ", d.Key, d.Value);
    }
    string contion = temp.Count() > 4 ? temp.Remove(temp.Count() - 4) : "";
    contion += string.Format("LIMIT {0},{1}", (pageno * pagesize), pagesize);
    return ServerHelp.findDataByCondition<torquechecktarget>(contion, _webip);
}

public int SelectCount(Dictionary<string, string> dict)
{
    try
    {        
        string temp = "";
        foreach (var d in dict)
        {
            if (d.Key == "starttime")
            {
                temp += string.Format("checkDate>='{0}' and ", d.Value);
                continue;
            }
            if (d.Key == "endtime")
            {
                temp += string.Format("checkDate<'{0}' and ", d.Value);
                continue;
            }
            temp += string.Format("{0}='{1}' and ", d.Key, d.Value);
        }
        string contion = temp.Count() > 4 ? temp.Remove(temp.Count() - 4) : "";
        return ServerHelp.findListWhereCount(contion, _webip, typeof(torquechecktarget).Name);   
    }
    catch { return 0; }
}
    }
}
