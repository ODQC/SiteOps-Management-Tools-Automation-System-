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
        public async Task<IActionResult> createEmployee(EmployeeDTO employeeDTO)
        {
            RespuestaGenerica respuestaGenerica = await _employeeService.createEmployee(employeeDTO);

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
        public IActionResult getEmployees()
        {
            return Ok(_employeeService.getEmployees());
        }

        [HttpGet("{nationalId}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," +
            Roles.ROLE_ADMIN + "," + Roles.ROLE_EMPLOYEE)]
        public IActionResult getEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.getEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("checkEmail/{email}")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public IActionResult checkEmail(string email)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.checkEmail(email);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500,respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("checkNationalId/{nationalId}")]
        [Authorize(Roles = Roles.ROLE_ADMIN)]
        public IActionResult checkNationalId(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.checkNationalId(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPut("{pK_idEmployee}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
            Roles.ROLE_ADMIN)]
        public IActionResult updateEmployee(int pK_idEmployee, EmployeeDTO employeeDTO)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.updateEmployee(pK_idEmployee, employeeDTO);

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

        [HttpPut("my-profile")]
        [Authorize]
        public IActionResult updateMyProfile(ActualizarPerfilDTO perfilDTO)
        {
            string nationalId = User.FindFirst("Cedula")?.Value;

            if (string.IsNullOrEmpty(nationalId))
            {
                return Unauthorized();
            }

            RespuestaGenerica respuestaGenerica = _employeeService.updateOwnProfile(nationalId, perfilDTO);

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
        public async Task<IActionResult> deleteEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = await _employeeService.deleteEmployee(nationalId);

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

        [HttpGet("toggle-status/{nationalId}")]
        [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + ","  +
            Roles.ROLE_ADMIN)]
        public IActionResult toggleEmployeeStatus(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _employeeService.toggleEmployeeStatus(nationalId);

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