using System;
using System.Collections.Generic;

namespace ngo_webapp.Models.Entities;

public partial class Appeal
{
    public int AppealsId { get; set; }

    public string AppealsName { get; set; } = null!;

    public string? Organization { get; set; }

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal Amount { get; set; }

    public bool? Status { get; set; }

    public string? AppealsImage { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
