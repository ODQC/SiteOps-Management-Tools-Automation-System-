using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioProject
    {
        RespuestaGenerica guardarProject(ProjectDTO projectDTO);
        RespuestaGenerica obtenerProject(long idProjectDTO);
        RespuestaGenerica obtenerProjects();
        RespuestaGenerica deshabilitarProject(long idProjectDTO, long pk_idEmployee);
        RespuestaGenerica eliminarProject(long idProjectDTO);
        RespuestaGenerica modificarProject(long pk_IdProject, ProjectDTO projectDTO);
        RespuestaGenerica obtenerPKProjectNA();
        RespuestaGenerica taskesXproject(long pk_idProject);
        RespuestaGenerica verificarProject(String codigoProject, long fk_idEmployee);
        RespuestaGenerica projectXemployee(long fk_idEmployee);

        RespuestaGenerica cambiarProgesoPlan(long pk_idProject, string progress);
    }
}
