using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_Estaticas;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;

const string connectionString = "Server=localhost,1433;Database=SAIH-Dev;User Id=sa;Password=SaihDev#2026Local;TrustServerCertificate=True";

var services = new ServiceCollection();
services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));
services.AddLogging();
services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var sp = scope.ServiceProvider;

var db = sp.GetRequiredService<ApplicationDbContext>();
var userManager = sp.GetRequiredService<UserManager<IdentityUser>>();
var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();

foreach (var rol in new[] { Roles.ROLE_SITE_MANAGER, Roles.ROLE_ADMIN, Roles.ROLE_EMPLOYEE, Roles.ROLE_SUPERVISOR })
{
    if (!await roleManager.RoleExistsAsync(rol))
        await roleManager.CreateAsync(new IdentityRole(rol));
}

// ---- Reference data (areas, parks, departments) ----
Region GetOrCreateArea(string codigo, string nombre)
{
    var a = db.Regions.FirstOrDefault(x => x.Code == codigo);
    if (a == null)
    {
        a = new Region(0, codigo, nombre, $"{nombre} (prueba)", Status.ACTIVE);
        db.Regions.Add(a);
        db.SaveChanges();
    }
    return a;
}

Department GetOrCreateDepto(string codigo, string nombre)
{
    var d = db.Departments.FirstOrDefault(x => x.CodigoDepartment == codigo);
    if (d == null)
    {
        d = new Department(0, codigo, nombre, $"{nombre} (prueba)", Status.ACTIVE);
        db.Departments.Add(d);
        db.SaveChanges();
    }
    else if (d.NombreDepartment != nombre)
    {
        d.NombreDepartment = nombre;
        d.DescripcionDepartment = $"{nombre} (prueba)";
        db.SaveChanges();
    }
    return d;
}

Site GetOrCreateParque(string codigo, string nombre, long areaId)
{
    var p = db.Sites.FirstOrDefault(x => x.Code == codigo);
    if (p == null)
    {
        p = new Site(0, codigo, nombre, $"{nombre} (prueba)", Status.ACTIVE, areaId);
        db.Sites.Add(p);
        db.SaveChanges();
    }
    return p;
}

Role GetOrCreateRol(string codigo, string name)
{
    var r = db.EmployeeRoles.FirstOrDefault(x => x.Name == name);
    if (r == null)
    {
        r = new Role(0, codigo, name, $"{name} (prueba)", Status.ACTIVE);
        db.EmployeeRoles.Add(r);
        db.SaveChanges();
    }
    return r;
}

var areaCentral = GetOrCreateArea("AC-01", "Área Central");
var areaGuanacaste = GetOrCreateArea("AC-02", "Área de Conservación Guanacaste");

// Nombre alineado con el que espera el formulario de registro de employees
// (frontend/src/app/pages/formulario/formulario.component.ts, retornarPkDepartment).
var deptoTI = GetOrCreateDepto("DEP-01", "Department TI");
var deptoAdmin = GetOrCreateDepto("DEP-02", "Department Administración");
// Department "N/A" para roles que no tienen uno propio (Administrador de Parque,
// Guardaparque) -- lo busca ServicioDepartment.obtenerPKDepartmentNA por CodigoDepartment.
var deptoNA = GetOrCreateDepto("N/A", "N/A");

var parqueUno = GetOrCreateParque("PN-01", "Parque Nacional de Prueba", areaCentral.PK_IdRegion);
var parqueDos = GetOrCreateParque("PN-02", "Volcán Arenal", areaGuanacaste.PK_IdRegion);
// Parque "N/A" para roles que no tienen uno propio (Supervisor, Administrador TI) -- lo
// busca ServicioSite.obtenerPKParqueNA por Code.
var parqueNA = GetOrCreateParque("N/A", "N/A", areaCentral.PK_IdRegion);

var rolAdminTI = GetOrCreateRol("ROL-01", Roles.ROLE_ADMIN);
var rolAdminParque = GetOrCreateRol("ROL-02", Roles.ROLE_SITE_MANAGER);
var rolGuardaparque = GetOrCreateRol("ROL-03", Roles.ROLE_EMPLOYEE);
var rolSupervisor = GetOrCreateRol("ROL-04", Roles.ROLE_SUPERVISOR);

foreach (var name in new[] { "Login", "Cambiar Password", "Datos de employee" })
{
    if (!db.EmailTemplates.Any(t => t.Name == name))
    {
        db.EmailTemplates.Add(new EmailTemplate { Name = name, Content = "<p>Plantilla de prueba</p>" });
    }
}
db.SaveChanges();

// ---- Test users, one per role ----
const string password = "Prueba1234";

var employeesDePrueba = new[]
{
    new { Email = "admin.prueba@sinac.go.cr", Cedula = "1-2345-6789", Nombre = "Admin", Apellido1 = "Prueba", Apellido2 = "SAIH", Rol = rolAdminTI, Depto = deptoTI, Parque = parqueUno },
    new { Email = "parque.prueba@sinac.go.cr", Cedula = "2-3456-7890", Nombre = "Ana", Apellido1 = "Administradora", Apellido2 = "Parque", Rol = rolAdminParque, Depto = deptoTI, Parque = parqueUno },
    new { Email = "guarda.prueba@sinac.go.cr", Cedula = "3-4567-8901", Nombre = "Carlos", Apellido1 = "Guarda", Apellido2 = "Parque", Rol = rolGuardaparque, Depto = deptoTI, Parque = parqueDos },
    new { Email = "supervisor.prueba@sinac.go.cr", Cedula = "4-5678-9012", Nombre = "Sofía", Apellido1 = "Supervisora", Apellido2 = "SAIH", Rol = rolSupervisor, Depto = deptoAdmin, Parque = parqueUno },
};

foreach (var u in employeesDePrueba)
{
    var identityUser = await userManager.FindByEmailAsync(u.Email);
    if (identityUser == null)
    {
        identityUser = new IdentityUser { Email = u.Email, UserName = u.Email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(identityUser, password);
        if (!result.Succeeded)
        {
            Console.WriteLine($"Error creando {u.Email}: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            continue;
        }
    }

    if (!await userManager.IsInRoleAsync(identityUser, u.Rol.Name))
    {
        await userManager.AddToRoleAsync(identityUser, u.Rol.Name);
    }

    var employee = db.Employees.FirstOrDefault(x => x.Email == u.Email);
    if (employee == null)
    {
        employee = new Employee(
            u.Cedula,
            u.Nombre,
            u.Apellido1,
            u.Apellido2,
            "88880000",
            u.Email,
            Status.ACTIVE,
            (int)u.Depto.PK_idDepartment,
            (int)u.Parque.PK_IdSite,
            (int)u.Rol.PK_idRole,
            0);
        db.Employees.Add(employee);
        db.SaveChanges();
    }
}

Console.WriteLine("Listo. Employees de prueba (password para todos: Prueba1234):");
foreach (var u in employeesDePrueba)
{
    Console.WriteLine($"  {u.Rol.Name,-28} {u.Email}");
}
