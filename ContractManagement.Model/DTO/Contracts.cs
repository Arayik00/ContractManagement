using ContractManagement.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Model.DTO
{
    public class ContractDto
    {
        public UserDto User { get; set; }
        public int Id { get; set; }
        public string Position { get; set; } = null!; // Employee role
        public string Description { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Range(100, double.MaxValue, ErrorMessage = "The wage must be at least $100")]
        public decimal Wage { get; set; }
    }
}
