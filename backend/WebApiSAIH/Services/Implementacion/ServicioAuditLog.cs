using SAIH_Backend.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAIH_Backend.Servicios.VO;
using SAIH_Backend.Servicios.DTO;
using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using Microsoft.EntityFrameworkCore;
using WebApiSAIH.Models;

namespace SAIH_Backend.Servicios.Implementacion
{
    public class ServicioAuditLog : IServicioAuditLog
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioAuditLog(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica guardarAuditLog(DTO.AuditLogDTO auditLogDTO)
        {
            try
            {

                AuditLog auditLog = _mapper.Map<AuditLog>(auditLogDTO);
                _context.AuditLogs.Add(auditLog);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "task registrada", _mapper.Map<AuditLogDTO>(auditLog));
            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }



        public RespuestaGenerica obtenerTaskesDeEmployees()
        {
            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Bitacora de employees desplegados", _context.AuditLogs.ToList());
        }

        public RespuestaGenerica obtenerTaskesPorEmployee(int idEmployee)
        {
            try
            {
                AuditLog auditLog = _context.AuditLogs.Where(
               s => s.Fk_IdEmployee2 == idEmployee).FirstOrDefault<AuditLog>();

                if (auditLog == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No hay tasks reuistradas es este employee", null);
                }
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Task completa de usario desplegada", _context.AuditLogs.ToList());
            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

        }
        private AuditLog covertirDTOAEntidad(AuditLog auditLog, AuditLogDTO auditLogDTO)
        {

            auditLog.Description = auditLogDTO.Description;
            auditLog.Date = auditLogDTO.Date;
            auditLog.Fk_IdEmployee2 = auditLogDTO.Fk_IdEmployee2;
            auditLog.PK_idAuditLog = auditLogDTO.PK_idAuditLog;


            return auditLog;
        }
    }
}