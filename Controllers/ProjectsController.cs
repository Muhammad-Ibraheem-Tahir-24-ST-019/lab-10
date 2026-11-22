using Microsoft.AspNetCore.Mvc;
using Pms.Api.Models;
using Pms.Api.Services;

namespace Pms.Api.Controllers;

/// <summary>CRUD endpoints for projects: /api/projects.</summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ProjectsController(InMemoryProjectStore store) : ControllerBase
{
    /// <summary>List all projects.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProjectItem>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyCollection<ProjectItem>> GetAll() => Ok(store.GetAll());

    /// <summary>Get one project by its ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectItem), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProjectItem> GetById(Guid id)
    {
        return store.TryGet(id, out var project)
            ? Ok(project)
            : NotFoundProblem($"No project with ID '{id}' was found.");
    }

    /// <summary>Create a project. Invalid request bodies automatically return HTTP 400.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectItem), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ProjectItem> Create(ProjectRequest request)
    {
        var project = store.Add(request);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    /// <summary>Replace all editable fields of a project.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProjectItem), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProjectItem> Update(Guid id, ProjectRequest request)
    {
        return store.TryUpdate(id, request, out var project)
            ? Ok(project)
            : NotFoundProblem($"No project with ID '{id}' was found.");
    }

    /// <summary>Delete a project.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        return store.Delete(id)
            ? NoContent()
            : NotFoundProblem($"No project with ID '{id}' was found.");
    }

    private NotFoundObjectResult NotFoundProblem(string detail) =>
        NotFound(new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Project not found",
            Detail = detail,
            Instance = HttpContext.Request.Path
        });
}
