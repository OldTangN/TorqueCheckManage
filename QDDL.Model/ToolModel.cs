using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDDL.Model
{
    public class wrench
    {
        public int id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string wrenchCode { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string wrenchBarCode { get; set; }
        /// <summary>
        /// 最小范围
        /// </summary>
        public decimal rangeMin { get; set; }
        /// <summary>
        /// 最大范围
        /// </summary>
        public decimal rangeMax { get; set; }
        /// <summary>
        /// 校验值
        /// </summary>
        public decimal targetvalue { get; set; }
        /// <summary>
        /// 辅助值1
        /// </summary>
        public decimal targetvalue1 { get; set; }
        /// <summary>
        /// 辅助值2 
        /// </summary>
        public decimal targetvalue2 { get; set; }
        /// <summary>
        /// 对应Pset值 如1/2/3 
        /// </summary>
        public string offPset { set; get; }
        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 厂家
        /// </summary>
        public string factory { get; set; }
        public DateTime createDate { get; set; }
        public string IP { get; set; }
        public string port { get; set; }
        /// <summary>
        /// 类别guid
        /// </summary>
        public string species { get; set; }
        /// <summary>
        /// 状态guid
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 最近维护时间
        /// </summary>
        public DateTime lastrepair { get; set; }
        public decimal cycletime { get; set; } = 0;
        public bool isallowcheck { get; set; }//1允许2不允许
        public string guid { get; set; }
    }
    public class ToolModel : wrench
    {

        // public wrench wrenchdata { get; set; }
        public string speciesName { get; set; }
        public string statusName { get; set; }
        public string statusDM { get; set; }
    }
    public class Toolinfo
    {
        public wrench wrench { get; set; }
        /// <summary>
        /// 工具类别名称
        /// </summary>
        public string speciesName { get; set; }

        /// <summary>
        /// 类别编码
        /// </summary>
        public string speciesCode { set; get; }
        /// <summary>
        /// 工具状态名称
        /// </summary>
        public string statusName { get; set; }

    }
    public class wrenchinfo
    {
        public wrench wrench { get; set; }
        public wrenchspecies species { get; set; }
        public wrenchstatus status { get; set; }

    }
}
