using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model.BllModel
{
    /// <summary>
    /// treeview的node结构
    /// </summary>
   public  class PropertyNodeItem
    {
        public string Icon { get; set; }//图片路径
        public string EditIcon { get; set; }//图片路径
        public string DisplayName { get; set; }//显示名称
        public string Name { get; set; }
        public int id { get; set; }//id
        public int parentId { get; set; }//父id
        public bool IsExpanded { get; set; }//是否展开
        public List<PropertyNodeItem> Children { get; set; }//子类集
        public PropertyNodeItem()
        {
            Children = new List<PropertyNodeItem>();
        }
    }
}
