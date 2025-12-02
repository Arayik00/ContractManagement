using ContractManagement.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Interfaces
{
    public interface ICompanyService
    {
        Task<Companies>? GetCompanyById(int id);
        Task<List<Companies>> GetAllCompanies();
        Task<List<Companies>> GetAllCompaniesByAdminId(int adminId);

        Task<int> InsertCompanyAsync(Companies company);
    }
}
