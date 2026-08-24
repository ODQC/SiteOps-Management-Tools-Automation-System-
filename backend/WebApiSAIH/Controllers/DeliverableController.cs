using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.VO;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliverableController : ControllerBase
    {
        IServicioDeliverable _servicioDeliverable;
        public DeliverableController(IServicioDeliverable servicioDeliverable)
        {
            _servicioDeliverable = servicioDeliverable;
        }

        [HttpGet]
        public IActionResult obtenerDeliverables()
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.obtenerDeliverables();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            return Ok(respuestaGenerica);
        }

        [HttpGet("{pk_IdDeliverable}")]
        public IActionResult obtenerDeliverable(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.obtenerDeliverable(pk_IdDeliverable);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpPost]
        public IActionResult guardarDeliverable(DeliverableDTO deliverableDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.guardarDeliverable(deliverableDTO);

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

        [HttpPut("{pk_IdDeliverable}")]
        public IActionResult modificarDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.modificarDeliverable(pk_IdDeliverable, deliverableDTO);

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

        [HttpDelete("{pk_IdDeliverable}")]
        public IActionResult eliminarDeliverable(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.eliminarDeliverable(pk_IdDeliverable);

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

        [HttpGet("cambiarEstado/{pk_IdDeliverable}")]
        public IActionResult deshabilitarDepartment(long pk_IdDeliverable)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.deshabilitarDeliverable(pk_IdDeliverable);

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

        [HttpGet("deliverablesXtask/{fk_idTask}")]
        public IActionResult deliverablesXtask(long fk_idTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.deliverablesXtask(fk_idTask);

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

        [HttpGet("verificarDeliverable/{code}/{fk_idTask}")]
        public IActionResult verificarDeliverable(string code, long fk_idTask)
        {
            RespuestaGenerica respuestaGenerica = _servicioDeliverable.verificarDeliverable(code, fk_idTask);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}
