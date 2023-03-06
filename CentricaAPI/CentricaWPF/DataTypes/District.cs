using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentricaAPI.Entities
{
    public class District
    {
        public int DistrictId { get; set; }
        public int PrimarySalespersonId { get; set; }
        public string Name { get; set; }
        public List<Salesperson> Salespersons { get; set; } = new List<Salesperson>();
        public List<Store> Stores { get; set; } = new List<Store>();
    }
}
