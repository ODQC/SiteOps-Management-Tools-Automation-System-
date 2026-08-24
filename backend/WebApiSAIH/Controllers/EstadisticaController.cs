using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    [Authorize(Roles = Roles.ROL_SUPERVISOR + "," + Roles.ROL_ADMINISTRADOR_PARQUE + "," + Roles.ROL_GUARDAPARQUE + "," +
        Roles.ROL_ADMINISTRADOR_TI)]
    public class EstadisticaController : ControllerBase
    {
        private IServicioEstadistica _servicioEstadistica;

        public EstadisticaController(IServicioEstadistica servicioEstadistica)
        {
            _servicioEstadistica = servicioEstadistica;
        }

        [HttpGet("employeesActivos")]
        public IActionResult employeesActivos()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.employeesActivos();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("employeesInactivos")]
        public IActionResult employeesInactivos()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.employeesInactivos();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("employeesRegistradosTotal")]
        public IActionResult employeesRegistradosTotal()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.employeesRegistradosTotal();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }


        [HttpGet("employeesActivosXAsp/{name}")]
        public IActionResult employeesActivosXAsp(String name)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.employeesActivosXAsp(name);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("taskesCompletadas{name}")]
        public IActionResult taskesCompletadas(String name)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.taskesCompletadas(name);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("planesCompletados{nombreASP}")]
        public IActionResult planesCompletados(String nombreASP)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.planesCompletados(nombreASP);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesCompletadasParque{nombreASP}")]
        public IActionResult porcentajeTaskesCompletadasParque(String nombreASP)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesCompletadasParque(nombreASP);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesEnProcesoParque{nombreASP}")]
        public IActionResult porcentajeTaskesEnProcesoParque(String nombreASP)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesEnProcesoParque(nombreASP);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesPendientesParque{nombreASP}")]
        public IActionResult porcentajeTaskesPendientesParque(String nombreASP)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesPendientesParque(nombreASP);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesCompletadasEmployee{nationalId}")]
        public IActionResult porcentajeTaskesCompletadasEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesCompletadasEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesEnProcesoEmployee{nationalId}")]
        public IActionResult porcentajeTaskesEnProcesoEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesEnProcesoEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeTaskesPendientesEmployee{nationalId}")]
        public IActionResult porcentajeTaskesPendientesEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeTaskesPendientesEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("porcentajeGeneralTaskesEmployee{nationalId}")]
        public IActionResult porcentajeGeneralTaskesEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.porcentajeGeneralTaskesEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("cantidadAdministradorParque")]
        public IActionResult cantidadAdministradorParque()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.cantidadAdministradorParque();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("cantidadGuardaparques")]
        public IActionResult cantidadGuardaparques()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.cantidadGuardaparques();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("planesDeTrabajoAsignados/{nombreAsp}")]
        public IActionResult planesDeTrabajoAsignados(String nombreAsp)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.planesDeTrabajoAsignados(nombreAsp);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("areasConservacionActivas")]
        public IActionResult areasConservacionActivas()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.areasConservacionActivas();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("siteesActivos")]
        public IActionResult siteesActivos()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.siteesActivos();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("cantidadSupervisores")]
        public IActionResult cantidadSupervisores()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.cantidadSupervisores();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("cantidadAdminTI")]
        public IActionResult cantidadAdminTI()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.cantidadAdminTI();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }
    }
}