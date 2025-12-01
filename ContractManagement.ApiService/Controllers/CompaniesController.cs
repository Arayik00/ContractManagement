using ContractManagement.BL.Interfaces;
using ContractManagement.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.ApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET api/companies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllCompanies();
            return Ok(companies);
        }

        // GET api/companies/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetCompanyById(id);
            return company == null ? NotFound() : Ok(company);
        }

        // GET api/companies/admin/{adminId}
        [HttpGet("admin/{adminId:int}")]
        public async Task<IActionResult> GetByAdminId(int adminId)
        {
            var companies = await _companyService.GetAllCompaniesByAdminId(adminId);
            return Ok(companies);
        }

        // POST api/companies
        [HttpPost]
        public async Task<IActionResult> CreateCompany(Companies company)
        {
            var created = await _companyService.InsertCompanyAsync(company);
            return Ok(created);
        }
    }
}
