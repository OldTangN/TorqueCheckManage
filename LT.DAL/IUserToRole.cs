using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LT.DAL
{
    /// <summary>
    /// 逻辑控制保证角色和人员信息一一对应
    /// </summary>
  public   interface  IUserToRole
    {
      /// <summary>
      /// 查找所有
      /// </summary>
      /// <returns></returns>
        List<LT.Model.UserToRoleModel.usertorole> select();
     /// <summary>
     /// 添加
     /// </summary>
     /// <param name="usertorolemodel"></param>
     /// <returns></returns>
        bool add(LT.Model.UserToRoleModel.usertorole usertorolemodel);
      /// <summary>
      /// 根据user 的guid 查找
      /// </summary>
      /// <param name="userguid"></param>
      /// <returns></returns>
        List<LT.Model.UserToRoleModel.usertorole> selectbyuserid(string userguid);
      /// <summary>
      /// 根据角色的guid查找
      /// </summary>
      /// <param name="roleguid"></param>
      /// <returns></returns>
        List<LT.Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid);
      /// <summary>
        /// 根据角色的guid, user 的guid 查找
      /// </summary>
      /// <param name="roleguid"></param>
      /// <param name="userguid"></param>
      /// <returns></returns>
        List<LT.Model.UserToRoleModel.usertorole> selectbyroleid(string roleguid,string userguid);
      /// <summary>
      /// 更新
      /// </summary>
      /// <param name="usertorolemodel"></param>
      /// <returns></returns>
         bool update(LT.Model.UserToRoleModel.usertorole usertorolemodel);
         bool delete(LT.Model.UserToRoleModel.usertorole usertorolemodel);
 
    }
}
