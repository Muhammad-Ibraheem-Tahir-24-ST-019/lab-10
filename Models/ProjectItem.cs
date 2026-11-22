namespace Pms.Api.Models;

/// <summary>A project record returned by the API.</summary>
public sealed record ProjectItem(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTime? DueDate);
