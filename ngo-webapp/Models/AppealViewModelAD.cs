using System.ComponentModel.DataAnnotations;

namespace ngo_webapp.Models;

public class AppealViewModelAD
{
    public int AppealsId { get; set; }

    [Required]
    [Display(Name = "Appeal Name")]
    public string AppealsName { get; set; }

    [Display(Name = "Organization")]
    public string? Organization { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }


    [Required]
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required]
    [Display(Name = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }


    [Display(Name = "Appeal Image")]
    public IFormFile? AppealImageFile { get; set; }

    public string? ExistingImagePath { get; set; }
}