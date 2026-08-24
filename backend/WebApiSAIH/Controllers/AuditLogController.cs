using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAIH_Backend.Servicios.Interfaces;
using SAIH_Backend.Servicios.VO;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.DTO;
using Microsoft.AspNetCore.Authorization;
using SAIH_Backend.Servicios.Clases_Estaticas;

namespace SAIH_Backend.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly IServicioAuditLog _auditLogService;

        public AuditLogController(IServicioAuditLog auditLogService) {

            _auditLogService = auditLogService;

        }
        [HttpGet]
        public IActionResult obtenerAuditLog()
        {
            return Ok(_auditLogService.obtenerTaskesDeEmployees());
        }
        [HttpGet("{idEmployee}")]
        public IActionResult obtenerTaskesPorEmployees(int idEmployee)
        {
            return Ok(_auditLogService.obtenerTaskesPorEmployee(idEmployee));
        }
        [HttpPost]
        public IActionResult guardarEmployee(AuditLogDTO auditLogDTO)
        {
            RespuestaGenerica respuestaGenerica = _auditLogService.guardarAuditLog(auditLogDTO);

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
    }
}
