using LT.Comm;
using LT.DAL;
using LT.DAL.MySql;
using LT.Model;
using LT.Model.BllModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.BLL.Check
{
 public  class CheckDataOutExcel
    {
     ICheckTargetRecord CheckTargetRecord = DataAccess.CreateCheckTargetRecord();
     IWrench Wrench = DataAccess.CreateWrench();
     IUser User = DataAccess.CreateUser();
     DataTable dt = new DataTable();
     List<WrenchCheckOut> _wrenchchecklist = new List<WrenchCheckOut>();
     public  CheckDataOutExcel()
     {
      
     }
     public DataTable Header(List<WrenchCheckOut> wcl) 
     {
         DataTable dt = new DataTable();
         List<string> headlist = new List<string>() {"序 号", "扳手编号","扳手量程(N.m)","校验结果","设定值(N.m)","单次结果","校验员","质检员","校验时间"};
         dt.Columns.Add(headlist[0]);
         dt.Columns.Add(headlist[1]);
         dt.Columns.Add(headlist[2]);
         dt.Columns.Add(headlist[3]);
         dt.Columns.Add(headlist[4]);
         int tempc = CheckdetailCount();
         for (int i = 0; i < tempc; i++)
         {
             dt.Columns.Add("校验值" + (i + 1).ToString() + "(N.m)");
         }
         dt.Columns.Add(headlist[5]);
         dt.Columns.Add(headlist[6]);
         dt.Columns.Add(headlist[7]);
         dt.Columns.Add(headlist[8]);
         return dt;
     }

     public DataTable  ToTable(List<WrenchCheckOut> wco)
     {

         DataTable dt = Header(wco);
         int tempc = CheckdetailCount(wco);
         List<CheckOutDetail> datadetaillist = new List<CheckOutDetail>();
         //object[] values = new object[8 + tempc];
         int count = 0;
         foreach (WrenchCheckOut w in wco)
         {
             object[] values = new object[8 + tempc];
             count++;
             wrench wc = Wrench.selectByBarcode(w.wrenchbarcode);
             values[0] = count.ToString();
             values[1] = w.wrenchcode.ToString();
             if (wc == null || wc.guid == null)
             {
                 values[2] = "";
             }
             else 
             {
                 values[2] = wc.rangeMin.ToString("f2") + "~" + wc.rangeMax.ToString("f2");
             }  
             values [3]=w.torquetargetvalue .ToString ();
             datadetaillist = GetDetail(w);
             if (datadetaillist == null || datadetaillist.Count <= 0)
                 continue;
             for (int i = 0; i < datadetaillist.Count(); i++)
             {
                 values[i+4] = datadetaillist[i].checkdata.ToString();
             }
             values[tempc + 4] = w.is_good ? "合格" : "不合格";
             values[tempc + 5] = w.jusername;
             values[tempc + 6] = w.zusername;
             values[tempc + 7] = w.checkdate+"\t";
             dt.Rows.Add(values);
         }
         return dt;
     }

     public int CheckdetailCount()
     {
         int i = 5;
         List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
         if (scsl != null || scsl.Count > 0)
         {
             i = scsl.FirstOrDefault().count ?? 5;
         }
         return i;
     }

     public int CheckdetailCount(List<WrenchCheckOut> wrenchchecklist) 
     {
         int count = 0;
         if (wrenchchecklist.Count <= 0)
             return count;
         foreach (WrenchCheckOut wco in wrenchchecklist)
         {
             List<CheckOutDetail> lc = GetDetail(wco);
             if(lc!=null&&lc.Count>0)
             count = GetDetail(wco).Count > count ? GetDetail(wco).Count : count;
         }
         return count;
     }



     public List<CheckOutDetail> GetDetail(WrenchCheckOut wrenchcheckout) 
     {                   
            List<CheckOutDetail> codlist = new List<CheckOutDetail>();
            if (wrenchcheckout == null)
                return codlist=null;
            List<torquecheckrecord> tempcheckrecorklist = new List<torquecheckrecord>();
            tempcheckrecorklist = CheckTargetRecord.SelectByCheckTargetID(wrenchcheckout.guid);
            if (tempcheckrecorklist == null||tempcheckrecorklist .Count <=0)
                return codlist=null;
            if (tempcheckrecorklist.FindIndex(p => p.isEffective == true) < 0)
            {
                int i = 0,j=0;
                List<systemcheckset> scsl = SerializeXML<systemcheckset>.Getlist();
                if (scsl == null || scsl.Count <= 0)
                {
                    i = 5;
                }
                else
                {
                    i =Convert .ToInt16 (scsl.First().count) ;
                }
                foreach (torquecheckrecord t in tempcheckrecorklist)
                {
                    if (j >= i)
                        break;
                    codlist.Add(new CheckOutDetail()
                    {
                        wrenchbarcode = wrenchcheckout.wrenchbarcode,
                        checkdata = t.analyserValue.ToString(),
                        checktime = t.checkTime.ToString(),
                        iseffect = t.passedFlag
                    });
                    j++;
                }
              
            }
            else 
            {
                foreach (torquecheckrecord t in tempcheckrecorklist)
                {
                    if (t.isEffective)
                    {
                        codlist.Add(new CheckOutDetail()
                        {
                            wrenchbarcode = wrenchcheckout.wrenchbarcode,
                            checkdata = t.analyserValue.ToString(),
                            checktime = t.checkTime.ToString(),
                            iseffect = t.passedFlag
                        });
                    }
                }
            }
     
            return codlist;
      }
     
    }


 public class OutExcelHeader
 { 
 
 }
}
