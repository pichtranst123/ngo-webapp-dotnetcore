namespace ngo_webapp.Models;

public class ProfileViewModel
{
    public string Username { get; set; }
    public DateTime RegistrationDate { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalAmount { get; set; }
    public int ProjectCount { get; set; }
    public string Bio {  get; set; }
    public string Email {  get; set; }
	public string? UserImage { get; set; }
	public List<DonationDetail> Donations { get; set; } = new List<DonationDetail>();


    public class DonationDetail
    {
        public int AppealId { get; set; }
        public string AppealName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
