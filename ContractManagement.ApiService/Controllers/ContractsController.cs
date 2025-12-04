using ContractManagement.BL.Entities;
using ContractManagement.BL.Interfaces;
using ContractManagement.Model.DTO;
using ContractManagement.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace ContractManagement.ApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;
        private readonly IUserService _userService;


        public ContractsController(IContractService contractService, IUserService userService)
        {
            _contractService = contractService;
            _userService = userService;
        }

        // GET api/contracts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _contractService.GetAllContracts();
            return Ok(contracts);
        }

        // GET api/contracts/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(); // 401
            string UserAccessLevel = await _contractService.getUserAccessLevel(id, userId);
            Console.WriteLine(UserAccessLevel);
            if (UserAccessLevel == "Denied")
            { 
                return Forbid(); 
            }
            var contract = await _contractService.GetContractById(id);
            contract.hasAdminAccess = UserAccessLevel == "Full";
            return contract == null ? NotFound() : Ok(contract);
        }

        // GET api/contracts/company/3
        //[HttpGet("company/{companyId:int}")]
        //public async Task<IActionResult> GetAllCompanyContracts(int companyId)
        //{
        //    var contracts = await _contractService.GetAllCompanyContracts(companyId);
        //    return Ok(contracts);
        //}
        // GET api/contracts/company/3
        [HttpGet("company/{companyId:int}")]
        public async Task<IActionResult> GetAllCompanyContracts(int companyId, [FromQuery] UserFilter filter)
        {
            var contracts = await _contractService.GetAllCompanyContracts(companyId, filter);
            return Ok(contracts);
        }
        // POST api/contracts
        [HttpPost]
        public async Task<IActionResult> Create(Contracts contract)
        {
            var newContract = await _contractService.InsertContractAsync(contract);
            return Ok(newContract);
        }

        // DELETE api/contracts/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(); // 401
            var result = await _contractService.DeleteContractAsync(id);
            return Ok(result);
        }

        // PATCH api/contracts/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, EditContractDto updatedContract)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized(); // 401
            updatedContract.contract.Id = id; // enforce consistency
            var access = await _contractService.getUserAccessLevel(id, userId);
            if(access == "Denied")
            {
                return Unauthorized(); // 401
            }
            updatedContract.hasAdminAccess = access == "Full";
            var result = await _contractService.UpdateContractData(updatedContract);
            result = await _userService.UpdateUserAsync(updatedContract.contract.User);
            return Ok(result);
        }

    }

}
