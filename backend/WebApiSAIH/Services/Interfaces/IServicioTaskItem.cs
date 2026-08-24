using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioTaskItem
    {
        RespuestaGenerica createTask(TaskItemDTO taskDTO);
        RespuestaGenerica getTask(long idTaskDTO);
        RespuestaGenerica getTasks();
        RespuestaGenerica toggleTaskStatus(long idTaskDTO);
        RespuestaGenerica deleteTask(long idTaskDTO);
        RespuestaGenerica updateTask(long pk_IdTask, TaskItemDTO taskDTO);
        RespuestaGenerica getNATaskId();
        RespuestaGenerica projectsByTask(String code);
        RespuestaGenerica checkTask(String code, long fk_idProject);
        RespuestaGenerica tasksByProject(long fk_idProject);
        RespuestaGenerica cambiarTaskStatus(long pk_idTaskItem, string estado);
    }
}
