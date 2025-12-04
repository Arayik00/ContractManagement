using ContractManagement.BL.Entities;
using ContractManagement.Model.DTO;
using ContractManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Interfaces
{
    public interface IContractRepository
    {
        Task<ContractDto>? GetById(int id);
        Task<List<Contracts>> GetAll();
        Task<List<ContractDto>> GetAllCompanyContracts(int CompanyId);
        Task<FilterContractDto> GetAllCompanyContracts(int CompanyId, UserFilter filter);

        Task<bool> InsertContractAsync(Contracts contract);

        Task<bool> DeleteContractAsync(int ContractId);

        Task<bool> UpdateContractData(EditContractDto updatedContract);
        Task<string> getUserAccessLevel(int contractId, int userId);

    }
}
