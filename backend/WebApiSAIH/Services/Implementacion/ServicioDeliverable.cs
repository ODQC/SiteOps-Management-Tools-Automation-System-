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

        public RespuestaGenerica toggleDeliverableStatus(long pk_idDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_idDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable not found", null);
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The deliverable's status has been changed", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica deleteDeliverable(long pk_idDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_idDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", null);
            }

            try
            {
                _context.Deliverables.Remove(deliverable);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable " + deliverable.Code + " deleted successfully", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica createDeliverable(DeliverableDTO deliverableDTO)
        {
            try
            {
                Deliverable deliverable = _context.Deliverables.Where(
                 s => s.Code == deliverableDTO.Code && s.FK_idTask1 == deliverableDTO.FK_idTask1).FirstOrDefault<Deliverable>();

                if (deliverable != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This deliverable is already registered in the system", null);
                }

                Deliverable deliverable2 = _mapper.Map<Deliverable>(deliverableDTO);

                _context.Deliverables.Add(deliverable2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Deliverable registered", _mapper.Map<DeliverableDTO>(deliverable2));
            }
            catch (DbUpdateException e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica updateDeliverable(long pk_IdDeliverable, DeliverableDTO deliverableDTO)
        {
            try
            {
                Deliverable deliverable = _context.Deliverables.Where(
                 s => s.PK_idDeliverable == pk_IdDeliverable).FirstOrDefault<Deliverable>();

                if (deliverable == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable not found", "No deliverable was updated");
                }

                Deliverable deliverableCodigo = _context.Deliverables.Where(
                 s => s.Code == deliverableDTO.Code && s.FK_idTask1 == deliverableDTO.FK_idTask1).FirstOrDefault<Deliverable>();

                if (deliverableCodigo != null && (deliverableCodigo != deliverable))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This deliverable code is already in use", "");
                }

                deliverable = covertirDTOAEntidad(deliverable, deliverableDTO);
                _context.Update(deliverable);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable updated", _mapper.Map<DeliverableDTO>(deliverable));
            }
            catch (Exception e)
            {
                if (!DeliverableExists(pk_IdDeliverable))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
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
            deliverable.Name = deliverableDTO.Name;
            deliverable.Description = deliverableDTO.Description;

            return deliverable;
        }

        public RespuestaGenerica getDeliverable(long pk_IdDeliverable)
        {
            Deliverable deliverable = _context.Deliverables.Where(
                s => s.PK_idDeliverable == pk_IdDeliverable).FirstOrDefault<Deliverable>();

            if (deliverable == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Deliverable not found", null);
            }

            Document document = _context.Documents.Where(
                s => s.DocumentId == deliverable.FK_idDocument).FirstOrDefault<Document>();

            deliverable.DocumentName = document?.Name;

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverable found", _mapper.Map<DeliverableDTO>(deliverable));
        }

        public RespuestaGenerica getDeliverables()
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Server error",e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Deliverables listed", _mapper.Map<List<DeliverableDTO>>(deliverables));
        }

        public RespuestaGenerica deliverablesByTask(long fk_idTask)
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
                        Mensaje = "No deliverables found for this task",
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
                    Mensaje = "Deliverables associated with the task " + fk_idTask,
                    Object = _mapper.Map<List<DeliverableDTO>>(deliverables)
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Internal Server Error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica checkDeliverable(string code, long fk_idTask)
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }
    }
}
