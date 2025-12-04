using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.Models
{
    public class UserFilter
    {
        public string? searchKey { set; get; } = "";
        public int pageNumber { set; get; } = 1;
        public int pageSize { set; get; } = 5;

        public string contractStatus { set; get; } = "all";
    }
}
