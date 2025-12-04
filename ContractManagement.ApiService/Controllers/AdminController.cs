using ContractManagement.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.ApiServer.Controllers
{
    public class AdminController : Controller
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
        }
    }
}
