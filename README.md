# SiteOps

A full-stack application for managing an organization's sites, departments, resources, and field work. Employees register regions, sites, departments, goals, and projects, track tasks and their deliverables, with role-based access control throughout.

## Stack

**Backend** — ASP.NET Core 8, Entity Framework Core + SQL Server, ASP.NET Identity with JWT authentication, AutoMapper, SendGrid for email notifications (password recovery).

**Frontend** — Angular 22 with Angular Material (Material Design 3), role-based permissions at both the menu and CRUD-action level.

**Infrastructure** — Docker Compose to bring up SQL Server, backend, and frontend with a single command.

## Structure

```
backend/WebApiSAIH/    REST API — 16 controllers, ~134 endpoints
backend/SeedTool/       .NET console app to seed demo data and test users
frontend/               Angular Material SPA
postman/                Postman/Newman collection: one test per endpoint
```

## Requirements

To run the project with Docker (recommended, works the same on macOS, Windows, and Linux):

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (on Windows, with the WSL2 backend enabled)
- [Git](https://git-scm.com/downloads)

The .NET 8 SDK and Node.js are only needed if you want to run the backend or frontend outside of Docker (see below).

## Features

- **Regions** and **sites**: create, edit, browse
- **Goals** and **projects**, with their associated **tasks**
- **Resources** and assigning resources to goals/tasks
- **Deliverables**: upload and download files per task
- **Employees and roles**: manage employees, roles, and permissions; the frontend menu and available actions adjust based on the signed-in role
- **Statistics** by site and task
- **Authentication**: JWT login, email-based password recovery flow

## Running it

```bash
git clone https://github.com/<your-username>/SiteOps.git
cd SiteOps
docker compose up -d
```

The same commands work on macOS and Windows (PowerShell, CMD, or WSL) once Docker Desktop is installed and running — nothing in `docker-compose.yml` is platform-specific.

This brings up SQL Server, the backend at `http://localhost:5080`, and the frontend at `http://localhost:4200`.

To develop just the backend against the containerized database:

```bash
docker compose up -d sqlserver
cd backend/WebApiSAIH && dotnet run --urls http://localhost:5000
```

Database, JWT, and SendGrid credentials are read from environment variables (`MSSQL_SA_PASSWORD`, `JWT_SECRET`, `SENDGRID_API_KEY`) with development defaults — see `docker-compose.yml`.

To develop the frontend with hot reload instead of the Docker build (requires Node ≥22.22.3, 24.15, or 26):

```bash
cd frontend
npm install
npm start   # ng serve, serves on http://localhost:4200 pointing at environment.ts
```

## Demo data

`backend/SeedTool` is a separate console app (not run in production) that creates regions, sites, roles, and one test user per role against the Docker database. With `sqlserver` running (`docker compose up -d sqlserver`):

```bash
cd backend/SeedTool
dotnet run
```

It's idempotent — safe to run as many times as needed, it won't duplicate data. It creates these accounts (all with password `Prueba1234`):

| Role | Email |
|---|---|
| Admin | `admin.demo@example.com` |
| Site Manager | `sitemanager.demo@example.com` |
| Employee | `employee.demo@example.com` |
| Supervisor | `supervisor.demo@example.com` |

## API testing

The Postman collection in [`postman/`](postman/) covers all 134 endpoints with one test per endpoint, and can be run end-to-end from the terminal with Newman:

```bash
npm install -g newman
newman run postman/SiteOps-Backend.postman_collection.json -e postman/SiteOps-Backend.postman_environment.json
```

## Engineering highlights

- **Security audit**: reviewed the login/auth flow and closed an unauthenticated privilege-escalation path, mitigated user-enumeration in the password-recovery flow, hardened JWT expiration handling, and removed a hardcoded SMTP credential that was committed in plaintext.
- **Bug-hunting via the test suite**: running the full Postman collection against a live instance surfaced several real defects — routes and DTO field names that had drifted from what the controllers actually expose, a route missing its path separator, and three private methods silently sharing one name (only the last definition was ever reachable) — all fixed and re-verified with a clean end-to-end run.
- **Consistent domain modeling**: the entity/role vocabulary (Region → Site → Department, Goal → Project → Task → Deliverable) is applied uniformly across the database schema, DTOs, API routes, and frontend, with a single generic 4-role permission model reused across every module.

## Roadmap

A planned next phase adds an AI assistant module on top of this same project: natural-language chat with tool-calling against the existing API (query projects/tasks/deliverables), plus retrieval over uploaded deliverables, applying the same security practices established here — role-aware access control, prompt-injection defenses on uploaded documents, and rate limiting on the LLM-backed endpoint.
