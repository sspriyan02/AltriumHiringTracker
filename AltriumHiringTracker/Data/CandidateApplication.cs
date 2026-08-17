using System.ComponentModel.DataAnnotations;

namespace AltriumHiringTracker.Data
{
    public class CandidateApplication
    {
        public int Id { get; set; }

        [Required]
        public int VacancyId { get; set; }

        public Vacancy? Vacancy { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string HighestQualification { get; set; } = string.Empty;

        [Range(0, 60)]
        public int YearsOfExperience { get; set; }

        [StringLength(255)]
        public string CvOriginalFileName { get; set; } = string.Empty;

        [StringLength(255)]
        public string CvStoredFileName { get; set; } = string.Empty;

        [StringLength(500)]
        public string CvFilePath { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Submitted";

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public decimal? MatchScore { get; set; }

        [StringLength(50)]
        public string MatchCategory { get; set; } = "Not assessed";

        public string? MatchedCriteria { get; set; }

        public string? MissingCriteria { get; set; }
    }
}