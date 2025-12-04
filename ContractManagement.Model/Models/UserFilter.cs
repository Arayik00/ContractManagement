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
        public int pageNumber { set; get; }
        public int pageSize { set; get; }
        public string contractStatus { set; get; } = "All";
    }
}
