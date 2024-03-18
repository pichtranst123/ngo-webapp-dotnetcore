using ngo_webapp.Models.Entities;

namespace ngo_webapp.Models;

public class AppealViewModel
{
    public Appeal Appeal { get; set; }
    public decimal TotalDonations { get; set; }
    public bool IsEnded => TotalDonations >= Appeal.Amount;
}
