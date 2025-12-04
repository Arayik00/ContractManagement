using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.BL.Entities
{
    public class Users
    {
        public int Id { get; set; }             // Primary key

        [Required(ErrorMessage = "First name is required.")]
        [MinLength(2)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(2)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(4)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4)]
        public string Password { get; set; } = null!;
    }
}
