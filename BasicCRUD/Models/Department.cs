using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCRUD.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }

        [Required]
        [Display(Name ="Department Name")]
        public string DepName { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
