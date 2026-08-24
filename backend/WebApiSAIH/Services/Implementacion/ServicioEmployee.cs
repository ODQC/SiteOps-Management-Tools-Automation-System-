using AspNetIdentityDemo.Api.Models;
using AspNetIdentityDemo.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioEmployee : IServicioEmployee
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private ISendGridEmailService _sendGridService;

        public ServicioEmployee(UserManager<IdentityUser> userManager, IConfiguration configuration,
            ApplicationDbContext context, IMapper mapper, RoleManager<IdentityRole> roleManager, ISendGridEmailService sendGridService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _sendGridService = sendGridService;
        }

        private const string MensajeForgetPasswordGenerico = "If the email is registered, a password reset link will be sent";

        public async Task<RespuestaGenerica> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal whether the email exists (avoids employee enumeration):
                // respond the same way as the success case, without sending any email.
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = MensajeForgetPasswordGenerico,
                    Object = null
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_configuration["AppAngular"]}/reset?email={email}&token={validToken}";

            EmailTemplate emailTemplate = _context.EmailTemplates.Where(s => s.Name == "Change Password").FirstOrDefault<EmailTemplate>();

            string htmlString = emailTemplate.Content;

            Employee employee = _context.Employees.Where(
                 s => s.Email == email).FirstOrDefault<Employee>();

            htmlString = Regex.Replace(htmlString, "EMPLOYEE FULL NAME", employee.FirstName + " "
                + employee.LastName);

            htmlString = Regex.Replace(htmlString, "URL", url);

            await _sendGridService.SendEmailAsyn(email, "Password Recovery", htmlString);

            Email emailToSave = new Email();
            emailToSave.De = "noreply@example.com";
            emailToSave.Para = email;
            emailToSave.Email_template_id = emailTemplate.PK_idTemplate;
            emailToSave.Fecha = DateTime.Now;

            _context.Emails.Add(emailToSave);
            _context.SaveChanges();

            return new RespuestaGenerica
            {
                Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                Mensaje = MensajeForgetPasswordGenerico,
                Object = null
            };
        }

        public async Task<RespuestaGenerica> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "Incorrect email or password",
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Object = null
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new RespuestaGenerica
                {
                    Mensaje = "Incorrect email or password",
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Object = null
                };

            Employee employee = _context.Employees.Where(
                 s => s.Email == model.Email).FirstOrDefault<Employee>();

            var claims = new List<Claim>
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("Cedula", employee?.NationalId ?? ""),
                new Claim("UserID", employee?.PK_idEmployee.ToString() ?? "0"),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ConfiguracionJWT:JWT_Secret"]));

            var token = new JwtSecurityToken(
                claims: claims,
                // Bounded exposure window: since there's no server-side token revocation
                // (no logout, no refresh tokens), 8h limits the blast radius of a stolen token.
                expires: DateTime.Now.AddHours(8),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            if(employee.Status == Status.INACTIVE)
            {
                return new RespuestaGenerica
                {
                    Mensaje = "This employee is inactive",
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Object = null
                };
            }

            EmailTemplate emailTemplate = _context.EmailTemplates.Where(s => s.Name == "Login").FirstOrDefault<EmailTemplate>();

            string htmlString = emailTemplate.Content;

            htmlString = Regex.Replace(htmlString, "EMPLOYEE NAME", employee.FirstName);

            htmlString = Regex.Replace(htmlString, "ACCOUNT", model.Email);

            htmlString = Regex.Replace(htmlString, "DATETIME", DateTime.Now.ToString("dd-MM-yyyy") + ", " + 
                DateTime.Now.ToString("hh:mm:ss"));

            await _sendGridService.SendEmailAsyn(model.Email, "New login", htmlString);

            Email emailToSave = new Email();
            emailToSave.De = "noreply@example.com";
            emailToSave.Para = model.Email;
            emailToSave.Email_template_id = emailTemplate.PK_idTemplate;
            emailToSave.Fecha = DateTime.Now;

            _context.Emails.Add(emailToSave);
            _context.SaveChanges();

            return new RespuestaGenerica
            {
                Mensaje = tokenAsString,
                Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                Object = employee
            };
        }

        public async Task<RespuestaGenerica> RegisterUserAsync(RegisterViewModel model, long fkRole)
        {
            if (model == null)
                throw new NullReferenceException("Register Model is null");

            if (model.Password != model.ConfirmPassword)
                return new RespuestaGenerica
                {
                    Mensaje = "Password confirmation does not match the password",
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Object = true
                };

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);

            if (result.Succeeded)
            {
                EmailTemplate emailTemplate = _context.EmailTemplates.Where(s => s.Name == "Employee data").FirstOrDefault<EmailTemplate>();

                string htmlString = emailTemplate.Content;

                htmlString = Regex.Replace(htmlString, "EMAIL", model.Email);

                htmlString = Regex.Replace(htmlString, "PASSWORD", model.ConfirmPassword);

                await _sendGridService.SendEmailAsyn(model.Email, "Password SAIH", htmlString);

                Email emailToSave = new Email();
                emailToSave.De = "noreply@example.com";
                emailToSave.Para = model.Email;
                emailToSave.Email_template_id = emailTemplate.PK_idTemplate;
                emailToSave.Fecha = DateTime.Now;

                _context.Emails.Add(emailToSave);
                _context.SaveChanges();

                if (!await _roleManager.RoleExistsAsync(Roles.ROLE_SITE_MANAGER))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_SITE_MANAGER));
                }
                if (!await _roleManager.RoleExistsAsync(Roles.ROLE_ADMIN))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_ADMIN));
                }
                if (!await _roleManager.RoleExistsAsync(Roles.ROLE_EMPLOYEE))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_EMPLOYEE));
                }
                if (!await _roleManager.RoleExistsAsync(Roles.ROLE_SUPERVISOR))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.ROLE_SUPERVISOR));
                }

                Role role2 = _context.EmployeeRoles.Where(
                 s => s.PK_idRole == fkRole).FirstOrDefault<Role>();

                if (await _roleManager.RoleExistsAsync(role2.Name))
                {
                    await _userManager.AddToRoleAsync(identityUser, role2.Name);
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_CREATED,
                    Mensaje = "Employee registered successfully!",
                    Object = true,
                };
            }

            return new RespuestaGenerica
            {
                Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                Mensaje = "Employee not created",
                Object = result.Errors.Select(e => e.Description)
            };

        }

        public async Task<RespuestaGenerica> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                // Don't reveal whether the email exists (avoids employee enumeration).
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Mensaje = "The link is invalid or has expired",
                    Object = null
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Passwords do not match",
                    Object = null
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Password cambiada correctamente!",
                    Object = null,
                };

            return new RespuestaGenerica
            {
                Mensaje = "Algo paso",
                Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                Object = result.Errors.Select(e => e.Description),
            };
        }

        public async Task<RespuestaGenerica> createEmployee(EmployeeDTO employeeDTO)
        {
            try
            {
                Employee employee = _context.Employees.Where(
                 s => s.NationalId == employeeDTO.NationalId).FirstOrDefault<Employee>();

                if (employee != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This employee is already registered in the system", null);
                }

                Employee employee2 = _mapper.Map<Employee>(employeeDTO);

                if (validarCampos(employee2) != null)
                {
                    return validarCampos(employee2);
                }

                Role rolAdminParque = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_SITE_MANAGER).FirstOrDefault<Role>();

                if (employee2.FK_idRole1 == rolAdminParque.PK_idRole && administradoresParqueActivos(employee2.FK_idSite1))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "There is already an active manager for that site", _context.Employees.Where(
                        s => s.Status == Status.ACTIVE && s.FK_idRole1 == rolAdminParque.PK_idRole && s.FK_idSite1 == employee2.FK_idSite1));
                }

                String password = Utilidades.Utilidades.GenerarStringPassword();

                RegisterViewModel registerViewModel = new RegisterViewModel();
                registerViewModel.Email = employeeDTO.Email;
                registerViewModel.Password = password;
                registerViewModel.ConfirmPassword = password;

                RespuestaGenerica genericAnswer = new RespuestaGenerica();
                genericAnswer = await RegisterUserAsync(registerViewModel, employeeDTO.FK_idRole1);

                if (genericAnswer.Codigo == CodigosEstadoHTTP.HTTP_STATUS_CREATED)
                {
                    _context.Employees.Add(employee2);
                    _context.SaveChanges();
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Employee registered", _mapper.Map<EmployeeDTO>(employee2));
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Datos incorrectos", null);
                }

            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        private bool administradoresParqueActivos(long fk_site)
        {
            Role rolAdminParque = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_SITE_MANAGER).FirstOrDefault<Role>();

            Site site = _context.Sites.Where(
                s => s.PK_IdSite == fk_site).FirstOrDefault<Site>();

            List<Employee> employees = _context.Employees.Where(
                s => s.FK_idRole1 == rolAdminParque.PK_idRole && s.Status == Status.ACTIVE
                    && s.FK_idSite1 == site.PK_IdSite).ToList();

            if (employees.Count == 0)
            {
                return false;
            }

            return true;
        }

        private RespuestaGenerica validarCampos(Employee employee)
        {
            List<String> listaErrores = new List<String>();

            Employee employee2 = _context.Employees.Where(
                 s => s.Email == employee.Email).FirstOrDefault<Employee>();

            if (employee2 != null)
            {
                listaErrores.Add(Error.EMAIL_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica updateEmployee(int pK_idEmployee, EmployeeDTO employeeDTO)
        {
            if (pK_idEmployee != employeeDTO.PK_idEmployee)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Bad Request", "The request format is invalid");
            }

            try
            {
                Employee employee = _context.Employees.Where(
                 s => s.PK_idEmployee == pK_idEmployee).FirstOrDefault<Employee>();

                if (employee == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", "No employee was updated");
                }

                Employee employeeWithSameEmail = _context.Employees.Where(
                 s => s.Email == employeeDTO.Email).FirstOrDefault<Employee>();

                if (employeeWithSameEmail != employee)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This email is already in use", "");
                }

                employee = covertirDTOAEntidad(employee, employeeDTO);
                _context.Update(employee);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Employee updated", _mapper.Map<EmployeeDTO>(employee));
            }
            catch (Exception e)
            {
                if (!EmployeeExists(pK_idEmployee))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        public RespuestaGenerica updateOwnProfile(string cedula, ActualizarPerfilDTO perfilDTO)
        {
            try
            {
                Employee employee = _context.Employees.Where(
                 s => s.NationalId == cedula).FirstOrDefault<Employee>();

                if (employee == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", null);
                }

                employee.PhoneEmployee = perfilDTO.PhoneEmployee;

                if (perfilDTO.FK_idDocument1.HasValue)
                {
                    employee.FK_idDocument1 = perfilDTO.FK_idDocument1.Value;
                }

                _context.Update(employee);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Profile updated", _mapper.Map<EmployeeDTO>(employee));
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        private bool EmployeeExists(int pK_idEmployee)
        {
            return _context.Employees.Any(e => e.PK_idEmployee == pK_idEmployee);
        }

        private Employee covertirDTOAEntidad(Employee Employee, EmployeeDTO EmployeeDTO)
        {
            Employee.NationalId = EmployeeDTO.NationalId;
            Employee.Email = EmployeeDTO.Email;
            Employee.Status = EmployeeDTO.Status;
            Employee.FirstName = EmployeeDTO.FirstName;
            Employee.LastName = EmployeeDTO.LastName;
            Employee.SecondLastName = EmployeeDTO.SecondLastName;
            Employee.PhoneEmployee = EmployeeDTO.PhoneEmployee;
            Employee.FK_idDepartment1 = EmployeeDTO.FK_idDepartment1;
            Employee.FK_idSite1 = EmployeeDTO.FK_idSite1;
            Employee.FK_idRole1 = EmployeeDTO.FK_idRole1;
            Employee.FK_idDocument1 = EmployeeDTO.FK_idDocument1;

            return Employee;
        }

        public RespuestaGenerica getEmployee(string nationalId)
        {
            Employee employee = _context.Employees.Where(
                s => s.NationalId == nationalId).FirstOrDefault<Employee>();

            if (employee == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Employee found", _mapper.Map<EmployeeDTO>(employee));
        }

        public RespuestaGenerica getEmployees()
        {
            List<Employee> employees = _context.Employees.ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Employees listed", _mapper.Map<List<EmployeeDTO>>(employees));
        }

        public RespuestaGenerica toggleEmployeeStatus(string nationalId)
        {
            Employee employee = _context.Employees.Where(
                s => s.NationalId == nationalId).FirstOrDefault<Employee>();

            if (employee == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", null);
            }

            Role rolAdminParque = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_SITE_MANAGER).FirstOrDefault<Role>();

            Site site = _context.Sites.Where(
                s => s.PK_IdSite == employee.FK_idSite1).FirstOrDefault<Site>();

            if (rolAdminParque == null || site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", "Something went wrong");
            }

            if (employee.FK_idRole1 == rolAdminParque.PK_idRole && employee.Status != Status.ACTIVE)
            {
                List<Employee> employees = _context.Employees.Where(
                s => s.FK_idRole1 == rolAdminParque.PK_idRole && s.Status == Status.ACTIVE
                    && s.FK_idSite1 == site.PK_IdSite).ToList();

                if (employees.Count != 0)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_BAD_REQUEST, "Bad Request", "There is already an active site manager for that site");
                }
            }

            if (employee.Status == Status.ACTIVE)
            {
                employee.Status = Status.INACTIVE;
            }
            else if (employee.Status == Status.INACTIVE)
            {
                employee.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(employee);
                _context.SaveChanges(); 
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The employee's status has been changed", _mapper.Map<EmployeeDTO>(employee));
        }

        public async Task<RespuestaGenerica> deleteEmployee(string nationalId)
        {
            Employee employee = _context.Employees.Where(
                s => s.NationalId == nationalId).FirstOrDefault<Employee>();

            if (employee == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Employee not found", null);
            }

            try
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                var identityUser = await _userManager.FindByEmailAsync(employee.Email);

                await _userManager.DeleteAsync(identityUser);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Employee " + employee.NationalId + " deleted successfully", _mapper.Map<EmployeeDTO>(employee));
        }

        public RespuestaGenerica checkEmail(string email)
        {
            try
            {
                Employee employee = _context.Employees.Where(
                s => s.Email == email).FirstOrDefault<Employee>();

                if (employee != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Email en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Email disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica checkNationalId(string nationalId)
        {
            try
            {
                Employee employee = _context.Employees.Where(
                s => s.NationalId == nationalId).FirstOrDefault<Employee>();

                if (employee != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Cedula en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Cedula disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }
    }
}
