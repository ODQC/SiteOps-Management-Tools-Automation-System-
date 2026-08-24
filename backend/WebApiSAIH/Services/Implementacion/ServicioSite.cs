using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.Interfaces;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models;

namespace SAIH_Backend.Servicios.Implementacion
{
    public class ServicioSite : IServicioSite
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioSite(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deshabilitarSite(string code)
        {
            Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Parque Nacional no encontrado", null);
            }

            if (site.Status == Status.ACTIVE)
            {
                site.Status = Status.INACTIVE;
            }
            else if (site.Status == Status.INACTIVE)
            {
                site.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(site);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado del Parque Nacional ha sido modificado", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica eliminarSite(string code)
        {
            Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Parque Nacional no encontrado", null);
            }

            try
            {
                _context.Sites.Remove(site);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Area Conservacion " + site.Code + " eliminada exitosamente", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica guardarSite(SiteDTO siteDTO)
        {
            try
            {
                Site site = _context.Sites.Where(
                 s => s.Code == siteDTO.Code).FirstOrDefault<Site>();

                if (site != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El parque nacional ya esta registrado en el sistema", null);
                }

                Site site2 = _mapper.Map<Site>(siteDTO);

                if (validarCampos(site2) != null)
                {
                    return validarCampos(site2);
                }

                _context.Sites.Add(site2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Parque Nacional registrada", _mapper.Map<SiteDTO>(site2));
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

        private RespuestaGenerica validarCampos(Site site)
        {
            List<String> listaErrores = new List<String>();

            Site site2 = _context.Sites.Where(
                 s => s.Code == site.Code).FirstOrDefault<Site>();

            if (site2 != null)
            {
                listaErrores.Add(Error.SITE_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Error de validaciones", listaErrores);
        }

        public RespuestaGenerica modificarSite(long pk_IdSite, SiteDTO siteDTO)
        {
            try
            {
                Site site = _context.Sites.Where(
                 s => s.PK_IdSite == pk_IdSite).FirstOrDefault<Site>();

                if (site == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Parque Nacional no encontrado", "No se actualizo ningun parque");
                }

                Site parqueCodigo = _context.Sites.Where(
                 s => s.Code == siteDTO.Code).FirstOrDefault<Site>();

                if (parqueCodigo != null && parqueCodigo != site)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo de parque ya está siendo utilizado", "");
                }

                site = covertirDTOAEntidad(site, siteDTO);
                _context.Update(site);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Parque Nacional actualizado", _mapper.Map<Site>(site));
            }
            catch (Exception e)
            {
                if (!ParqueExists(pk_IdSite))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Parque Nacional no encontrado", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
                }
            }
        }

        private bool ParqueExists(long pk_IdSite)
        {
            return _context.Sites.Any(e => e.PK_IdSite == pk_IdSite);
        }

        private Site covertirDTOAEntidad(Site site, SiteDTO siteDTO)
        {
            site.Code = siteDTO.Code;
            site.Description = siteDTO.Description;
            site.Status = siteDTO.Status;
            site.Name = siteDTO.Name;

            return site;
        }

        public RespuestaGenerica obtenerSite(string code)
        {
            Site site = _context.Sites
                 .Where(s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el parque Nacional", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Parque Nacional", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica obtenerParquesNacionales(int idRegion)
        {
            List<Site> parquesNacionales = _context.Sites
                       .Where(s => s.Code != "N/A" && s.FK_idRegion1 == idRegion)
                       .ToList();

            if (parquesNacionales == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontraron parques Nacionales", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Parques Nacionales desplegados", _mapper.Map<List<SiteDTO>>(parquesNacionales));
        }

        public RespuestaGenerica obtenerPKParqueNA()
        {
            Site site = _context.Sites
                .Where(s => s.Code == "N/A").FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro parque Nacional con N/A", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "PK parque nacional con N/A", site.PK_IdSite);
        }

        public RespuestaGenerica obtenerTodosParquesNacionales()
        {
            List<Site> parquesNacionales = _context.Sites
                .Where(s => s.Code != "N/A").ToList();

            if (parquesNacionales == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND,"No se encontraron parques",null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Parques desplegados", _mapper.Map<List<SiteDTO>>(parquesNacionales));
        }

        public RespuestaGenerica verificarParque(string code)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

                if (site != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Parque en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Parque disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica verificarAdminParque(string code)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

                if (administradoresParqueActivos(site.PK_IdSite) == true)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El parque " + site.Name + 
                        " ya tiene un administrador activo", true);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El parque " + site.Name +
                        " no tiene un administrador activo", false);
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        private bool administradoresParqueActivos(long fk_site)
        {
            Role rolAdminParque = _context.Roles.Where(
                s => s.Name == Roles.ROL_ADMINISTRADOR_PARQUE).FirstOrDefault<Role>();

            Site site = _context.Sites.Where(
                s => s.PK_IdSite == fk_site).FirstOrDefault<Site>();

            List<Employee> employees = _context.Employees.Where(
                s => s.FK_idRole1 == rolAdminParque.PK_idRole && s.Status == Status.ACTIVE
                    && s.FK_idSite1 == site.PK_IdSite).ToList();

            if (employees.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}