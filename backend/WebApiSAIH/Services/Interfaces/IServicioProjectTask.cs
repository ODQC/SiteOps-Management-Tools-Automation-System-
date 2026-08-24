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
        RespuestaGenerica guardarProjectTask(ProjectTaskDTO projectTaskDTO);

        RespuestaGenerica guardarProjectTaskList(List<ProjectTaskDTO> projectTaskDTOs);

        RespuestaGenerica eliminarProjectTask(long idProjectTaskDTO);

        RespuestaGenerica verificarRelacionExistente(long fk_idProject, long fk_idTask);

        RespuestaGenerica obtenerProjectTask();
    }
}
