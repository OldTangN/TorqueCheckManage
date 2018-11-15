using LongTie.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace LongTie.Domain.ModleMap
{
    public class RoleMap : EntityTypeConfiguration<role>
    {
        public RoleMap()
        {
            this.HasKey(t=>t.id);
            this.HasRequired(t => t.roleName);
            this.HasRequired(t => t.system).WithMany(b => b.roles).HasForeignKey(t => t.systemguid).WillCascadeOnDelete();
           
           
        }
    }
}
