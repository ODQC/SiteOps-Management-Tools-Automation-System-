using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAIH_Backend.Servicios.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SAIH_Backend.Servicios.VO;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.Clases_Estaticas;

namespace SAIH_Backend.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IServicioRole _roleService;
        public RoleController(IServicioRole roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult obtenerRoles()
        {
            return Ok(_roleService.obtenerRoless());
        }

        [HttpGet("{code}")]
        public IActionResult obtenerRole(string code)
        {
            RespuestaGenerica respuestaGenerica = _roleService.obtenerRole(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("obtenerRolPk/{pk_idRole}")]
        public IActionResult obtenerRole(long pk_idRole)
        {
            RespuestaGenerica respuestaGenerica = _roleService.obtenerRole(pk_idRole);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarRole(RoleDTO roleDTO)
        {
            RespuestaGenerica respuestaGenerica = _roleService.guardarRole(roleDTO);

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

        [HttpPut("{pK_idRole}")]
        public IActionResult modificarRole(long pK_idRole, RoleDTO roleDTO)
        {
            RespuestaGenerica respuestaGenerica = _roleService.modificarRole(pK_idRole, roleDTO);

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

        [HttpDelete("{code}")]
        public IActionResult eliminarRole(string code)
        {
            RespuestaGenerica respuestaGenerica = _roleService.eliminarRole(code);

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

        [HttpGet("cambiarEstado/{code}")]
        public IActionResult deshabilitarRole(string code)
        {
            RespuestaGenerica respuestaGenerica = _roleService.deshabilitarRole(code);

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