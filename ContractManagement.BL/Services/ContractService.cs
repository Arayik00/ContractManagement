using ContractManagement.BL.Interfaces;
using ContractManagement.Database.Interfaces;
using ContractManagement.Database.Repositories;
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

        public async Task<Contracts?> GetContractById(int id)
        {
            return await _contractRepository.GetById(id);
        }

        public async Task<List<ContractNames>> GetAllCompanyContracts(int companyId)
        {
            return await _contractRepository.GetAllCompanyContracts(companyId);
        }

        public async Task<bool> InsertContractAsync(Contracts contract)
        {
            return await _contractRepository.InsertContractAsync(contract);
        }

        public async Task<bool> DeleteContractAsync(int ContractId)
        {
            return await _contractRepository.DeleteContractAsync(ContractId);
        }

        public async Task<bool> UpdateContractData(Contracts updatedContract)
        {
            return await _contractRepository.UpdateContractData(updatedContract);
        }
        public async Task<bool> CanUserEditContractAsync(int contractId, int userId)
        {
            return await _contractRepository.CanUserEditContractAsync(contractId, userId);
        }
    }
}
