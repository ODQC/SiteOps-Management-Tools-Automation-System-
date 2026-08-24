using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioEstadistica
    {
        //Admin TI
        RespuestaGenerica activeEmployees();
        RespuestaGenerica inactiveEmployees();
        RespuestaGenerica totalRegisteredEmployees();

        //Administrador de ASP
        RespuestaGenerica activeEmployeesBySite(String name);
        RespuestaGenerica assignedProjects(String name);
        RespuestaGenerica completedTasks(String name);
        RespuestaGenerica completedProjects(String name);

        //Count by employee role
        RespuestaGenerica countSiteManagers();
        RespuestaGenerica countEmployees();
        RespuestaGenerica countSupervisors();

        RespuestaGenerica countAdmins();
        RespuestaGenerica activeRegions();
        RespuestaGenerica activeSites();

        //Porcentajes
        RespuestaGenerica percentageOfCompletedTasksBySite(String siteName);
        RespuestaGenerica percentageOfInProgressTasksBySite(String siteName);
        RespuestaGenerica percentageOfPendingTasksBySite(String siteName);

        RespuestaGenerica percentageOfCompletedTasksByEmployee(String nationalId);
        RespuestaGenerica percentageOfInProgressTasksByEmployee(String nationalId);
        RespuestaGenerica percentageOfPendingTasksByEmployee(String nationalId);

        RespuestaGenerica percentageOfTasksByEmployee(String nationalId);
    }
}
