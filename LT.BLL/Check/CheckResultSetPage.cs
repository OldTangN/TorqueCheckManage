using LT.DAL;
using LT.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LT.BLL.Check
{
    public class CheckResultSetPage
    {
        ICheckTarget CheckTarget = DataAccess.CreateCheckTarget();
        int pagesize = 20;
        int pageno = 0;
        int totalpage = 0;
        Dictionary<string, string> dict = null;
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }
        public int PageNo
        {
            get { return pageno; }
            set { pageno = value; }
        }
        public Dictionary<string, string> Dictionary
        {
            get { return dict; }
            set { dict = value; }
        }
        public int TotalPage
        {
            get { return totalpage; }
        }
        public int getTotalPage()
        {
            if (Dictionary != null)
                return totalpage = (CheckTarget.SelectCount(Dictionary) % PageSize) > 0 ? CheckTarget.SelectCount(Dictionary) / PageSize + 1 : CheckTarget.SelectCount(Dictionary) / PageSize;
            return 0;
        }
        public List<torquechecktarget> getTorquechecktarget(int num = 0)
        {
            if (totalpage > 0 && num < totalpage && num >= 0)
            {
                List<torquechecktarget> list = CheckTarget.SelectByContion(dict, PageSize, num);
                return list;
            }
            else
                return null;
        }
        public List<torquechecktarget> proPage()
        {
            
            if (PageNo >= 0)
            {
            PageNo--;
            return getTorquechecktarget(PageNo);
            }
               
            else
                return getTorquechecktarget(0);
        }

        public List<torquechecktarget> nextPage()
        {
           
            if (PageNo < totalpage)
            {
             PageNo++;
             return getTorquechecktarget(PageNo);
            }
                
            else
            {
                return null;

            }
        }
        public List<torquechecktarget> targetPage(int num)
        {
           
            if (PageNo >= 0 && PageNo < totalpage)
            {
                PageNo = num - 1;
                return getTorquechecktarget(PageNo);
            }

            else
                return null;
        }

    }
}
