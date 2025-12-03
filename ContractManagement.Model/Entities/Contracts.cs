using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.Entities
{
    public class Contracts
    {
        public int Id { get; set; }             // Primary key
        public int CompanyId { get; set; }      // Foreign key to Company
        public int UserId { get; set; }     // Foreign key to User
        [Required(ErrorMessage = "The role is required")]
        [MinLength(4, ErrorMessage = "The position must be at least 4 characters long")]
        public string Position { get; set; } = null!; // Employee role
        [Required(ErrorMessage = "The description is required")]
        [MinLength(10, ErrorMessage = "The description must be at least 10 characters long")]
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(100, double.MaxValue,ErrorMessage ="The wage must be at least $100")]
        public decimal Wage { get; set; }
    }
}
