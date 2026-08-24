using AutoMapper;
using BackendAPI3._1.Models;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioDeliverable : IServicioDeliverable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioDeliverable(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deshabilitarDeliverable(long pk_idDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_idDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable no encontrada", null);
            }

            if (deliverable.Status == Status.ACTIVE)
            {
                deliverable.Status = Status.INACTIVE;
            }
            else if (deliverable.Status == Status.INACTIVE)
            {
                deliverable.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(deliverable);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado de la deliverable ha sido modificada", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica eliminarDeliverable(long pk_idDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_idDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department no encontrado", null);
            }

            try
            {
                _context.Deliverables.Remove(deliverable);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable " + deliverable.Code + " eliminada exitosamente", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica guardarDeliverable(DeliverableDTO deliverableDTO)
        {
            try
            {
                Deliverable deliverable = _context.Deliverables.Where(
                 s => s.Code == deliverableDTO.Code && s.FK_idTask1 == deliverableDTO.FK_idTask1).FirstOrDefault<Deliverable>();

                if (deliverable != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "La deliverable ya esta registrada en el sistema", null);
                }

                Deliverable deliverable2 = _mapper.Map<Deliverable>(deliverableDTO);

                _context.Deliverables.Add(deliverable2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Deliverable registrada", _mapper.Map<DeliverableDTO>(deliverable2));
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

        public RespuestaGenerica modificarDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO)
        {
            try
            {
                Deliverable deliverable = _context.Deliverables.Where(
                 s => s.PK_idDeliverable == pk_IdDeliverable).FirstOrDefault<Deliverable>();

                if (deliverable == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable no encontrada", "No se actualizo ninguna deliverable");
                }

                Deliverable deliverableCodigo = _context.Deliverables.Where(
                 s => s.Code == deliverableDTO.Code && s.FK_idTask1 == deliverableDTO.FK_idTask1).FirstOrDefault<Deliverable>();

                if (deliverableCodigo != null && (deliverableCodigo != deliverable))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo de deliverable ya está siendo utilizado", "");
                }

                deliverable = covertirDTOAEntidad(deliverable, deliverableDTO);
                _context.Update(deliverable);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable actualizada", _mapper.Map<DeliverableDTO>(deliverable));
            }
            catch (Exception e)
            {
                if (!DeliverableExists(pk_IdDeliverable))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable no encontrada", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
                }
            }
        }

        private bool DeliverableExists(long pK_idDeliverable)
        {
            return _context.Deliverables.Any(e => e.PK_idDeliverable == pK_idDeliverable);
        }

        private Deliverable covertirDTOAEntidad(Deliverable deliverable, DeliverableDTO deliverableDTO)
        {
            deliverable.Code = deliverableDTO.Code;
            deliverable.Status = deliverableDTO.Status;
            deliverable.FK_idDocument = deliverableDTO.FK_idDocument;
            deliverable.Nombre = deliverableDTO.Nombre;
            deliverable.Descripcion = deliverableDTO.Descripcion;

            return deliverable;
        }

        public RespuestaGenerica obtenerDeliverable(long pk_IdDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_IdDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable no encontrada", null);
            }

            Document document = _context.Documents.Where(
                s => s.DocumentId == deliverable.FK_idDocument).FirstOrDefault<Document>();

            deliverable.DocumentName = document?.Name;

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable encontrada", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica obtenerDeliverables()
        {
            List<Deliverable> deliverables = null;
            try
            {
                 deliverables = _context.Deliverables.ToList();

                for(int i = 0; i< deliverables.Count; i++)
                {
                    Document document = _context.Documents.Where(
                        s => s.DocumentId == deliverables[i].FK_idDocument).FirstOrDefault<Document>();

                    deliverables[i].DocumentName = document.Name;
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error del servidor",e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverables desplegadas", _mapper.Map<List<DeliverableDTO>>(deliverables));
        }

        public RespuestaGenerica deliverablesXtask(long fk_idTask)
        {
            try
            {
                List<Deliverable> deliverables = _context.Deliverables.Where(
                s => s.FK_idTask1 == fk_idTask).ToList();

                if (deliverables == null)
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_NOT_FOUND,
                        Mensaje = "No se encontraron evidenias asociadas a la task",
                        Object = null
                    };
                }

                for (int i = 0; i < deliverables.Count; i++)
                {
                    Document document = _context.Documents.Where(
                        s => s.DocumentId == deliverables[i].FK_idDocument).FirstOrDefault<Document>();

                    deliverables[i].DocumentName = document.Name;
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Deliverables asociadas a la task " + fk_idTask,
                    Object = _mapper.Map<List<DeliverableDTO>>(deliverables)
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Error Interno del Servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica verificarDeliverable(string code, long fk_idTask)
        {
            try
            {
                Deliverable deliverable = _context.Deliverables.Where(
                s => s.Code == code && s.FK_idTask1 == fk_idTask).FirstOrDefault<Deliverable>();

                if (deliverable != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Task en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Task disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }
    }
}
