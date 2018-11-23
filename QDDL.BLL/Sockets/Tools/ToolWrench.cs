using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDDL.BLL.Sockets.Tools
{
    public class ToolWrench
    {
        /// <summary>
        /// PsetID
        /// </summary>
        public int PsetId { set; get; }
        /// <summary>
        /// 目标扭矩
        /// </summary>
        public decimal torTarget { set; get; }
        /// <summary>
        /// 最大扭矩
        /// </summary>
        public decimal torMax { set; get; }
        /// <summary>
        /// 最小扭矩
        /// </summary>
        public decimal torMin { set; get; }
        /// <summary>
        /// 目标角度
        /// </summary>
        public decimal aglTarget { set; get; }
        /// <summary>
        /// 最大角度
        /// </summary>
        public decimal aglMax { set; get; }
        /// <summary>
        /// 最大角度
        /// </summary>
        public decimal aglMin { set; get; }

        /// <summary>
        /// 工具IP地址
        /// </summary>
        public string ToolsAddress { set; get; }
        /// <summary>
        /// 工具端口号
        /// </summary>
        public int ToolsPort { set; get; }

        ///// <summary>
        ///// 工具品牌
        ///// </summary> 
        //public Toolsbrand Toolsbrand { set; get; }

    }
}