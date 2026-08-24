using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;
using WebApiSAIH.Services.VOs;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IServicioDocument _servicioDocument;
        public DocumentController(IServicioDocument servicioDocument)
        {
            _servicioDocument = servicioDocument;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult guardarDocument()
        {
            FormFile document = null;

            try
            {
                 document = (FormFile)Request.Form.Files[0];
            }
            catch (Exception e)
            {
                return StatusCode(500, new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error en el servidor", e.Message));
            }
            
            RespuestaGenerica respuestaGenerica = _servicioDocument.guardarDocument(document);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            return StatusCode(200, respuestaGenerica);
        }

        [HttpGet]
        public IActionResult obtenerDocuments()
        {
            return Ok(_servicioDocument.obtenerDocuments());
        }

        [HttpGet("imagenEmployeeCedula/{cedula}")]
        public IActionResult obtenerDocumentXemployee(string cedula)
        {
            RespuestaGenerica respuestaGenerica = _servicioDocument.obtenerDocumentXemployee(cedula);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_BAD_REQUEST)
            {
                return BadRequest(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpDelete("{id}")]
        public IActionResult eliminarDocument(long id)
        {
            RespuestaGenerica respuestaGenerica = _servicioDocument.eliminarDocument(id);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> descargarDocument(int id)
        {
            RespuestaGenerica respuestaGenerica = await _servicioDocument.descargarDocument(id);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }

            DatosDocument datosDocument = (DatosDocument)respuestaGenerica.Object;

            return File(datosDocument.Memory, datosDocument.ContentType, datosDocument.FileName);
        }

        [HttpGet("imagenEmployee/{nationalId}")]
        public IActionResult obtenerImagenEmployee(string nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioDocument.obtenerDocumentXemployeeBytes(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }

        [HttpGet("obtenerNombrePkDocument/{pk_idDocument}")]
        public IActionResult obtenerNombrePkDocument(long pk_idDocument)
        {
            RespuestaGenerica respuestaGenerica = _servicioDocument.obtenerNombrePkDocument(pk_idDocument);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR)
            {
                return StatusCode(500, respuestaGenerica);
            }
            else if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_NOT_FOUND)
            {
                return NotFound(respuestaGenerica);
            }

            return Ok(respuestaGenerica);
        }
    }
}
