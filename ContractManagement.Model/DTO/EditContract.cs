using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.DTO
{
    public class EditContractDto
    {
        public ContractDto contract { set; get; }
        public bool hasAdminAccess { set; get; } = false;
    }
}
