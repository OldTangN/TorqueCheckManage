using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Comm
{
 public static  class SerializeXML<T>
    {
     public static void SaveList(List<T>list)
     {
         try
         {
             System.Xml.Serialization.XmlSerializer write = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));

             using (System.IO.StreamWriter sw = new System.IO.StreamWriter(typeof(T).Name))
             {
                 write.Serialize(sw, list);
             }
         }
         catch
         {
             LogUtil.WriteLog(typeof(T), "写入xml文件时失败！");
         }
     }
     public static List <T> Getlist() 
     {
         try
         {
             System.Xml.Serialization.XmlSerializer read = new System.Xml.Serialization.XmlSerializer(typeof(List<T>));
             using (System.IO.StreamReader sr = new System.IO.StreamReader(typeof(T).Name))
             {
                 return (List<T>)read.Deserialize(sr);
             }
         }
         catch
         {
             LogUtil.WriteLog(typeof(T), "读取xml文件时失败！");
             return null;
         }
     }

     public static bool exit()
     {
         string s = (typeof(T).Name);
         return Directory.Exists(s);
     }
     public static void  del()
     {
         try {

             Directory.Delete((typeof(T).Name));
         }
         catch (Exception ep)
         {
             MessageAlert.Error("出错："+"删除失败" );
             LogUtil.WriteLog(typeof(T), "删除"+typeof(T).Name+"文件时失败！");
         }
        
     }
    }
   
}
