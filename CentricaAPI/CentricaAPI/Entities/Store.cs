using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentricaAPI.Entities
{
    public class Store
    {
        public int StoreId { get; set; }
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }
}
