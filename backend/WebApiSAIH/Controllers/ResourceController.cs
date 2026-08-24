using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResourceController : ControllerBase
    {
        private readonly IServicioResource _resourceService;

        public ResourceController(IServicioResource resourceService)
        {

            _resourceService = resourceService;

        }

        [HttpGet]
        public IActionResult obtenerResources()
        {
            return Ok(_resourceService.obtenerResources());
        }

        [HttpGet("resourceNA")]
        public IActionResult obtenerPKResourceNA()
        {
            RespuestaGenerica respuestaGenerica = _resourceService.obtenerPKResourceNA();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{code}")]
        public IActionResult obtenerResource(string code)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.obtenerResource(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarResource(ResourceDTO resourceDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.guardarResource(resourceDTO);

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

        [HttpPut("{pK_idResource}")]
        public IActionResult modificarResource(long pK_idResource, ResourceDTO resourceDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.modificarResource(pK_idResource, resourceDTO);

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
        public IActionResult eliminarResource(string code)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.eliminarResource(code);

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
        public IActionResult deshabilitarResource(string code)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.deshabilitarResource(code);

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

        [HttpGet("goalsXresource/{code}")]
        public IActionResult goalsXresource(string code)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.goalsXresource(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("verificarResource/{code}")]
        public IActionResult verificarResource(string code)
        {
            RespuestaGenerica respuestaGenerica = _resourceService.verificarResource(code);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}