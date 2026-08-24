using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioProjectTask
    {
        RespuestaGenerica createProjectTask(ProjectTaskDTO projectTaskDTO);

        RespuestaGenerica createProjectTaskList(List<ProjectTaskDTO> projectTaskDTOs);

        RespuestaGenerica deleteProjectTask(long idProjectTaskDTO);

        RespuestaGenerica checkExistingLink(long fk_idProject, long fk_idTask);

        RespuestaGenerica getProjectTask();
    }
}
