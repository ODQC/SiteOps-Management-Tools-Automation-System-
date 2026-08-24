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

        public RespuestaGenerica toggleSiteStatus(string code)
        {
            Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Site not found", null);
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The site's status has been changed", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica deleteSite(string code)
        {
            Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Site not found", null);
            }

            try
            {
                _context.Sites.Remove(site);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site " + site.Code + " deleted successfully", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica createSite(SiteDTO siteDTO)
        {
            try
            {
                Site site = _context.Sites.Where(
                 s => s.Code == siteDTO.Code).FirstOrDefault<Site>();

                if (site != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This site is already registered in the system", null);
                }

                Site site2 = _mapper.Map<Site>(siteDTO);

                if (validarCampos(site2) != null)
                {
                    return validarCampos(site2);
                }

                _context.Sites.Add(site2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Site registered", _mapper.Map<SiteDTO>(site2));
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

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica updateSite(long pk_IdSite, SiteDTO siteDTO)
        {
            try
            {
                Site site = _context.Sites.Where(
                 s => s.PK_IdSite == pk_IdSite).FirstOrDefault<Site>();

                if (site == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Site not found", "No site was updated");
                }

                Site parqueCodigo = _context.Sites.Where(
                 s => s.Code == siteDTO.Code).FirstOrDefault<Site>();

                if (parqueCodigo != null && parqueCodigo != site)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This site code is already in use", "");
                }

                site = covertirDTOAEntidad(site, siteDTO);
                _context.Update(site);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site updated", _mapper.Map<Site>(site));
            }
            catch (Exception e)
            {
                if (!ParqueExists(pk_IdSite))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Site not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
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

        public RespuestaGenerica getSite(string code)
        {
            Site site = _context.Sites
                 .Where(s => s.Code == code).FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Site not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site", _mapper.Map<SiteDTO>(site));
        }

        public RespuestaGenerica getSitesByRegion(int idRegion)
        {
            List<Site> parquesNacionales = _context.Sites
                       .Where(s => s.Code != "N/A" && s.FK_idRegion1 == idRegion)
                       .ToList();

            if (parquesNacionales == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No sites found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Sites listed", _mapper.Map<List<SiteDTO>>(parquesNacionales));
        }

        public RespuestaGenerica getNASiteId()
        {
            Site site = _context.Sites
                .Where(s => s.Code == "N/A").FirstOrDefault<Site>();

            if (site == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A site found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site PK for N/A", site.PK_IdSite);
        }

        public RespuestaGenerica getAllSites()
        {
            List<Site> parquesNacionales = _context.Sites
                .Where(s => s.Code != "N/A").ToList();

            if (parquesNacionales == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND,"No sites found",null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Sites listed", _mapper.Map<List<SiteDTO>>(parquesNacionales));
        }

        public RespuestaGenerica checkSite(string code)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

                if (site != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site code in use", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Site code available", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica checkSiteManager(string code)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.Code == code).FirstOrDefault<Site>();

                if (administradoresParqueActivos(site.PK_IdSite) == true)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The site " + site.Name + 
                        " already has an active manager", true);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The site " + site.Name +
                        " has no active manager", false);
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        private bool administradoresParqueActivos(long fk_site)
        {
            Role rolAdminParque = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_SITE_MANAGER).FirstOrDefault<Role>();

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