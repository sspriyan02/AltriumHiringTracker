using System.ComponentModel.DataAnnotations;

namespace AltriumHiringTracker.Data
{
    public class Vacancy
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string HiringManager { get; set; } = string.Empty;

        [Range(1, 999)]
        public int NumberOfOpenings { get; set; } = 1;

        [Required]
        public string EmploymentType { get; set; } = "Full-time";

        [Required]
        public string Status { get; set; } = "Open";

        public DateTime? OpenDate { get; set; }

        public DateTime? ClosingDate { get; set; }

        [Required]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        public string ExpectedCriteria { get; set; } = string.Empty;

        [Required]
        public string RecruitmentStage { get; set; } = "CV Screening";

        public DateTime CreatedAt { get; set; }
    }
}