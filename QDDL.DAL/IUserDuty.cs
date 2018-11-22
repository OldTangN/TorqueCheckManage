using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL
{
  public   interface IUserDuty
    {

      /// <summary>
      /// 添加职务
      /// </summary>
      /// <param name="duty"></param>
      /// <returns></returns>
      bool Add(duties duty);
      /// <summary>
      /// 查询所有职务
      /// </summary>
      /// <returns></returns>
      List<duties> Select();
      /// <summary>
      /// 按名称查询职务
      /// </summary>
      /// <param name="name"></param>
      /// <returns></returns>
    List <duties>SelectByName(string  name);
      /// <summary>
      /// 
      /// </summary>
      /// <param name="departmentguid"></param>
      /// <returns></returns>
    List<duties> SelectByDepartment(string departmentguid);
      /// <summary>
      /// 按职务guid查询
      /// </summary>
      /// <param name="guid"></param>
      /// <returns></returns>
    duties SelectByGuid(string guid);
      /// <summary>
      /// 更新职务
      /// </summary>
      /// <param name="duty"></param>
      /// <returns></returns>
      bool Update(duties duty);
      bool Del(duties duty);
    }
}
