using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.Interfaces;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly IServicioDepartment _departmentService;

        public DepartmentController(IServicioDepartment departmentService)
        {

            _departmentService = departmentService;

        }

        [HttpGet]
        public IActionResult obtenerDepartments()
        {
            return Ok(_departmentService.obtenerDepartments());
        }

        [HttpGet("departmentNA")]
        public IActionResult obtenerPKDepartmentNA()
        {
            RespuestaGenerica respuestaGenerica = _departmentService.obtenerPKDepartmentNA();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{codigoDepartment}")]
        public IActionResult obtenerDepartment(string codigoDepartment)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.obtenerDepartment(codigoDepartment);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarDepartment(DepartmentDTO departmentDTO)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.guardarDepartment(departmentDTO);

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

        [HttpPut("{pk_IdDepartment}")]
        public IActionResult modificarDepartment(long pk_IdDepartment, DepartmentDTO departmentDTO)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.modificarDepartment(pk_IdDepartment, departmentDTO);

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

        [HttpDelete("{codigoDepartment}")]
        public IActionResult eliminarDepartment(string codigoDepartment)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.eliminarDepartment(codigoDepartment);

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

        [HttpGet("cambiarEstado/{codigoDepartment}")]
        public IActionResult deshabilitarDepartment(string codigoDepartment)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.deshabilitarDepartment(codigoDepartment);

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

        [HttpGet("verificarDepartment/{codigoDepartment}")]
        public IActionResult verificarDepartment(string codigoDepartment)
        {
            RespuestaGenerica respuestaGenerica = _departmentService.verificarDepartment(codigoDepartment);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}