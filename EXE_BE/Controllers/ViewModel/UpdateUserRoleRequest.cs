using EXE_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace EXE_BE.Controllers.ViewModel
{
    public class UpdateUserRoleRequest
    {
        [Required(ErrorMessage = "Role is required")]
        public user_role Role { get; set; }
    }
}