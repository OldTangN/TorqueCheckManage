using QDDL.Comm;
using QDDL.DAL;
using QDDL.DAL.MySql;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.BLL
{
    public class UserLogin
    {
        string _name;
        string _pwd;
        string CardID;
        IUser User = DataAccess.CreateUser();
        IUserRole UserRole = DataAccess.CreateUserRole();
        IDepartment Department = DataAccess.CreateDepartment();
        IUserToRole UserToRole = DataAccess.CreateUserToRole();
        IProject Project = DataAccess.CreateProject();
        string programname = OperationConfig.GetValue("programname");
        public userinfo _userinfo = new userinfo();
        system _system = new system();
        public UserLogin(string name, string pwd, string _CardID)
        {
            _name = name;
            _pwd = pwd;
            this.CardID = _CardID;
        }


        users getuser()
        {
            if (_name == null && CardID != null)
            {
                return User.Select(CardID);
            }
            else
            {
                return User.Select(_name, MD5Encrypt.GetMD5(_pwd));
            }
        }
        system getsystem()
        {
            try
            {

                List<system> s = Project.selectByname(programname);
                if (s != null && s.Count > 0)
                    return s.FirstOrDefault();
                return null;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(role), "获取系统信息失败" + ex);
                return null;
            }
        }
        role getrole()
        {
            role ro = new role();
            try
            {
                _system = getsystem();

                // LogUtil.WriteLog(typeof(role), _system.systemName );
                if (_system == null || _system.guid == null)
                    return ro = null;

                users u = getuser();
                if (u == null || string.IsNullOrEmpty(u.guid))
                    return ro = null;
                LogUtil.WriteLog(typeof(role), u.username);
                _userinfo.user = u;
                List<QDDL.Model.UserToRoleModel.usertorole> r = UserToRole.selectbyuserid(_userinfo.user.guid);
                if (r == null || r.Count <= 0)
                    return ro = null;
                List<role> re = UserRole.selectSysidandguid(_system.guid, r.FirstOrDefault().role);
                if (re == null || re.Count <= 0)
                    return ro = null;
                LogUtil.WriteLog(typeof(role), ro.roleName);
                return _userinfo.role = re.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(role), "获取人员信息失败！" + ex);
                return ro = null;
            }
        }
        department getdepartment()
        {
            department d = new department();
            d = Department.SelectByGuid(_userinfo.user.department);
            return d;
        }
        public int emplogin()
        {
            try
            {

                role r = getrole();
                if (r == null)
                    return 0;
                if (_userinfo.user != null && _userinfo.role != null)
                {
                    _userinfo.department = getdepartment();
                    return 1;
                }
                return 0;

            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(typeof(department), "登录失败！");
                return 0;
            }
        }
        public userinfo returnuser()
        {
            return _userinfo;
        }
    }
    public class GetRole
    {
        string programname = OperationConfig.GetValue("programname");
        IProject Project = DataAccess.CreateProject();
        IUserRole UserRole = DataAccess.CreateUserRole();
        system getsystem(string programname)
        {
            system sys = new system();
            List<system> s = Project.selectByname(programname);
            if (s == null || s.Count <= 0)
                return sys = null;
            return s.FirstOrDefault();
        }
        public List<role> getrole()
        {
            List<role> roles = new List<role>();
            if (getsystem(programname) != null)
                roles = UserRole.SelectBySysGuid(getsystem(programname).guid);
            return roles;
        }
    }
    public class GetUser
    {
        IUser User = DataAccess.CreateUser();
        IUserRole UserRole = DataAccess.CreateUserRole();
        IDepartment Department = DataAccess.CreateDepartment();
        IUserToRole UserToRole = DataAccess.CreateUserToRole();
        IUserDuty UserDuty = DataAccess.CreateUserDuty();
        IProject Project = DataAccess.CreateProject();
        string programname = OperationConfig.GetValue("programname");
        system getsystem(string programname)
        {
            system sys = new system();
            List<system> s = Project.selectByname(programname);
            if (s.Count > 0)
                return s.FirstOrDefault();
            return sys;
        }
        public List<users> getusers(string cardid)
        {
            List<users> us = new List<users>();
            users u = User.Select(cardid);
            if (u != null)
                us.Add(u);
            return us;
        }
        public List<users> getusers(string cardid, string pwd)
        {
            List<users> us = new List<users>();
            us = User.SelectByCard(cardid, MD5Encrypt.GetMD5(pwd));
            return us;
        }
        public userinfo getuserinfo(List<users> u)
        {
            List<users> us = u;
            userinfo ui = new userinfo();
            system s = getsystem(programname);
            List<role> rlist = new List<role>();
            department d = new department();
            duties duty = new duties();
            List<QDDL.Model.UserToRoleModel.usertorole> ur = new List<UserToRoleModel.usertorole>();
            if (us != null && us.Count > 0)
            {
                ur = UserToRole.selectbyuserid(us.FirstOrDefault().guid);
                d = Department.SelectByGuid(us.FirstOrDefault().department);
                duty = UserDuty.SelectByGuid(us.FirstOrDefault().duties);
            }
            if (s != null)
            {
                foreach (QDDL.Model.UserToRoleModel.usertorole um in ur)
                {
                    List<role> r = UserRole.selectSysidandguid(s.guid, um.role);
                    if (r.Count > 0)
                    {
                        rlist.Add(r.FirstOrDefault());
                    }
                }
                if (rlist != null && rlist.Count > 0)
                {
                    ui.user = us.FirstOrDefault();
                    ui.role = rlist.FirstOrDefault();
                    if (d.guid != null)
                        ui.department = d;
                    ui.duty = duty;
                }
            }
            return ui;
        }

        public List<QDDL.Model.UserToRoleModel.usertorole> GetUserToRole(string systemname)
        {
            system sys = getsystem(programname);
            if (sys == null)
                return null;
            List<role> ur = UserRole.SelectBySysGuid(sys.guid);
            if (ur == null || ur.Count < 0)
                return null;
            List<QDDL.Model.UserToRoleModel.usertorole> utr = new List<UserToRoleModel.usertorole>();
            foreach (role r in ur)
            {
                List<QDDL.Model.UserToRoleModel.usertorole> temputr = UserToRole.selectbyroleid(r.guid);
                if (temputr != null)
                {
                    foreach (QDDL.Model.UserToRoleModel.usertorole role in temputr)
                    {
                        utr.Add(role);
                    }
                }

            }
            return utr;
        }

        public List<UserModel> GetUserModel()
        {
            List<UserModel> uml = new List<UserModel>();
            List<QDDL.Model.UserToRoleModel.usertorole> utr = new List<UserToRoleModel.usertorole>();
            try
            {
                int count = 0;
                utr = GetUserToRole(programname);
                foreach (QDDL.Model.UserToRoleModel.usertorole ur in utr)
                {
                    count++;
                    users us = User.SelectByguid(ur.user);
                    role r = UserRole.SelectByGuid(ur.role);
                    department d = Department.SelectByGuid(us.department);
                    duties du = new duties();
                    du = UserDuty.SelectByGuid((us == null || us.duties == null) ? "" : us.duties);
                    uml.Add(new UserModel()
                    {
                        username = us.username,
                        id = count,
                        password = us.password,
                        is_superuser = us.is_superuser,
                        is_staff = us.is_staff,
                        //joinDate =us.joinDate.Replace ('T',' ') ,
                        joinDate = us.joinDate,
                        duties = us.duties,
                        empID = us.empID,
                        cardID = us.cardID,
                        phoneNumber = us.phoneNumber,
                        IDNum = us.IDNum,
                        mail = us.mail,
                        department = us.department,
                        comment = us.comment,
                        guid = us.guid,
                        departName = (d == null || d.departmentName == null) ? "" : d.departmentName,
                        roleName = (r == null || r.roleName == null) ? "" : r.roleName,
                        roleID = (r == null || r.roleDM == null) ? "" : r.guid,
                        dutyname = (du == null || du.dutiesName == null) ? "" : du.dutiesName
                    });

                }

            }
            catch (Exception ep) { return uml = null; }
            return uml;
        }

    }

}
