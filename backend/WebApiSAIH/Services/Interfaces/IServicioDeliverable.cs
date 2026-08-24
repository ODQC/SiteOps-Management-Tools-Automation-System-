using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Services.DTO;

namespace WebApiSAIH.Services.Interfaces
{
    public interface IServicioDeliverable
    {
        RespuestaGenerica guardarDeliverable(DeliverableDTO deliverableDTO);
        RespuestaGenerica obtenerDeliverable(long pk_idDeliverable);
        RespuestaGenerica obtenerDeliverables();
        RespuestaGenerica deshabilitarDeliverable(long pk_idDeliverable);
        RespuestaGenerica eliminarDeliverable(long pk_idDeliverable);
        RespuestaGenerica modificarDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO);
        RespuestaGenerica deliverablesXtask(long fk_idTask);
        RespuestaGenerica verificarDeliverable(string code, long fk_idTask);
    }
}
