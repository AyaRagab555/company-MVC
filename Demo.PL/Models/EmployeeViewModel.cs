using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public int DepartmentId { get; set; }
        public virtual DepartmentViewModel Department { get; set; }
    }
}
