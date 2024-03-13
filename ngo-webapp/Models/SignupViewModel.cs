using System.ComponentModel.DataAnnotations;

namespace ngo_webapp.Models;

public class SignupViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
