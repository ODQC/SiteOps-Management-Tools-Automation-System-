using AspNetIdentityDemo.Api.Models;
using AspNetIdentityDemo.Shared;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioEmployee
    {
        Task<RespuestaGenerica> RegisterUserAsync(RegisterViewModel model, long fkRole);

        Task<RespuestaGenerica> LoginUserAsync(LoginViewModel model);

        Task<RespuestaGenerica> ForgetPasswordAsync(string email);

        Task<RespuestaGenerica> ResetPasswordAsync(ResetPasswordViewModel model);
        Task<RespuestaGenerica> createEmployee(EmployeeDTO employeeDTO);

        RespuestaGenerica getEmployee(string nationalId);

        RespuestaGenerica getEmployees();

        RespuestaGenerica toggleEmployeeStatus(string nationalId);

        Task<RespuestaGenerica> deleteEmployee(string nationalId);

        RespuestaGenerica updateEmployee(int pK_idEmployee, EmployeeDTO employeeDTO);

        RespuestaGenerica updateOwnProfile(string nationalId, ActualizarPerfilDTO perfilDTO);

        RespuestaGenerica checkEmail(string email);

        RespuestaGenerica checkNationalId(string nationalId);
    }
}
