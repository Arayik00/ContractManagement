using ContractManagement.BL.Entities;
using ContractManagement.Model.DTO;
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
        Task<EditContractDto>? GetContractById(int id);
        Task<List<Contracts>> GetAllContracts();
        Task<List<ContractDto>> GetAllCompanyContracts(int companyId);
        Task<FilterContractDto> GetAllCompanyContracts(int companyId, UserFilter filter);
        Task<bool> InsertContractAsync(Contracts contract);

        Task<bool> DeleteContractAsync(int ContractId);

        Task<bool> UpdateContractData(EditContractDto updatedContract);

        Task<string> getUserAccessLevel(int contractId, int userId);


    }
}
