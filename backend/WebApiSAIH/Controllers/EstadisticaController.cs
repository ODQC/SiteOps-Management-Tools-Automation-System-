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
    [Authorize(Roles = Roles.ROLE_SUPERVISOR + "," + Roles.ROLE_SITE_MANAGER + "," + Roles.ROLE_EMPLOYEE + "," +
        Roles.ROLE_ADMIN)]
    public class EstadisticaController : ControllerBase
    {
        private IServicioEstadistica _servicioEstadistica;

        public EstadisticaController(IServicioEstadistica servicioEstadistica)
        {
            _servicioEstadistica = servicioEstadistica;
        }

        [HttpGet("activeEmployees")]
        public IActionResult activeEmployees()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.activeEmployees();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("inactiveEmployees")]
        public IActionResult inactiveEmployees()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.inactiveEmployees();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("totalRegisteredEmployees")]
        public IActionResult totalRegisteredEmployees()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.totalRegisteredEmployees();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }


        [HttpGet("activeEmployeesBySite/{name}")]
        public IActionResult activeEmployeesBySite(String name)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.activeEmployeesBySite(name);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("completedTasks/{name}")]
        public IActionResult completedTasks(String name)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.completedTasks(name);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("completedProjects/{siteName}")]
        public IActionResult completedProjects(String siteName)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.completedProjects(siteName);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfCompletedTasksBySite/{siteName}")]
        public IActionResult percentageOfCompletedTasksBySite(String siteName)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfCompletedTasksBySite(siteName);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfInProgressTasksBySite/{siteName}")]
        public IActionResult percentageOfInProgressTasksBySite(String siteName)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfInProgressTasksBySite(siteName);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfPendingTasksBySite/{siteName}")]
        public IActionResult percentageOfPendingTasksBySite(String siteName)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfPendingTasksBySite(siteName);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfCompletedTasksByEmployee/{nationalId}")]
        public IActionResult percentageOfCompletedTasksByEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfCompletedTasksByEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfInProgressTasksByEmployee/{nationalId}")]
        public IActionResult percentageOfInProgressTasksByEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfInProgressTasksByEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfPendingTasksByEmployee/{nationalId}")]
        public IActionResult percentageOfPendingTasksByEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfPendingTasksByEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("percentageOfTasksByEmployee/{nationalId}")]
        public IActionResult percentageOfTasksByEmployee(String nationalId)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.percentageOfTasksByEmployee(nationalId);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("countSiteManagers")]
        public IActionResult countSiteManagers()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.countSiteManagers();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("countEmployees")]
        public IActionResult countEmployees()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.countEmployees();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("assignedProjects/{siteName}")]
        public IActionResult assignedProjects(String siteName)
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.assignedProjects(siteName);

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("activeRegions")]
        public IActionResult activeRegions()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.activeRegions();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("activeSites")]
        public IActionResult activeSites()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.activeSites();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("countSupervisors")]
        public IActionResult countSupervisors()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.countSupervisors();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }

        [HttpGet("countAdmins")]
        public IActionResult countAdmins()
        {
            RespuestaGenerica respuestaGenerica = _servicioEstadistica.countAdmins();

            if (respuestaGenerica.Codigo == CodigosEstadoHTTP.HTTP_STATUS_OK)
            {
                return Ok(respuestaGenerica);
            }
            return StatusCode(500, respuestaGenerica);
        }
    }
}