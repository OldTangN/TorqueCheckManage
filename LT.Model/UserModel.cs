using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
   public  class UserModel:users 
    {       
       public string departName { get; set; }
       public string roleName { get; set; }
       public string roleID { get; set; }///角色的guid
       public string dutyname { get; set; }
    }
   public class users  {
       public int id { get; set; }
       public string username { get; set; }
       public string password { get; set; }
       public bool  is_superuser { get; set; }
       public bool  is_staff { get; set; }
       public DateTime joinDate { get; set; }
       public string duties { get; set; }
       public string empID { get; set; }
       public string cardID { get; set; }
       public string phoneNumber { get; set; }
       public string IDNum { get; set; }
       public string mail { get; set; }
       public string department { get; set; }         
       public string comment { get; set; }
       public string guid { get; set; }
   
   }

   public class ShowUser {
       public users user { get;set;}
       public Userchild userchild { get; set; }
   }
   public class Userchild {    
       public string departName { get; set; }
       public string roleName { get; set; }
   }
   public class userinfo {
       public users user { get; set; }
       public department department { get; set; }
       public role role { get; set; }
       public duties duty { get; set; }
   }
  
}
