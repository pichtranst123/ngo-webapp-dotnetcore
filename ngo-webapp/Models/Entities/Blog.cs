using System;
using System.Collections.Generic;

namespace ngo_webapp.Models.Entities;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public int? UserId { get; set; }

    public string? BlogImage { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual User? User { get; set; }
}
