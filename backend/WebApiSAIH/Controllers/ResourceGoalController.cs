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
        public IActionResult createResourceGoal(ResourceGoalDTO resourceGoalDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.createResourceGoal(resourceGoalDTO);

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
        public IActionResult deleteResourceGoal(long pk_idResourceGoal)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.deleteResourceGoal(pk_idResourceGoal);

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

        [HttpPost("createBatch")]
        public IActionResult createResourceGoalList(List<ResourceGoalDTO> listResourcegoalDTO)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.createResourceGoalList(listResourcegoalDTO);

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

        [HttpGet("checkExistingLink/{fk_idResource2}/{fk_idGoal2}")]
        public IActionResult checkExistingLink(long fk_idResource2, long fk_idGoal2)
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.checkExistingLink(fk_idResource2, fk_idGoal2);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }

        [HttpGet]
        public IActionResult getResourceGoals()
        {
            RespuestaGenerica respuestaGenerica = _resourceGoalService.getResourceGoals();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }
    }
}