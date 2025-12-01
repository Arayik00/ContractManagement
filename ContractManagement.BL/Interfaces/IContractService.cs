using ContractManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Interfaces
{
    public interface IContractService
    {
        Task<Contracts>? GetContractById(int id);
        Task<List<Contracts>> GetAllContracts();
        Task<List<ContractNames>> GetAllCompanyContracts(int companyId);

        Task<bool> InsertContractAsync(Contracts contract);

        Task<bool> DeleteContractAsync(int ContractId);

        Task<bool> UpdateContractData(Contracts updatedContract);

        Task<bool> CanUserEditContractAsync(int contractId, int userId);


    }
}
