using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LT.BLL
{
  public  class SetPage<T>
    {
      List<T> _list =null;
      public List<T> TList
      {
          get { return _list; }
          set { _list = value; }
      }
      DataGrid _datagrid=null;
      int pagecount = 0;
      public  int pageno = 0;
      int pagesize = 30;
      public int PageSize
      {
          get { return pagesize; }
          set { pagesize = value; }
                 
      }
      public SetPage(DataGrid showgrid)
      {
         
          _datagrid = showgrid;     
      }
      public  int  PageCount()
      {
          if (PageSize <= 0)
          {
              pagecount = 0;
              return 0;
          }
         return  pagecount = _list.Count / PageSize + (_list.Count % PageSize > 0 ? 1 : 0);
      }

     public  void  firstpage()
      {
          pageno = 1; 
          _datagrid.ItemsSource = null;
          _datagrid.ItemsSource= _list.Take(PageSize ).ToList();
      }
     public void endpage()
      {
          _datagrid.ItemsSource = null;
          _datagrid.ItemsSource=  _list.Skip((pagecount - 1) * PageSize).Take(PageSize ).ToList ();
      }
     public void perPage()
      {

          if (pageno == 1)
          {
              pageno = 1;
          }
          else
          {
              pageno--;
          }
           _datagrid.ItemsSource = null;
           _datagrid .ItemsSource = _list.Skip((pageno - 1) * PageSize).Take(PageSize).ToList();
      }
     public void nextPage()
      {

          if (pageno >= pagecount)
          { 
              pageno = pagecount; 
          }
          else 
          { 
          pageno++;
          }
          _datagrid.ItemsSource = null;
          _datagrid .ItemsSource = _list.Skip((pageno - 1) * PageSize).Take(PageSize).ToList();
      }
     public void targetPage(int page)
      {
          if (page > pagecount)
              return;
          _datagrid.ItemsSource = null;
          _datagrid .ItemsSource = _list.Skip((page - 1) * PageSize).Take(PageSize).ToList();
      }
    }
}
