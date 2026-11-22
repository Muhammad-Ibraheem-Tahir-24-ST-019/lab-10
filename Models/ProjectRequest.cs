using System.ComponentModel.DataAnnotations;

namespace Pms.Api.Models;

/// <summary>
/// JSON body used for both creating and replacing a project.
/// Example: { "title": "Prepare lecture", "description": "Draft the slides", "isCompleted": false }
/// </summary>
public sealed class ProjectRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    // Send a date/time string in ISO 8601 format, for example "2026-12-01T09:00:00Z".
    public DateTime? DueDate { get; set; }
}
