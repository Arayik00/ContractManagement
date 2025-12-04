using ContractManagement.BL.Entities;
using ContractManagement.BL.Interfaces;
using ContractManagement.Model.DTO;
using ContractManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Services
{
    public class ContractService: IContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<List<Contracts>> GetAllContracts()
        {
            return await _contractRepository.GetAll();
        }

        public async Task<EditContractDto?> GetContractById(int id)
        {
            var contract = await _contractRepository.GetById(id);
            return new EditContractDto
            {
                contract = contract,
                hasAdminAccess = false,
            };
        }

        public async Task<List<ContractDto>> GetAllCompanyContracts(int companyId)
        {
            return await _contractRepository.GetAllCompanyContracts(companyId);
        }
        public async Task<FilterContractDto> GetAllCompanyContracts(int companyId, UserFilter filter)
        {
            return await _contractRepository.GetAllCompanyContracts(companyId, filter);
        }
        public async Task<bool> InsertContractAsync(Contracts contract)
        {
            return await _contractRepository.InsertContractAsync(contract);
        }

        public async Task<bool> DeleteContractAsync(int ContractId)
        {
            return await _contractRepository.DeleteContractAsync(ContractId);
        }

        public async Task<bool> UpdateContractData(EditContractDto updatedContract)
        {
            return await _contractRepository.UpdateContractData(updatedContract);
        }
        public async Task<string> getUserAccessLevel(int contractId, int userId)
        {
            return await _contractRepository.getUserAccessLevel(contractId, userId);
        }
    }
}
