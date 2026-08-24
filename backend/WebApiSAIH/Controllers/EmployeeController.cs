using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;

namespace SAIH_Backend.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IServicioEmployee _employeeService;

        public EmployeeController(IServicioEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public async Task<IActionResult> guardarEmployee(EmployeeDTO employeeDTO)
        {
            RespuestaGenerica respuestaGenerica = await _employeeService.guardarEmployee(employeeDTO);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return StatusCode(200, respuestaGenerica);
            }

            return StatusCode(201, respuestaGenerica);
        }

        [HttpGet]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," +
            Roles.ROLE_ADMIN + "," + Roles.ROLE_EMPLOYEE)]
        public IActionResult obtenerEmployeerios()
        {
            return Ok(_employeeService.obtenerEmployeerios());
        }

        [HttpGet("{nationalId}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," +
            Roles.ROLE_ADMIN + "," + Roles.ROLE_EMPLOYEE)]
        public IActionResult obtenerEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.obtenerEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("verificarEmail/{email}")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public IActionResult verificarEmail(string email)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.verificarEmail(email);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500,respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("verificarCedula/{employeeCedula}")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public IActionResult verificarCedula(string employeeCedula)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.verificarCedula(employeeCedula);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPut("{pK_idEmployee}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
            Roles.ROLE_ADMIN)]
        public IActionResult modificarEmployee(int pK_idEmployee, EmployeeDTO employeeDTO)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.modificarEmployee(pK_idEmployee, employeeDTO);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPut("mi-perfil")]
        [Authorize]
        public IActionResult actualizarMiPerfil(ActualizarPerfilDTO perfilDTO)
        {
            string cedula = User.FindFirst("Cedula")?.Value;

            if (string.IsNullOrEmpty(cedula))
            {
                return Unauthorized();
            }

            RespuestaGenerica respuestaGenerica = _employeeService.actualizarPerfilPropio(cedula, perfilDTO);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpDelete("{nationalId}")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public async Task<IActionResult> eliminarEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = await _employeeService.eliminarEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("cambiarEstado/{nationalId}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + ","  +
            Roles.ROLE_ADMIN)]
        public IActionResult deshabilitarEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.deshabilitarEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}