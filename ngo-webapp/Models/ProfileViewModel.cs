namespace ngo_webapp.Models;

public class ProfileViewModel
{
    public string Username { get; set; }
    public DateTime RegistrationDate { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalAmount { get; set; }
    public int ProjectCount { get; set; }
    public List<DonationDetail> Donations { get; set; } = new List<DonationDetail>();


    public class DonationDetail
    {
        public string AppealName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
        public int AppealId { get; set; }
    }
}
