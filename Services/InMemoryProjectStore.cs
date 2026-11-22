using System.Collections.Concurrent;
using Pms.Api.Models;

namespace Pms.Api.Services;

/// <summary>
/// A simple process-local store for practice. Data resets when the API stops.
/// Replace this class with a database-backed repository in a larger application.
/// </summary>
public sealed class InMemoryProjectStore
{
    private readonly ConcurrentDictionary<Guid, ProjectItem> _projects = new();

    public InMemoryProjectStore()
    {
        var first = new ProjectItem(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Practice API",
            "Build a first REST API with ASP.NET Core.",
            false,
            DateTime.UtcNow.AddDays(14));
        var second = new ProjectItem(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Read the documentation",
            "Learn the basics of HTTP and JSON.",
            true,
            null);

        _projects.TryAdd(first.Id, first);
        _projects.TryAdd(second.Id, second);
    }

    public IReadOnlyCollection<ProjectItem> GetAll() =>
        _projects.Values.OrderBy(project => project.Title).ToArray();

    public bool TryGet(Guid id, out ProjectItem? project) => _projects.TryGetValue(id, out project);

    public ProjectItem Add(ProjectRequest request)
    {
        var project = new ProjectItem(Guid.NewGuid(), request.Title.Trim(), request.Description?.Trim(), request.IsCompleted, request.DueDate);
        _projects[project.Id] = project;
        return project;
    }

    public bool TryUpdate(Guid id, ProjectRequest request, out ProjectItem? updated)
    {
        updated = null;
        if (!_projects.TryGetValue(id, out var current))
        {
            return false;
        }

        updated = new ProjectItem(id, request.Title.Trim(), request.Description?.Trim(), request.IsCompleted, request.DueDate);
        if (_projects.TryUpdate(id, updated, current))
        {
            return true;
        }

        updated = null;
        return false;
    }

    public bool Delete(Guid id) => _projects.TryRemove(id, out _);
}
