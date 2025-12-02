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
        public string Position { get; set; } = null!; // Employee role
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(100, double.MaxValue,ErrorMessage ="The wage must be between $200-100000")]
        public decimal Wage { get; set; }
    }
}
