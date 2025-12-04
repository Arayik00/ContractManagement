using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.DTO
{
    public class FilterContractDto
    {
        public bool hasNextPage { set; get; }
        public bool hasPreviousPage { set; get; }

        public List<ContractDto> contracts { set; get; }
    }
}
