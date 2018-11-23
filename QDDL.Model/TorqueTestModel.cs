using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Model
{
    /// <summary>
    /// 校验台参数设置
    /// </summary>
    public class TorqueTestModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string testername { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public string portname { get; set; }
        /// <summary>
        /// 比特位
        /// </summary>
        public int databit { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int baundrate { get; set; }
        /// <summary>
        /// 最小校验范围
        /// </summary>
        public decimal minvalue { get; set; }
        /// <summary>
        /// 最大校验范围
        /// </summary>
        public decimal maxvalue { get; set; }
    }
}
