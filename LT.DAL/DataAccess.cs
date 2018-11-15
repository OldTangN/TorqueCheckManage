using LT.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.DAL
{
   public static class DataAccess
    {
      static  string DBType = OperationConfig.GetValue("dbtype");
      static  string assemblyName = "LT.DAL";
      

      public static IBorrow CreateBorrow()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "Borrow";
          return ReflectionHelper.CreateInstance<IBorrow>(assemblyName, NameSpaceName, className);
      }

      public static IBorrowRecord CreateBorrowRecord()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "BorrowRecord";
          return ReflectionHelper.CreateInstance<IBorrowRecord>(assemblyName, NameSpaceName, className);
      }

      public static ICheckTarget CreateCheckTarget()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "CheckTarget";
          return ReflectionHelper.CreateInstance<ICheckTarget>(assemblyName, NameSpaceName, className);
      }


      public static ICheckTargetRecord CreateCheckTargetRecord()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "CheckTargetRecord";
          return ReflectionHelper.CreateInstance<ICheckTargetRecord>(assemblyName, NameSpaceName, className);
      }

      public static IDepartment CreateDepartment()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "Department";
          return ReflectionHelper.CreateInstance<IDepartment>(assemblyName, NameSpaceName, className);
      }

      public static IErrorRangset CreateErrorRangset()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "ErrorRangset";
          return ReflectionHelper.CreateInstance<IErrorRangset>(assemblyName, NameSpaceName, className);
      }

      public static IProject CreateProject()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "Project";
          return ReflectionHelper.CreateInstance<IProject>(assemblyName, NameSpaceName, className);
      }

      public static ISystemCheckset CreateSystemCheckset()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "SystemCheckset";
          return ReflectionHelper.CreateInstance<ISystemCheckset>(assemblyName, NameSpaceName, className);
      }

      public static IUser CreateUser()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "User";
          return ReflectionHelper.CreateInstance<IUser>(assemblyName, NameSpaceName, className);
      }

      public static IUserDuty CreateUserDuty()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "UserDuty";
          return ReflectionHelper.CreateInstance<IUserDuty>(assemblyName, NameSpaceName, className);
      }

      public static IUserRole CreateUserRole()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "UserRole";
          return ReflectionHelper.CreateInstance<IUserRole>(assemblyName, NameSpaceName, className);
      }

      public static IUserToRole CreateUserToRole()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "UserToRole";
          return ReflectionHelper.CreateInstance<IUserToRole>(assemblyName, NameSpaceName, className);
      }

      public static IWrench CreateWrench()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "Wrench";
          return ReflectionHelper.CreateInstance<IWrench>(assemblyName, NameSpaceName, className);
      }

      public static IWrenchSpecies CreateWrenchSpecies()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "WrenchSpecies";
          return ReflectionHelper.CreateInstance<IWrenchSpecies>(assemblyName, NameSpaceName, className);
      }

      public static IWrenchStatus CreateWrenchStatus()
      {
          string NameSpaceName = assemblyName + "." + DBType;
          string className = DBType + "WrenchStatus";
          return ReflectionHelper.CreateInstance<IWrenchStatus>(assemblyName, NameSpaceName, className);
      }

    }
}
