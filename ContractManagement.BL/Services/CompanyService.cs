using ContractManagement.BL.Interfaces;
using ContractManagement.BL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<List<Companies>> GetAllCompanies()
        {
            return await _companyRepository.GetAll();
        }

        public async Task<Companies?> GetCompanyById(int id)
        {
            return await _companyRepository.GetById(id);
        }
        public async Task<List<Companies?>> GetAllCompaniesByAdminId(int id)
        {
            return await _companyRepository.GetAllByAdminId(id);
        }

        public async Task<int> InsertCompanyAsync(Companies company)
        {
            return await _companyRepository.InsertCompanyAsync(company);
        }
    }
}
