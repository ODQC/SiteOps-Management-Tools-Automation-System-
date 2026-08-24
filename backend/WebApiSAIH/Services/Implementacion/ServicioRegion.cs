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
    public class ServicioRegion : IServicioRegion
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioRegion(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica updateRegion(int pkIdRegion, RegionDTO regionDTO)
        {
            try
            {
                Region region = _context.Regions.Where(
                 s => s.PK_IdRegion == pkIdRegion).FirstOrDefault<Region>();

                if (region == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Region not found", "No region was updated");
                }

                Region areaCodigo = _context.Regions.Where(
                 s => s.Code == regionDTO.Code).FirstOrDefault<Region>();

                if (areaCodigo != null && areaCodigo != region)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This region code is already in use", "");
                }

                region = covertirDTOAEntidad(region, regionDTO);
                _context.Update(region);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Region updated", _mapper.Map<RegionDTO>(region));
            }
            catch (Exception e)
            {
                if (!AreaExists(pkIdRegion))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Region not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool AreaExists(int pkIdRegion)
        {
            return _context.Regions.Any(e => e.PK_IdRegion == pkIdRegion);
        }

        private Region covertirDTOAEntidad(Region region, RegionDTO regionDTO)
        {
            region.Code = regionDTO.Code;
            region.Description = regionDTO.Description;
            region.Status = regionDTO.Status;
            region.Name = regionDTO.Name;

            return region;
        }

        public RespuestaGenerica deleteRegion(string code)
        {
            Region region = _context.Regions.Where(
                s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Region not found", null);
            }

            try
            {
                _context.Regions.Remove(region);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Region " + region.Code + " deleted successfully", _mapper.Map<RegionDTO>(region));
        }

        public RespuestaGenerica createRegion(RegionDTO regionDTO)
        {
            try
            {
                Region region = _context.Regions.Where(
                 s => s.Code == regionDTO.Code).FirstOrDefault<Region>();

                if (region != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This region is already registered in the system", null);
                }

                Region region2 = _mapper.Map<Region>(regionDTO);

                if (validarCampos(region2) != null)
                {
                    return validarCampos(region2);
                }

                _context.Regions.Add(region2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Region registered", _mapper.Map<RegionDTO>(region2));
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

        private RespuestaGenerica validarCampos(Region region)
        {
            List<String> listaErrores = new List<String>();

            Region region2 = _context.Regions.Where(
                 s => s.Code == region.Code).FirstOrDefault<Region>();

            if (region2 != null)
            {
                listaErrores.Add(Error.REGION_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica getRegion(string code)
        {
            Region region = _context.Regions
                .Where(s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Region not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Region", region);
        }

        public RespuestaGenerica getRegions()
        {
            List<Region> areasConservacion = _context.Regions
                       .Where(s => s.Code != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Regions listed", _mapper.Map<List<Region>>(areasConservacion));
        }

        public RespuestaGenerica getNARegionId()
        {
            Region region = _context.Regions
                .Where(s => s.Code == "N/A").FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A region found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Region PK for N/A", region.PK_IdRegion);
        }

        public RespuestaGenerica toggleRegionStatus(string code)
        {
            Region region = _context.Regions.Where(
                s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Region not found", null);
            }

            if (region.Status == Status.ACTIVE)
            {
                region.Status = Status.INACTIVE;
            }
            else if (region.Status == Status.INACTIVE)
            {
                region.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(region);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The region's status has been changed", _mapper.Map<RegionDTO>(region));
        }

        public RespuestaGenerica checkRegion(string code)
        {
            try
            {
                Region region = _context.Regions.Where(
                s => s.Code == code).FirstOrDefault<Region>();

                if (region != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Area en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Area disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica getRegionBySite(long pk_idSite)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.PK_IdSite == pk_idSite).FirstOrDefault<Site>();

                Region region = _context.Regions.Where(
                s => s.PK_IdRegion == site.FK_idRegion1).FirstOrDefault<Region>();

                if (region == null)
                {
                    return new RespuestaGenerica 
                    {
                       Codigo =  CodigosEstadoHTTP.HTTP_NOT_FOUND,
                       Mensaje = "Region not found", 
                       Object = null
                    };
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Region associated with the site " + pk_idSite,
                    Object = _mapper.Map<RegionDTO>(region)
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }
    }
}