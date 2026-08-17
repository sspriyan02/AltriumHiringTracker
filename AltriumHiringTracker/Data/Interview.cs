using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltriumHiringTracker.Data;

public class Interview
{
    public int Id { get; set; }

    [Required]
    public int CandidateApplicationId { get; set; }

    [ForeignKey(nameof(CandidateApplicationId))]
    public CandidateApplication? CandidateApplication { get; set; }

    [Required]
    [StringLength(100)]
    public string Stage { get; set; } = string.Empty;

    [Required]
    public DateTime ScheduledDateTime { get; set; }

    [Range(15, 240)]
    public int DurationMinutes { get; set; } = 60;

    [Required]
    [StringLength(50)]
    public string Mode { get; set; } = string.Empty;

    [Required]
    [StringLength(450)]
    public string InterviewerEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Scheduled";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}