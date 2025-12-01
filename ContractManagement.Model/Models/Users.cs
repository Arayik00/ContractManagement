using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.Models
{
    public class Users
    {
        public int Id { get; set; }             // Primary key
        
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!; // Hash in production
    }
}
