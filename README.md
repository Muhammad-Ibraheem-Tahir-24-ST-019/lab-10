# PMS — beginner ASP.NET Core REST API

A small **P**roject **M**anagement **S**ystem practice API built with ASP.NET Core 8 controllers. It demonstrates CRUD routes, request validation, HTTP status codes, and a simple in-memory store without requiring a database or extra NuGet packages.

## Run it

Install the .NET 8 SDK, then from this directory run:

```bash
dotnet run
```

The console prints the local HTTP address to use. You can also choose one explicitly:

```bash
dotnet run --urls http://localhost:5000
```

The sample projects are loaded at startup. The in-memory data is reset when the application stops.

## Routes

| Method | Route | Success response |
| --- | --- | --- |
| `GET` | `/api/projects` | `200 OK` with all projects |
| `GET` | `/api/projects/{id}` | `200 OK` with one project; `404 Not Found` if missing |
| `POST` | `/api/projects` | `201 Created` with the new project and a `Location` header |
| `PUT` | `/api/projects/{id}` | `200 OK` with the replaced project; `404 Not Found` if missing |
| `DELETE` | `/api/projects/{id}` | `204 No Content`; `404 Not Found` if missing |

IDs in the route must be GUIDs. `POST` and `PUT` expect a title with 2–100 characters; description is optional and limited to 500 characters. Invalid JSON fields are automatically reported as `400 Bad Request` validation details by `[ApiController]`.

## Try it with curl

List sample projects:

```bash
curl http://localhost:5000/api/projects
```

Create a project:

```bash
curl -i -X POST http://localhost:5000/api/projects \
  -H 'Content-Type: application/json' \
  -d '{"title":"Prepare lecture","description":"Draft the slides","isCompleted":false,"dueDate":"2026-12-01T09:00:00Z"}'
```

Copy the returned `id` to read, replace, or delete that project:

```bash
curl http://localhost:5000/api/projects/11111111-1111-1111-1111-111111111111

curl -i -X PUT http://localhost:5000/api/projects/11111111-1111-1111-1111-111111111111 \
  -H 'Content-Type: application/json' \
  -d '{"title":"Updated lecture","description":"Slides are ready","isCompleted":true,"dueDate":null}'

curl -i -X DELETE http://localhost:5000/api/projects/11111111-1111-1111-1111-111111111111
```

Example project JSON:

```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "title": "Practice API",
  "description": "Build a first REST API with ASP.NET Core.",
  "isCompleted": false,
  "dueDate": "2026-12-06T12:00:00Z"
}
```

## Where to start exploring

- `Program.cs` registers the controller and in-memory store.
- `Controllers/ProjectsController.cs` maps HTTP methods and routes to CRUD operations and status codes.
- `Models/ProjectRequest.cs` demonstrates validation attributes for incoming JSON.
- `Services/InMemoryProjectStore.cs` keeps sample data and CRUD logic in one place.

This is intentionally a learning example: the store is process-local and has no authentication or database persistence. A real application would typically add a database and appropriate security.
