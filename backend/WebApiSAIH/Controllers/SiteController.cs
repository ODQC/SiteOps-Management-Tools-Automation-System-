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
    public class SiteController : ControllerBase
    {
        private readonly IServicioSite _servicioSite;
        public SiteController(IServicioSite ServicioSite)
        {
            _servicioSite = ServicioSite;
        }

        [HttpGet("{idRegion}")]
        public IActionResult obtenerParquesNacional(int idRegion)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.obtenerParquesNacionales(idRegion);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("parqueNA")]
        public IActionResult obtenerPKParqueNA()
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.obtenerPKParqueNA();

            if(respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet]
        public IActionResult obtenerTodosParquesNacionales()
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.obtenerTodosParquesNacionales();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("parque/{code}")]
        public IActionResult obtenerSite(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.obtenerSite(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarSite(SiteDTO siteDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.guardarSite(siteDTO);

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

        [HttpPut("{pk_IdSite}")]
        public IActionResult modificarSite(long pk_IdSite, SiteDTO siteDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.modificarSite(pk_IdSite, siteDTO);

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
        public IActionResult eliminarSite(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.eliminarSite(code);

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
        public IActionResult deshabilitarSite(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.deshabilitarSite(code);

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

        [HttpGet("verificarParque/{code}")]
        public IActionResult verificarParque(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.verificarParque(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("verificarAdminParque/{code}")]
        public IActionResult verificarAdminParque(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioSite.verificarAdminParque(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}