using QDDL.Comm;
using QDDL.DAL.Service;
using QDDL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.DAL.MySql
{
  public   class MySqlBorrowRecord:IBorrowRecord 
    {
          string _webip = OperationConfig.GetValue("WebServiceIp");

        public List<Model.borrowrecord> selectByIsreturn(bool isreturn)
        {
            throw new NotImplementedException();
        }

        public List<Model.borrowrecord> selectall()
        {
            throw new NotImplementedException();
        }

        public bool add(Model.borrowrecord borrowrecordmodel)
        {
            throw new NotImplementedException();
        }

        public bool addmany(List<Model.borrowrecord> borrowrecordlist)
        {
            return ServerHelp.addManyInfoReturnBool<borrowrecord>(borrowrecordlist,_webip);
        }

        public bool update(Model.borrowrecord borrowrecordmodel)
        {
            string tablename = typeof(borrowrecord).Name;
            string contion = string.Format("guid='{0}'",borrowrecordmodel.guid );
            string strsql = string.Format("WrenchID_id='{0}',returnDate='{1}',returnOperator='{2}',returnUser='{3}',is_return='{4}',comment='{5}'",borrowrecordmodel .WrenchID ,borrowrecordmodel .returnDate =DateTime.Now .ToString ("yyyy-MM-dd hh:mm:ss"),borrowrecordmodel.returnOperator ,borrowrecordmodel .returnUser ,borrowrecordmodel .is_return ?1:0,borrowrecordmodel .comment );

            return ServerHelp.updateByWhere(tablename ,_webip ,contion ,strsql);
        }


        public List<borrowrecord> select(string wrenchid, bool is_return)
        {
            string contion = string.Format("WrenchID_id='{0}' and is_return='{1}'",wrenchid ,is_return ?1:0);
            return ServerHelp.findDataByCondition<borrowrecord>(contion ,_webip );
        }
    }
}
