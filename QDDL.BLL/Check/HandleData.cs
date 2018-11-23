using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using QDDL.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace QDDL.BLL.Check
{

    /// <summary>
    /// 重新重构功能太多
    /// </summary>
  public  class HandleData
    {
      wrench _wrench = new wrench();
      public List<ShowCheckresult> Checkdatashow { get; set; }
      public List<ShowCheckresult> _filecheckdatashow = new List<ShowCheckresult>();
      public List<ShowCheckresult> _successcheckdatashow = new List<ShowCheckresult>();
      //ShowCheckresult _checkdatashow = new ShowCheckresult();
       users _juser = new users();
       userinfo  _zuser = new userinfo ();
       systemcheckset _systemset = new systemcheckset ();
       decimal _setvalue;
       decimal  _errorrangmin = 0;
       decimal _errorrangmax = 0;
       bool _wrenchisgood = false;
       List<ShowCheckresult> success = new List<ShowCheckresult>();
       List<ShowCheckresult> fail = new List<ShowCheckresult>();
      // int confcount = Convert.ToInt32( OperationConfig.GetValue ("count"));
       ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
       ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();

       public HandleData(users juser, userinfo  zuser, wrench wrenchmodel,bool wrenchisgood,decimal setvalue=0,decimal  errorrangmin=0,decimal errorrangmax=0)
       {
           _wrench = wrenchmodel;
           //_checkdatashow = checkdatashow;
           _setvalue = setvalue;
           _errorrangmin = errorrangmin;
           _errorrangmax = errorrangmax;
           _systemset = GetSystem();
           _juser = juser;
           _zuser = zuser;
           _wrenchisgood = wrenchisgood;
      }

       systemcheckset GetSystem()
       {
           List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
           if (scsl != null && scsl.Count > 0)
               return scsl.FirstOrDefault();
           return new systemcheckset ();
       }

      public HandleData() { }
      /// <summary>
      /// 分开失败和成功数据
      /// </summary>
      /// <returns></returns>
     public  bool filterdata()
     {
          if (Checkdatashow.Count <= 0) return  false;
          foreach (ShowCheckresult cds in Checkdatashow)
          {

              if (cds.result.Equals("×"))
              {

                  foreach (ShowCheckresult cd in _successcheckdatashow)
                  {
                      _filecheckdatashow.Add(cd);
                  }
                  _successcheckdatashow.Clear();
                  _filecheckdatashow.Add(cds);
              }
              else
              {
                  _successcheckdatashow.Add(cds);
                 
              }
          }                        
          if (_successcheckdatashow.Count < (_systemset.count??5) )
          {
              foreach (ShowCheckresult cd in _successcheckdatashow)
              {
                  _filecheckdatashow.Add(cd);
              }
              _successcheckdatashow.Clear();
          }
          return true;
      }
      /// <summary>
      /// 从校验集合中挑选打印内容
      /// </summary>
      /// <returns></returns>
     public List<ShowCheckresult> Getprint()
     {
         GetSplit(Checkdatashow, ref  success, ref fail);
         List<ShowCheckresult> printlist = new List<ShowCheckresult>();
         //List<ShowCheckresult> cd = new List<ShowCheckresult>();
         List<string> faildata = GetTargetCheck(fail);
         List<string> successdata = GetTargetCheck(success);
        // List<string> faildata = new List<string>();
         foreach (ShowCheckresult s in success)
         {
             printlist.Add(s);
         }
         foreach (string s in successdata)
         {
             faildata.RemoveAll(p =>
             { 
                 if(p== s)
                 {
                     return true; 
                 }
                 else{return false ;}
             });
         }
         foreach (string s in faildata)
         {
             int i = 0;
             List<ShowCheckresult> sc = fail.FindAll(p=>p.setdata .ToString ("f2")==s);
             foreach (ShowCheckresult scr in sc)
             {
                 if (i >= _systemset.count)
                     break;
                 printlist.Add(scr);
                 i++;
             }
         }
         return printlist;
  }

      /// <summary>
      /// 列表中获取校验目标值列表
      /// </summary>
      /// <param name="checklist"></param>
      /// <returns></returns>
     List<string> GetTargetCheck(List<ShowCheckresult> checklist)
     {
         List<string> resultlist = new List<string>();
         foreach (ShowCheckresult s in checklist)
         {
             if (!resultlist.Contains(s.setdata.ToString("f2")))
                 resultlist.Add(s.setdata.ToString("f2"));
         }
         return resultlist;
     }


      /// <summary>
      /// 在列表中成功失败分开
      /// </summary>
      /// <param name="templist"></param>
      /// <param name="success"></param>
      /// <param name="fail"></param>
      /// <param name="count"></param>
     public void GetSplit(List<ShowCheckresult> templist, ref List<ShowCheckresult> success, ref List<ShowCheckresult> fail)
     {
         int count = _systemset.count??0;
         if (count <=0)
             return;

         for (int j = 0; j < templist.Count; j++)
         {
             int m = 1;
             List<ShowCheckresult> temp = new List<ShowCheckresult>();
             for (int i = 0; i < count; i++)
             {
                 if (j + i >= templist.Count)
                     break;
                 if (templist[j + i].result == "√")
                 {
                     temp.Add(templist[j + i]);
                     m++;
                 }

             }
             if (m <= count)
             {
                 fail.Add(templist[j]);
             }
             else
             {
                 foreach (ShowCheckresult t in temp)
                 {
                     success.Add(t);
                 }
                 j += count - 1;
             }
         }
     }


  public  bool  save() {
        //  if (!filterdata()) return false ;
          try
          {
              if (_zuser == null||_zuser .user==null )
              {
                  _zuser = new userinfo ();
                  _zuser.user = new users();
                  _zuser.user.cardID  = "空";
              }
              GetSplit(Checkdatashow, ref  success, ref fail);
              torquechecktarget tqct = new torquechecktarget() { wrenchID = _wrench.id.ToString(), checkDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), qaID = _zuser.user.cardID, operatorID = _juser.cardID, torqueTargetValue = _wrench.targetvalue, errorRangeMax = (_errorrangmax / 100).ToString ("f0"), errorRangeMin = (_errorrangmin / 100).ToString ("f0"), is_good = _wrenchisgood, guid = Guid.NewGuid().ToString() ,count =_systemset.count??0 ,arry =_systemset .arry ??0};
              string ttid = CheckTarget.AddReturnGuid(tqct);      
              List<torquecheckrecord> tc =  getresult(ttid); 
              return CheckTargetRecord.AddMany(tc);
          }
          catch {
              new Exception("保存时出现异常");
              return false; 
          }
  }
  List<torquecheckrecord> getresult(string ttid) 
  {
      List<torquecheckrecord> tc = new List<torquecheckrecord>();
      foreach (ShowCheckresult c in fail)
      {
          tc.Add(new torquecheckrecord() { TorqueCheckTargetID = ttid, analyserValue = Convert.ToDecimal(c.checkdata), torqueTargetValue = Convert.ToDecimal(c.setdata), checkTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), passedFlag = c.result.Equals("√") ? true : false, isEffective = false, guid = Guid.NewGuid().ToString(), errorRangeMax = c.normalmax, errorRangeMin = c.normalmin, isTurn = c.isturn });
      }
      foreach (ShowCheckresult c in success)
      {
          tc.Add(new torquecheckrecord() { TorqueCheckTargetID = ttid, analyserValue = Convert.ToDecimal(c.checkdata), torqueTargetValue = Convert.ToDecimal(c.setdata), checkTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), passedFlag = true, isEffective = true, guid = Guid.NewGuid().ToString(), errorRangeMax = c.normalmax, errorRangeMin = c.normalmin, isTurn = c.isturn });
      }
      return tc;
  }

    }
}
