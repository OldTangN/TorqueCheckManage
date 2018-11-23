using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.Sockets.Tools
{
    public class TestResult
    {
        /// <summary>
        ///实际扭矩
        /// </summary>
        public string RealTar { set; get; }

        /// <summary>
        /// 实际角度
        /// </summary>
        public string RealAgi { set; get; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsOk { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { set; get; }
    }
    public class ReciveEventArgs : EventArgs
    {
        public byte[] data { set; get; }
        public ReciveEventArgs(byte[] _data) : base()
        {
            this.data = _data;
        }
    }
}