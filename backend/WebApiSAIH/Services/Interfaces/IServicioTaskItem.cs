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
        RespuestaGenerica guardarTask(TaskItemDTO taskDTO);
        RespuestaGenerica obtenerTask(long idTaskDTO);
        RespuestaGenerica obtenerTaskes();
        RespuestaGenerica deshabilitarTask(long idTaskDTO);
        RespuestaGenerica eliminarTask(long idTaskDTO);
        RespuestaGenerica modificarTask(long pk_IdTask, TaskItemDTO taskDTO);
        RespuestaGenerica obtenerPKTaskNA();
        RespuestaGenerica planesTrabajoXtask(String code);
        RespuestaGenerica verificarTask(String code, long fk_idProject);
        RespuestaGenerica taskesXproject(long fk_idProject);
        RespuestaGenerica cambiarTaskStatus(long pk_idTaskItem, string estado);
    }
}
