using ContractManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Database.Interfaces
{
    public interface IContractRepository
    {
        Task<Contracts>? GetById(int id);
        Task<List<Contracts>> GetAll();
        Task<List<ContractNames>> GetAllCompanyContracts(int CompanyId);

        Task<bool> InsertContractAsync(Contracts contract);

        Task<bool> DeleteContractAsync(int ContractId);

        Task<bool> UpdateContractData(Contracts updatedContract);
        Task<bool> CanUserEditContractAsync(int contractId, int userId);

    }
}
