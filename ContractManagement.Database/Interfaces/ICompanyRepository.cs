using ContractManagement.Model.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Companies>? GetById(int id);
        Task<List<Companies>> GetAll();
        Task<List<Companies>> GetAllByAdminId(int adminId);

        Task<int> InsertCompanyAsync(Companies company);
    }
}
