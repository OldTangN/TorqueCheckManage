using LongTie.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace LongTie.Domain.ModleMap
{
    public class SystemMap : EntityTypeConfiguration<system>
    {
        public SystemMap()
        { 
        this.HasRequired(p=>p.roles)
        }
    }
}
