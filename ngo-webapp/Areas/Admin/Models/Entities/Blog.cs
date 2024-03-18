using System;
using System.Collections.Generic;

namespace ngo_webapp.Areas.Admin.Models.Entities;

public partial class Blog
{
	public int BlogId { get; set; }

	public int AppealId { get; set; }

	public string Title { get; set; } = null!;

	public string Content { get; set; } = null!;

	public int UserId { get; set; }

	public DateTime CreationDate { get; set; }

	public virtual Appeal Appeal { get; set; } = null!;

	public virtual User User { get; set; } = null!;
}
