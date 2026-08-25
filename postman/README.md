# Postman collection — SiteOps Backend

Covers all 134 endpoints across the backend's 16 controllers (`WebApiSAIH`), with one test per
endpoint that checks the HTTP status code and, where applicable, the response shape
(`{codigo, mensaje, object}` from `RespuestaGenerica`).

## How to use it

1. Import both files into Postman: `SiteOps-Backend.postman_collection.json` and
   `SiteOps-Backend.postman_environment.json`. Select the "SiteOps Backend - Local" environment.
2. Start the backend one of two ways:
   - **Everything in Docker** (`docker compose up -d` from the repo root) — backend at
     `http://localhost:5080`. Change `baseUrl` in the environment to that value.
   - **Local backend + only the database in Docker** (`docker compose up -d sqlserver` from the
     repo root, then `dotnet run --urls http://localhost:5000` from `backend/WebApiSAIH`) —
     backend at `http://localhost:5000`, which is the environment's default `baseUrl`.
3. Set `loginEmail` / `loginPassword` in the environment to a real user in your database (the
   `SeedTool` demo accounts work, e.g. `admin.demo@example.com` / `Prueba1234`).
4. Run **Auth → Login** first: it saves the JWT into the `token` variable automatically, which
   the rest of the collection uses via inherited Bearer auth.
5. Many "get/update/delete by id" endpoints use environment variables (`{{actividadId}}`,
   `{{codigoArea}}`, etc.) with example values (`1`, `AC-01`, ...). Point those at ids that exist
   in your database, or run each folder's creation `POST` first and copy the returned id.

## The real admin account is protected from automated runs

The **Delete** and **Toggle status** tests under `Employee`, `Role`, `Region`, `Department`, and
`Site` use `*Descartable` variables (`{{nationalIdEmployeeDescartable}}`,
`{{codigoRolDescartable}}`, etc.) that by default **don't exist** in the database, instead of the
real variables (`{{nationalIdEmployee}}` = `1-2345-6789`, `{{codigoRol}}` = `ROL-01`, ...) that
point at the demo admin account and its reference data (role, department, site, region). This
way, running the whole collection (*Run collection*) never deletes or deactivates the account you
use to log in — those requests simply get a 404 against a nonexistent id, which is still a valid
test of "not found" handling.

To actually exercise the delete/toggle flow, point the matching `*Descartable` variable at an id
you created yourself with that folder's `POST`.

## Running it all from the terminal (Newman)

```bash
npm install -g newman
newman run SiteOps-Backend.postman_collection.json -e SiteOps-Backend.postman_environment.json
```

## Real backend bugs found and fixed with this collection

1. **`Document / Download document by id`** (`GET /api/Document/{id}`) — if the id didn't exist,
   the service didn't null-check before using `file.FilePath`, throwing an unhandled exception
   (500) instead of returning 404. **Fixed**: it now validates that the record and the physical
   file exist before reading them, and the controller respects the returned code (404/500)
   instead of assuming success.
2. **`Deliverable / Get deliverable by PK`** (`GET /api/Deliverable/{id}`) — the service used the
   deliverable's document FK **before** checking `if (deliverable == null)`, so a nonexistent id
   threw a NullReferenceException (500) instead of 404. **Fixed**: the null check now runs first.
3. **Seven `GET .../xNA` routes** (`taskNA`, `regionNA`, `departmentNA`, `resourceNA`, `goalNA`,
   `siteNA`, `projectNA`) called `NotFound(...)` **without `return`**, so the method kept
   executing and always responded 200, never 404. **Fixed**: all seven now return immediately
   on the not-found path.
4. **Nine `Estadistica` routes were missing the `/` separator** before their route parameter
   (e.g. `completedTasks{name}` instead of `completedTasks/{name}`), and one handler's parameter
   name (`nombreAsp`) didn't match its route placeholder (`{siteName}`). **Fixed**: all nine
   routes now have a proper separator, and the parameter name matches its placeholder.

## Behavior notes (not bugs, by design)

- `POST /api/Auth/ResetPassword` with invalid data doesn't return `RespuestaGenerica`: the
  `[ApiController]` attribute short-circuits execution before the controller's own code runs,
  returning its own default `ValidationProblemDetails` instead.
