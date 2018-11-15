using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongTie.Nlbs.Control.EncoderControl
{
  public   class EncoderConvert
    {
      const double  Circle = 1024.0;
      const double  Degree = 360.0;
      /// <summary>
      /// 度数到16进制指令
      /// </summary>
      /// <param name="degree"></param>
      /// <returns></returns>
      public string DegreeToString(int degree)
      {
          try
          {
              return (Convert .ToInt32 ((degree / Degree) * Circle)).ToString("x4").ToUpper();
          }
          catch
          {
              return "";
          }
      }
      /// <summary>
      /// plc字符串到度数
      /// </summary>
      /// <param name="count"></param>
      /// <returns></returns>
      public string StringToDegree(string   count)
      {
          try
          {
              int temp = Convert.ToInt32(count, 16);
              return (temp / Circle * Degree).ToString("f0");
          }
          catch
          {
              return "";
          }
      }
    }
}
