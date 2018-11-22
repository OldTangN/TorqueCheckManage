using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDDL.BLL.Check
{
    public class FilterData
    {
        string id = "";
        string cardid = "fail:";
        public string CardId
        {
            get
            {
                return cardid;
            }
            set { cardid = value; }
        }
        public CheckDatashow CheckData { get; set; }
        CheckDatashow _resultcheckdata = new CheckDatashow();
        public  void resetCard()
        {
            id = "";
            if (!string.IsNullOrEmpty(CardId))
            {
                string[] s = CardId.Split(' ');
              for(int i=s.Length-1 ;i>=1 ;i--)
              {
                  id += s[i].ToString ();
              }
              id = Convert.ToInt32(id,16).ToString ();
            }
        }


        public string  getcardid()
        {
            return id;
        }
        public string resetid(string s) 
        {
            return id = s;
        }
        private bool filterchekdata()
        {
            try
            {
                if (CheckData == null) { return false; }
                double throwdata = Math.Abs((CheckData.targetvalue - CheckData.checkvalue) / CheckData.targetvalue);
                if (throwdata > (CheckData.rate * 10))
                    return false;
                return true;
            }
            catch { return false; }
        }

    public  CheckDatashow  returncheck()
        {
            if (filterchekdata())
            {
                CheckData.resultrate = Math.Abs((CheckData.targetvalue - CheckData.checkvalue)) / CheckData.targetvalue;
                if (CheckData.resultrate < CheckData.rate) { CheckData.isgood = "√"; } else { CheckData.isgood = "×"; }
             
            }
            return CheckData;
        }
    }

}