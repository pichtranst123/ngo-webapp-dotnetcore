using System.ComponentModel.DataAnnotations;

namespace ngo_webapp.Models;

public class AdminLoginViewModel
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }
	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[Required]
	public bool Is_Admin { get; set; }
}
