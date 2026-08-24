using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Interfaces
{
    public interface IServicioAuditLog
    {
        RespuestaGenerica createAuditLog(AuditLogDTO ActEmployee); 
        RespuestaGenerica getAuditLogByEmployee(int idEmployee);
        RespuestaGenerica getAuditLog();
    }
}
