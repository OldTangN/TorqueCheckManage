using QDDL.Comm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.Service
{
  public  class ServerHelp
    {
     

      public static bool updateByWhere<T>( string url, string guid)where T:class 
      {
          string uri = url + "updateDataByWhere/";
          string tableName = typeof(T).Name;
          Dictionary<string, object> dict2 = new Dictionary<string, object>();
          dict2.Add("tableName", tableName);
          dict2.Add("field", "is_useful = '0'");
          dict2.Add("condition", "guid = '" + guid + "'");
          return getFormSave(uri, dict2);
      }

  
      public static bool updateByWhere(string talbeName, string url, string condition, string updatesql)
      {
          string uri = url + "updateDataByWhere/";

          Dictionary<string, object> dict2 = new Dictionary<string, object>();
          dict2.Add("tableName", talbeName);
          dict2.Add("field", updatesql);
          dict2.Add("condition", condition);
          return getFormSave(uri, dict2);
      }

      public static bool deleteDataByWhere<T>(string url,string sql_condition)where T:class 
      {        
          string tableName = typeof(T).Name;
          string uri = url + "deleteDataByWhere/";
          Dictionary<string, object> dict2 = new Dictionary<string, object>();
          dict2.Add("tableName", tableName);
          dict2.Add("condition", sql_condition);
          return getFormSave(uri, dict2);
      }



      public static bool getFormSave(string uri, Dictionary<string, object> dict2)
      {
          string userAgent = "Someone";
          HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(uri, userAgent, dict2);
          StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
          string fullResponse = responseReader.ReadToEnd();
          bool successFlag;
          string val_first = HttpWebResponseUtility.getValueByKeyName(fullResponse, out successFlag);
          return successFlag;
      }
      public static  string str = "";
      public  static string urll="";
      public static int addSingleInfoReturnID<T>(T t,string url) where T : class
      {
          try
          {
              List<T> list = new List<T>();
              list.Add(t);
              string strSerialResult = JsonConvert.SerializeObject(list);//序列化结果   

              Type type = typeof(T);
              string className = type.Name;

             // string uri = Config.globalServiceURL + "service/addSingleDataReturnGUID/?tableName=" + className;
              string uri = url + "addSingleDataReturnGUID/?tableName=" + className;
              Dictionary<string, string> dict = new Dictionary<string, string>();
              dict.Add(className, strSerialResult);
              str = strSerialResult;
              urll=uri;
              string responseStr = HttpWebResponseUtility.PostWebResponse(uri, dict, null, null, Encoding.UTF8, null);
              string typeID = HttpWebResponseUtility.getValueByKeyName_return_id(responseStr, "data"); //reader.ReadToEnd为响应信息，这里为键值对，格式为 "data":5 
              return Convert.ToInt32(typeID);
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return -1;
          }
      }

      public static bool addSingleInfoNotReturnID<T>(T t,string url) where T : class
      {
          try
          {
              List<T> list = new List<T>();
              list.Add(t);
              string strSerialResult = JsonConvert.SerializeObject(list);//序列化结果    
              Type type = typeof(T);
              string className = type.Name;
             // string uri = Config.globalServiceURL + "service/addSingleDataNotReturn/?tableName=" + className;
              string uri = url + "addSingleDataNotReturn/?tableName=" + className;
              Dictionary<string, string> dict = new Dictionary<string, string>();
              dict.Add(className, strSerialResult);

              string responseStr = HttpWebResponseUtility.PostWebResponse(uri, dict, null, null, Encoding.UTF8, null);
               string flagStr = HttpWebResponseUtility.getValueByKeyName_not_return_guid(responseStr, "data"); //reader.ReadToEnd为响应信息，这里为键值对，格式为 "data":5 
              return flagStr.Equals("success");
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return false;
          }
      }

      /// <summary>
      /// 没用
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="id"></param>
      /// <param name="updateT"></param>
      /// <param name="url"></param>
      /// <returns></returns>
      public static bool updateSingleInfo<T>(int id, T updateT,string url) where T : class
      {
          try
          {
              Type type = typeof(T);
              string className = type.Name;
              string strSerialResult = JsonConvert.SerializeObject(updateT);//序列化结果   

              //string uri = Config.globalServiceURL + "service/updateSingleInfoByTableName/?tableName=" + className + "&id=" + id;
              string uri = url + "updateSingleInfoByTableName/?tableName=" + className + "&id=" + id;
              Dictionary<string, string> dict = new Dictionary<string, string>();
              dict.Add(className, strSerialResult);

              string responseStr = HttpWebResponseUtility.PostWebResponse(uri, dict, null, null, Encoding.UTF8, null);
              string flagStr = HttpWebResponseUtility.getValueByKeyName(responseStr, "data"); //reader.ReadToEnd为响应信息，这里为键值对，格式为 "data":5 


              if (!flagStr.Equals("success"))
              {
                  return false;
              }
              else
              {
                  return true;
              }
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return false;
          }
      }
      public static bool addManyInfoReturnBool<T>(List<T> list,string url) where T : class
      {
          try
          {
              string strSerialResult = JsonConvert.SerializeObject(list);//序列化结果    
              Type type = typeof(T);
              string className = type.Name;

             // string uri_add = Config.globalServiceURL + "service/addManyData/?tableName=" + className;
              string uri_add = url + "addManyData/?tableName=" + className;
              Dictionary<string, string> dict_add = new Dictionary<string, string>();
              dict_add.Add(className, strSerialResult);
              string responseStr_add = HttpWebResponseUtility.PostWebResponse(uri_add, dict_add, null, null, Encoding.UTF8, null);
             string flag_add = HttpWebResponseUtility.getValueByKeyName(responseStr_add, "data");
              return flag_add.Equals("success");
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return false;
          }
      }



      public static bool findList<T>(out List<T> list_get,string url) where T : class
      {
          try
          {
              List<T> list = new List<T>();
              Type type = typeof(T);
              string className = type.Name;

             // string return_tr = HttpWebResponseUtility.GetWebRequest(Config.globalServiceURL + "service/findListByTableName/?tableName=" + className, Encoding.UTF8);
              string uri=url+ "findListByTableName/?tableName=" + className;
              string return_tr = HttpWebResponseUtility.GetWebRequest(uri, Encoding.UTF8);
              bool successFlag;
              string val_first = HttpWebResponseUtility.getValueByKeyName(return_tr, out successFlag);
              if (successFlag == true)
              {
                  list = (List<T>)JsonConvert.DeserializeObject<List<T>>(val_first);
                  list_get = list;
                  return true;
              }
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
          }

          list_get = null;
          return false;
      }
      public static List<T> findDataByContion<T>(string field, string condition, string url,string tableName)
      {
          try
          {
              //Type type = typeof(T);
              string className = tableName;
              List<T> list = new List<T>();
              Dictionary<string, object> dict2 = new Dictionary<string, object>();
              dict2.Add("tableName", className);
              dict2.Add("field", field);
              dict2.Add("condition", condition);

              // string postURL = Config.globalServiceURL + "service/findListByWhere/";
              string postURL = url + "findDataByWhere/?tableName=" + className;
              string userAgent = "Someone";
              HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, dict2);

              StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
              string fullResponse = responseReader.ReadToEnd();
              bool successFlag;
              string val_first = HttpWebResponseUtility.getValueByKeyName(fullResponse, out successFlag);
              if (successFlag == true)
              {
                   list = (List<T>)JsonConvert.DeserializeObject<List<T>>(val_first);
              }
              return list;

          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return null;
          }
      }

      public static int findListWhereCount(string condition, string url, string tableName)
      {
          try
          {
              //Type type = typeof(T);
              string className = tableName;
              // List<T> list = new List<T>();
              Dictionary<string, object> dict2 = new Dictionary<string, object>();
              dict2.Add("tableName", className);
              dict2.Add("field", "*");
              // dict2.Add("field", field);
              if(!string.IsNullOrEmpty(condition))
              dict2.Add("condition", condition);
              // string postURL = Config.globalServiceURL + "service/findListByWhere/";
              string postURL = url + "findListWhereCount/?tableName=" + className;
              string userAgent = "Someone";
              HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, dict2);
              StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
              string fullResponse = responseReader.ReadToEnd();
              bool successFlag;
              string val_first = HttpWebResponseUtility.getValueByKeyName(fullResponse, out successFlag);
              if (successFlag == true)
              {
                  return Convert.ToInt32(val_first);
              }
              return 0;
          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return 0;
          }
      }


      public static List<T> findDataByCondition<T>(string condition,string url)
      {
          try
          {
              Type type = typeof(T);
              string className = type.Name;
              List<T> list = new List<T>();

              Dictionary<string, object> dict2 = new Dictionary<string, object>();
              dict2.Add("tableName", className);
              dict2.Add("field", "*");
              dict2.Add("condition", condition);

             // string postURL = Config.globalServiceURL + "service/findListByWhere/";
              string postURL = url + "findDataByWhere/?tableName="+className ;
              string userAgent = "Someone";
              HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, dict2);

              StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
              string fullResponse = responseReader.ReadToEnd();
              bool successFlag;
              string val_first = HttpWebResponseUtility.getValueByKeyName(fullResponse, out successFlag);
                if (successFlag == true)
              {
                  list = (List<T>)JsonConvert.DeserializeObject<List<T>>(val_first);
              }
              return list;

          }
          catch (Exception ce)
          {
              LogUtil.WriteLog(null, ce.Message);
              return null;
          }
      }
        
    }
}
