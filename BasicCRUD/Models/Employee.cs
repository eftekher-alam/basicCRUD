using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BasicCRUD.Models
{
    public class Employee
    {
        [Key]
        [Required]
        [StringLength(maximumLength: 4, MinimumLength = 4, ErrorMessage = "Lenght must be 4 char.")]
        [Display(Name = "Employee Id")]
        public string EmpID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(maximumLength: 11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits.")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        [Display(Name = "Join Date")]
        [DisplayFormat(DataFormatString ="{yyyy-MM-dd}")]
        public DateTime JoinDate { get; set; }

        public string ProPic { get; set; }


        [Display(Name ="Department")]
        public int DeptId { get; set; }
        [ForeignKey("DeptId")] //This anotation is optional, but if you add, it must be after the prop

        public Department Department { get; set; }
    }
}
