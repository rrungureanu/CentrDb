using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentricaAPI.Entities
{
    public class Salesperson
    {
        public int SalespersonId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsPrimary { get; set; }
    }
}
