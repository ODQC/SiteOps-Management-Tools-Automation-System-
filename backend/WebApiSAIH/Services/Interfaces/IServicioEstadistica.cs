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
        RespuestaGenerica employeesActivos();
        RespuestaGenerica employeesInactivos();
        RespuestaGenerica employeesRegistradosTotal();

        //Administrador de ASP
        RespuestaGenerica employeesActivosXAsp(String name);
        RespuestaGenerica planesDeTrabajoAsignados(String name);
        RespuestaGenerica taskesCompletadas(String name);
        RespuestaGenerica planesCompletados(String name);

        //Cantidad por rol de employee
        RespuestaGenerica cantidadAdministradorParque();
        RespuestaGenerica cantidadGuardaparques();
        RespuestaGenerica cantidadSupervisores();

        RespuestaGenerica cantidadAdminTI();
        RespuestaGenerica areasConservacionActivas();
        RespuestaGenerica siteesActivos();

        //Porcentajes
        RespuestaGenerica porcentajeTaskesCompletadasParque(String nombreASP);
        RespuestaGenerica porcentajeTaskesEnProcesoParque(String nombreASP);
        RespuestaGenerica porcentajeTaskesPendientesParque(String nombreASP);

        RespuestaGenerica porcentajeTaskesCompletadasEmployee(String nationalId);
        RespuestaGenerica porcentajeTaskesEnProcesoEmployee(String nationalId);
        RespuestaGenerica porcentajeTaskesPendientesEmployee(String nationalId);

        RespuestaGenerica porcentajeGeneralTaskesEmployee(String nationalId);
    }
}
