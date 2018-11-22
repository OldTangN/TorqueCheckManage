using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.BLL.MyChart
{
  public   class ChartData
    {
    }

    public class YearCheckData{
    
        DateTime _datetime = new DateTime();
        string _barcode = null;
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        public List<string> datelist = new List<string>();
        IWrench Wrench = DataAccess.CreateWrench();

        public YearCheckData(DateTime dt, string wrenchbarcode)
        {
            _datetime = dt;
            _barcode = wrenchbarcode;
        }
        public List<string> GetList() 
        {
            List<string> listcount = new List<string>();
             wrench w = Wrench.selectByBarcode(_barcode);
             if (w == null || w.guid == null)
                 return listcount;
            for (int i = 1; i <=12; i++) {
                string s = _datetime.Year.ToString()+ "-"+ string.Format("{0:00}",i ) ;
                DateTime start =Convert .ToDateTime (s);
                DateTime lasts = start.AddMonths(1).AddMilliseconds(-1);
                List<torquechecktarget> tl = CheckTarget.SelectByDate(start, lasts, w.guid);
                if (tl != null)
                    listcount.Add(tl.Count.ToString());
                else
                    listcount.Add("0");
                datelist.Add(start.ToString("yyyy-MM"));
            }
            return listcount;
        }

    }

  public class WeekCheckData 
  {
      DateTime _datetime = new DateTime();
      string _barcode = null;
      ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
      IWrench Wrench = DataAccess.CreateWrench();

      public List<string> datelist = new List<string>();
      public WeekCheckData(DateTime dt,string wrenchbarcode)     
      {
          _datetime = dt;
          _barcode = wrenchbarcode;
      }

      public List<string> GetList() {
          List<string> listcount = new List<string>();
          DateTime first = new DateTime();
          DateTime last = new DateTime();
          if (GetDaysOfWeeks(Convert.ToInt32(_datetime.Year), getweek(_datetime), out first, out last))
          {
              wrench w=Wrench.selectByBarcode (_barcode);
              if(w!=null&&w.guid !=null){

              for (int i = 0; i < 7; i++) {
                  DateTime dt = first.AddDays(i);
                  
                   DateTime start=dt;
                   DateTime lasts=dt.AddDays (1).AddMilliseconds (-1);
                  List<torquechecktarget> tl = CheckTarget.SelectByDate(start,lasts, w.guid);
                  if (tl != null)
                      listcount.Add(tl.Count.ToString());
                  else
                      listcount.Add("0");
                  datelist.Add(dt.ToString ("yyyy-MM-dd"));
              }

              }
          }
          return listcount;
      }

      int getweek(DateTime time) {
          GregorianCalendar gc = new GregorianCalendar();
          int weekOfYear = gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
          return weekOfYear;
      }
       static bool GetDaysOfWeeks(int year, int index, out DateTime first, out DateTime last)
      {
          first = DateTime.MinValue;
          last = DateTime.MinValue;
          if (year < 1700 || year > 9999)
          {
              //"年份超限"        
              return false;
          }
          if (index < 1 || index > 53)
          {
              //"周数错误"
              return false;
          }
          DateTime startDay = new DateTime(year, 1, 1);  //该年第一天
          DateTime endDay = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
          int dayOfWeek = 0;
          if (Convert.ToInt32(startDay.DayOfWeek.ToString("d")) > 0)
              dayOfWeek = Convert.ToInt32(startDay.DayOfWeek.ToString("d"));  //该年第一天为星期几
          if (dayOfWeek == 7) { dayOfWeek = 0; }
          if (index == 1)
          {
              first = startDay;
              if (dayOfWeek == 6)
              {
                  last = first;
              }
              else
              {
                  last = startDay.AddDays((6 - dayOfWeek));
              }
          }
          else
          {
              first = startDay.AddDays((7 - dayOfWeek) + (index - 2) * 7); //index周的起始日期
              last = first.AddDays(6);
              if (last > endDay)
              {
                  last = endDay;
              }
          }
          if (first > endDay)  //startDayOfWeeks不在该年范围内
          {
              //"输入周数大于本年最大周数";
              return false;
          }
          return true;
      }

  }
}
