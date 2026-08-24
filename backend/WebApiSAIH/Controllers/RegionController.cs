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
    public class RegionController : ControllerBase
    {
        private readonly IServicioRegion _servicioRegion;
        public RegionController(IServicioRegion servicioRegion)
        {
            _servicioRegion = servicioRegion;
        }

        [HttpGet]
        public IActionResult obtenerRegion()
        {
            return Ok(_servicioRegion.obtenerAreasConservacion());
        }

        [HttpGet("regionNA")]
        public IActionResult obtenerPKRegionNA()
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.obtenerPKRegionNA();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{code}")]
        public IActionResult obtenerRegion(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.obtenerRegion(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarRegion(RegionDTO regionDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.guardarRegion(regionDTO);

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

        [HttpPut("{pkIdRegion}")]
        public IActionResult actualizarRegion(int pkIdRegion, RegionDTO regionDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.actualizarRegion(pkIdRegion, regionDTO);

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
        public IActionResult eliminarRegion(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.eliminarRegion(code);

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
        public IActionResult deshabilitarRegion(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.deshabilitarRegion(code);

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

        [HttpGet("verificarArea/{code}")]
        public IActionResult verificarArea(string code)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.verificarArea(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("obtenerAreaXparque/{pk_idParque}")]
        public IActionResult obtenerAreaXparque(long pk_idParque)
        {
            RespuestaGenerica respuestaGenerica = _servicioRegion.obtenerAreaXparque(pk_idParque);

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
    }
}