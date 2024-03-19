namespace ngo_webapp.Models;

public class ProfileViewModel
{
    public string Username { get; set; }
<<<<<<< HEAD
	public string Email { get; set; } = null!;
=======
>>>>>>> 5e7047f48a6768fb60e7cd64c4d3616fdf36defd
    public DateTime RegistrationDate { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalAmount { get; set; }
    public int ProjectCount { get; set; }
<<<<<<< HEAD
    public List<DonationDetail> Donations { get; set; } = new List<DonationDetail>();

	public class DonationDetail
=======
    public string Bio {  get; set; }
    public string Email {  get; set; }
	public string? UserImage { get; set; }
	public List<DonationDetail> Donations { get; set; } = new List<DonationDetail>();


    public class DonationDetail
>>>>>>> 5e7047f48a6768fb60e7cd64c4d3616fdf36defd
    {
        public int AppealId { get; set; }
        public string AppealName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
