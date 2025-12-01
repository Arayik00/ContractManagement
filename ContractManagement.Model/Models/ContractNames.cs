using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.Models
{
    public class ContractNames
    {
        public Contracts contract { set; get; }
        public string firstName { set; get; }
        public string lastName { set; get; }
    }
}
