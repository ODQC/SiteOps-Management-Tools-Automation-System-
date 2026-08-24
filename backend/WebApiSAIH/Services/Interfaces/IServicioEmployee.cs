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
        Task<RespuestaGenerica> guardarEmployee(EmployeeDTO employeeDTO);

        RespuestaGenerica obtenerEmployee(string employeeCedula);

        RespuestaGenerica obtenerEmployeerios();

        RespuestaGenerica deshabilitarEmployee(string employeeCedula);

        Task<RespuestaGenerica> eliminarEmployee(string employeeCedula);

        RespuestaGenerica modificarEmployee(int pK_idEmployee, EmployeeDTO employeeDTO);

        RespuestaGenerica actualizarPerfilPropio(string cedula, ActualizarPerfilDTO perfilDTO);

        RespuestaGenerica verificarEmail(string email);

        RespuestaGenerica verificarCedula(string employeeCedula);
    }
}
