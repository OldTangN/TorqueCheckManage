using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.Plc
{
    /// <summary>
    /// 获取校验码
    /// </summary>
    public class HexUtil
    {
        public static byte[] SendStrMake(string dataStr)//重载上面的方法，生成和验证码
        {
            int sumData = 0;
            for (int j = 0; j < dataStr.Length; j += 2)
            {
                sumData += byte.Parse(dataStr.Substring(j, 2), System.Globalization.NumberStyles.HexNumber);
            }
            sumData = ~sumData + 0x01;
            string prelrc = sumData.ToString("X");
            string lrc = prelrc.Substring(prelrc.Length - 2, 2);
            string newStr = ":" + dataStr + lrc;
            return StrToHex(newStr);
        }

        public static byte[] StrToHex(string mStr) //返回处理后的十六进制字符串
        {
            string convertStr = BitConverter.ToString(ASCIIEncoding.Default.GetBytes(mStr)).Replace("-", " ");
            convertStr += " 0D 0A";
            string[] arr = convertStr.Split(' ');
            byte[] t = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                t[i] = (byte)Convert.ToInt32(arr[i], 16);
            }

            return t;
        }
    }
}
