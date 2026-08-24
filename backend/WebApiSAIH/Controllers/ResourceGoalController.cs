using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ResourceGoalController : ControllerBase
    {
        private readonly IServicioResourceGoal _resourceGoalService;

        public ResourceGoalController(IServicioResourceGoal resourceGoalService)
        {

            _resourceGoalService = resourceGoalService;

        }

        [HttpPost]
        public IActionResult guardarResourceGoal(ResourceGoalDTO resourceGoalDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.guardarResourceGoal(resourceGoalDTO);

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

        [HttpDelete("{pk_idResourceGoal}")]
        public IActionResult eliminarResourceGoal(long pk_idResourceGoal)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.eliminarResourceGoal(pk_idResourceGoal);

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

        [HttpPost("guardarLista")]
        public IActionResult guardarResourceGoalList(List<ResourceGoalDTO> listResourcegoalDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.guardarResourceGoalList(listResourcegoalDTO);

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

        [HttpGet("verificarRelacionExistente/{fk_idResource2}/{fk_idGoal2}")]
        public IActionResult verificarRelacionExistente(long fk_idResource2, long fk_idGoal2)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.verificarRelacionExistente(fk_idResource2, fk_idGoal2);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }

        [HttpGet]
        public IActionResult obtenerResourcesGoal()
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.obtenerResourcesGoal();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }
    }
}