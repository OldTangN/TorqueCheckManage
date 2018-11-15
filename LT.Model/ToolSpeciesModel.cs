using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Model
{
    public class wrenchspecies {
        public int id { get; set; }
        public string speciesName { get; set; }
        public string speciesCode { get; set; }
        public string parentSpecies { get; set; }
        public string comment { get; set; }
        public string guid { get; set; }
    }
  public   class ToolSpeciesModel:wrenchspecies
    {
    }
}
