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
        RespuestaGenerica createProject(ProjectDTO projectDTO);
        RespuestaGenerica getProject(long idProjectDTO);
        RespuestaGenerica getProjects();
        RespuestaGenerica toggleProjectStatus(long idProjectDTO, long pk_idEmployee);
        RespuestaGenerica deleteProject(long idProjectDTO);
        RespuestaGenerica updateProject(long pk_IdProject, ProjectDTO projectDTO);
        RespuestaGenerica getNAProjectId();
        RespuestaGenerica tasksByProject(long pk_idProject);
        RespuestaGenerica checkProject(String codigoProject, long fk_idEmployee);
        RespuestaGenerica projectByEmployee(long fk_idEmployee);

        RespuestaGenerica cambiarProgesoPlan(long pk_idProject, string progress);
    }
}
