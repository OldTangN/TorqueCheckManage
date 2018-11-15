using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DAL
{
   public  interface IWrench
    {
       bool add(wrench wrenchtool );
       bool addlist(List<wrench> wrenchlist);
       bool updata(wrench wrenchtool);
       bool Del(wrench wrenchmodel);
       List <wrench> select();
       List<wrench> SelectBarorcode(string contion);
       wrench  selectByguid(string guid);
       List<wrench> selectByContion(Dictionary <string ,string > dict);
     
        wrench  selectByBarcode(string barcode);
       List<wrench> selectBycode(string code);
       List<wrench> selectByid(int id);
       List<wrench> selectBystatus(string status);
       /// <summary>
       /// 分页
       /// </summary>
       /// <param name="page">每页条数</param>
       /// <param name="pageNo">页码</param>
       /// <returns></returns>
       List<wrench> selectPage(int page,int pageNo);
       int SelectCount();
    
    }
}
