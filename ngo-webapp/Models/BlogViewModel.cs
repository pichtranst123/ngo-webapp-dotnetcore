using ngo_webapp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ngo_webapp.Models;

public class BlogViewModel
{
	[Required]
	public int BlogId { get; set; }

	[Required]
	public string Title { get; set; } = string.Empty;
	[Required]
	public string Content { get; set; }

	[Required]
	public string BlogImage { get; set; }
	[Required]
	public DateTime CreationDate { get; set; }
	[Required]
	public int AppealID { get; set; }
	[Required]
	public int UserId { get; set; }
	public IEnumerable<Comment> Comments { get; set; }
}
