using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ngo_webapp.Models.Entities; // Assuming this is where your Blog and Comment entities are defined

namespace ngo_webapp.Models;

public class BlogDetailViewModel
{
	[Required]
	public int BlogId { get; set; }

	[Required]
	public string Title { get; set; } = string.Empty;

	[Required]
	public string Content { get; set; }


	[Required]
	public DateTime CreationDate { get; set; }

	[Required]
	public int AppealID { get; set; }

	[Required]
	public int UserID { get; set; }

	// Property to hold the list of comments associated with the blog
	public List<Comment> Comments { get; set; } = new List<Comment>();

	// Property to hold a new comment that a user may submit from the blog detail page
	public Comment NewComment { get; set; } = new Comment();
}
