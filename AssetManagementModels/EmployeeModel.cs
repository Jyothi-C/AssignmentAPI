using System.ComponentModel.DataAnnotations;

namespace AssetManagementSystem.Models
{
    public class EmployeeModel
    {
        [Required(ErrorMessage = "UserName is Required")]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}