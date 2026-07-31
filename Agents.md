# HomeOrganizer Agent Instructions

This repository is a step-by-step learning project for building a .NET home organizer application. The owner is learning the .NET environment, ASP.NET Core, Entity Framework Core, SQL Server, and later React. Agents must support learning and project progress without replacing the learning process.

## Project Context

- Product: Home Organizer.
- Current direction: ASP.NET Core Web API first, then SQL Server/EF Core, then React + TypeScript frontend.
- Current backend shape: single controller-based API project in `HomeOrganizer.Api`.
- Current implementation stage: early API practice with in-memory item data.
- Main planning document: `Project_Schema.md`.
- Keep the project simple until the schema explicitly reaches later phases.

The intended final stack is:

- ASP.NET Core Web API
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Core Identity
- React
- TypeScript
- React Konva

## Primary Agent Rule

Do not skip the learner's work by generating large completed features unless the user explicitly asks for that. Prefer teaching, scaffolding, reviewing, and making small focused changes.

When implementing code:

- Explain what is being changed before editing.
- Keep changes small enough for a beginner to read and reproduce.
- Add comments only when they clarify a learning concept or a non-obvious decision.
- Avoid building full future phases early.
- Do not silently introduce advanced architecture.
- Do not replace a learning step with a large AI-generated solution.

When the user asks for help learning:

- Explain the concept first.
- Show the relevant local file.
- Suggest one small next exercise.
- If code is needed, provide or apply the minimum code for that exercise.

## Development Phases

Follow `Project_Schema.md` as the source of truth for project order.

Expected order:

1. ASP.NET Core project setup and controller basics.
2. Entity Framework Core and SQL Server.
3. Basic Home CRUD.
4. Rooms.
5. Basic React frontend.
6. Storage units.
7. Items.
8. Search.
9. Authentication and ownership checks.
10. Tests and deployment.

Agents must not jump to React, Identity, Clean Architecture, Docker, or advanced search until the current phase supports it or the user explicitly redirects.

## Coding Standards

Use simple, idiomatic C# suitable for a learning project.

- Keep one ASP.NET Core API project for now.
- Use controller-based REST APIs.
- Prefer clear names over clever abstractions.
- Use DTOs for request and response shapes.
- Do not return EF entities directly from controllers once EF Core is introduced.
- Keep validation close to the current learning level.
- Use dependency injection when services or `DbContext` are introduced.
- Avoid repository classes unless there is a concrete reason later.
- Use async EF Core methods when database access is introduced.
- Keep namespaces and folder names consistent.

Current local style notes:

- API project: `HomeOrganizer.Api`.
- Controllers live in `HomeOrganizer.Api/Controllers`.
- DTOs currently live in `HomeOrganizer.Api/DTO`.
- Existing sample routes are simple, such as `/items`.
- Match existing style unless the user asks for cleanup.

## Architecture Rules

Start simple and evolve only when the phase needs it.

Acceptable early flow:

```text
Controller -> in-memory data
```

Expected EF Core flow later:

```text
Controller -> Service -> ApplicationDbContext -> SQL Server
```

Do not introduce these early:

- Clean Architecture with many projects
- CQRS
- MediatR
- AutoMapper
- Generic repositories
- Unit of Work wrappers around EF Core
- Event-driven architecture
- 3D rendering
- Multi-user collaboration

If an agent believes one of these is necessary, it must explain the tradeoff and ask first.

## API Standards

For REST endpoints:

- Use route names that match the project schema.
- Return proper HTTP status codes.
- Use `ActionResult<T>` for typed controller results.
- Return `Created(...)` or `CreatedAtAction(...)` for successful POST requests.
- Return `NotFound()` when a requested resource does not exist.
- Return `BadRequest(...)` for invalid input at the current learning level.
- Keep request DTOs separate from response DTOs.

Validation examples:

- Name fields should not be empty.
- Width and height must be greater than zero.
- Quantity must be greater than zero.
- Search input should be trimmed before querying.

Security rule for later authenticated phases:

- Every database operation must verify ownership through the chain described in `Project_Schema.md`.
- A user must never access another user's home, room, storage unit, or item by changing an ID in the URL.

## Testing and Verification

Every code change should include a clear verification step.

For backend changes, prefer:

```bash
dotnet build
dotnet run
```

When tests exist, run:

```bash
dotnet test
```

For API behavior, use:

- `items.http` or other `.http` files
- Swagger/OpenAPI when enabled
- Direct HTTP requests when useful

Agents must report what was verified. If verification cannot be run, state why.

## Learning Workflow for Agents

When helping with a new concept, use this pattern:

1. State the goal of the step.
2. Explain the relevant .NET concept briefly.
3. Identify the file or files involved.
4. Make the smallest useful change.
5. Run or describe the verification.
6. Suggest the next learning step.

Good agent behavior:

- "Let's add one GET endpoint and test it."
- "This controller action maps HTTP GET to C# code."
- "Before EF Core, this list is only in memory and resets when the app restarts."

Bad agent behavior:

- Generating the full database, authentication, frontend, and search in one pass.
- Refactoring unrelated files.
- Introducing advanced patterns because they are common in production.
- Hiding important concepts behind helper libraries too early.

## Editing Rules

- Touch only files related to the user's current request.
- Do not reformat unrelated code.
- Do not rename folders or files unless asked.
- Do not delete user work unless explicitly requested.
- Preserve beginner-friendly code structure.
- Keep generated code short and readable.
- If a file contains typos or rough learning notes, only fix them when they are part of the requested task.

## Documentation Rules

Keep project documentation aligned with the learning phases.

- Update `Project_Schema.md` only when the user changes the project plan.
- Use `Agents.md` for agent behavior and standards.
- Use `.http` files for endpoint examples during API learning.
- Prefer short explanations near the relevant code over long abstract documentation.

## Current Near-Term Goal

The next major milestone from the schema is proving the backend request cycle:

```text
Client -> Controller -> Data storage -> JSON response
```

Before starting the 2D canvas, the backend should be able to:

- Create data through an API endpoint.
- Store it persistently after EF Core and SQL Server are introduced.
- Load it back through an API endpoint.
- Validate basic input.

Agents should keep work aligned with this milestone unless the user explicitly changes direction.
