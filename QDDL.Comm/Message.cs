using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDDL.Comm
{
 public  static   class MessageAlert
    {
     public static   void Error(string str) {
        
         MessageBox.Show  (str,"错误！");
     }
     
     public static void Warning(string str)
     {
         MessageBox.Show(str, "警告！");
     }
     public static void Alert(string str)
     {
         MessageBox.Show(str,"提示！");
     }
     public static bool Alter(string str) {
         if (MessageBox.Show(str, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
             return true;
         }
         return false;
     }
  
    }
}
