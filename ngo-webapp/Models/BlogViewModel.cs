using System;
using System.ComponentModel.DataAnnotations;
namespace ngo_webapp.Models;

public class BlogViewModel
{

	public int BlogId { get; set; }

	[Required(ErrorMessage = "Title is required")]
	public string Title { get; set; }

	[Required(ErrorMessage = "Content is required")]
	public string Content { get; set; }

	public int AppealId { get; set; }

	public int UserId { get; set; }

	public DateTime CreationDate { get; set; }

}