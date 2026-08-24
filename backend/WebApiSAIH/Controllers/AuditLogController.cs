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
        public IActionResult getAuditLog()
        {
            return Ok(_auditLogService.getAuditLog());
        }
        [HttpGet("{idEmployee}")]
        public IActionResult getAuditLogByEmployee(int idEmployee)
        {
            return Ok(_auditLogService.getAuditLogByEmployee(idEmployee));
        }
        [HttpPost]
        public IActionResult createEmployee(AuditLogDTO auditLogDTO)
        {
            RespuestaGenerica respuestaGenerica = _auditLogService.createAuditLog(auditLogDTO);

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
