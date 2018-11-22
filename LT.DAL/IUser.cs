using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LT.DAL
{
    public interface IUser
    {
        /// <summary>
        /// 查找所有
        /// </summary>
        /// <returns></returns>
        List<Model.users> Select();
        /// <summary>
        ///根据cardid查找
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        Model.users Select(string CardId);
        /// <summary>
        /// 根据卡号和员工的编号查找
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="empID"></param>
        /// <returns></returns>
        List<users> SelectByCBcode(string cardid, string empID);
        /// <summary>
        /// 根据员工guid查找
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Model.users SelectByguid(string guid);
        /// <summary>
        /// 根据员工编号查找
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        List<Model.users> SelectByCode(string empID);
        /// <summary>
        /// 根据员工姓名和密码查找
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Model.users Select(string name, string password);
        /// <summary>
        /// 跟据员工的卡号和密码查找
        /// </summary>
        /// <param name="cardid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        List<Model.users> SelectByCard(string cardid, string password);
        /// <summary>
        /// 根据姓名查找
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        List<Model.users> SelectByName(string Name);
        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="contion">字段名称，字段的值</param>
        /// <returns></returns>
        List<Model.users> SelectByContion(Dictionary<string, string> contion);
        List<Model.users> SelectNameOrCardid(string contion);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">员工信息，名字不能重复</param>
        /// <returns></returns>
        bool Add(Model.users user);
        /// <summary>
        /// 添加员工信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns>返回值是id</returns>
        string addreturnid(Model.users user);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="userlist"></param>
        /// <returns></returns>
        bool Add(List<Model.users> userlist);
        /// <summary>
        /// 跟新员工信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool Update(Model.users user);
        bool Delete(users user);
    }
}
