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
        RespuestaGenerica createDeliverable(DeliverableDTO deliverableDTO);
        RespuestaGenerica getDeliverable(long pk_idDeliverable);
        RespuestaGenerica getDeliverables();
        RespuestaGenerica toggleDeliverableStatus(long pk_idDeliverable);
        RespuestaGenerica deleteDeliverable(long pk_idDeliverable);
        RespuestaGenerica updateDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO);
        RespuestaGenerica deliverablesByTask(long fk_idTask);
        RespuestaGenerica checkDeliverable(string code, long fk_idTask);
    }
}
