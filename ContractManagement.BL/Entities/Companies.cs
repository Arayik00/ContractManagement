using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Entities
{
    public class Companies
    {
        public int Id { get; set; }             // Primary key
        public int AdminId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
    }
}
