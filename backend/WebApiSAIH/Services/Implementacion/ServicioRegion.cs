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

        public RespuestaGenerica actualizarRegion(int pkIdRegion, RegionDTO regionDTO)
        {
            try
            {
                Region region = _context.Regions.Where(
                 s => s.PK_IdRegion == pkIdRegion).FirstOrDefault<Region>();

                if (region == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Area Conservacion no encontrada", "No se actualizo ningun area");
                }

                Region areaCodigo = _context.Regions.Where(
                 s => s.Code == regionDTO.Code).FirstOrDefault<Region>();

                if (areaCodigo != null && areaCodigo != region)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo de area ya está siendo utilizado", "");
                }

                region = covertirDTOAEntidad(region, regionDTO);
                _context.Update(region);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Area de Conservacion actualizada", _mapper.Map<RegionDTO>(region));
            }
            catch (Exception e)
            {
                if (!AreaExists(pkIdRegion))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Area de conservacion no encontrada", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
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

        public RespuestaGenerica eliminarRegion(string code)
        {
            Region region = _context.Regions.Where(
                s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Area Conservacion no encontrada", null);
            }

            try
            {
                _context.Regions.Remove(region);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Area Conservacion " + region.Code + " eliminada exitosamente", _mapper.Map<RegionDTO>(region));
        }

        public RespuestaGenerica guardarRegion(RegionDTO regionDTO)
        {
            try
            {
                Region region = _context.Regions.Where(
                 s => s.Code == regionDTO.Code).FirstOrDefault<Region>();

                if (region != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El area de conservacion ya esta registrada en el sistema", null);
                }

                Region region2 = _mapper.Map<Region>(regionDTO);

                if (validarCampos(region2) != null)
                {
                    return validarCampos(region2);
                }

                _context.Regions.Add(region2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Area Conservacion registrada", _mapper.Map<RegionDTO>(region2));
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

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Error de validaciones", listaErrores);
        }

        public RespuestaGenerica obtenerRegion(string code)
        {
            Region region = _context.Regions
                .Where(s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el area de conservacion", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Area de Conservacion", region);
        }

        public RespuestaGenerica obtenerAreasConservacion()
        {
            List<Region> areasConservacion = _context.Regions
                       .Where(s => s.Code != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Areas de Conservacion desplegadas", _mapper.Map<List<Region>>(areasConservacion));
        }

        public RespuestaGenerica obtenerPKRegionNA()
        {
            Region region = _context.Regions
                .Where(s => s.Code == "N/A").FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro area de conservacion con N/A", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "PK area de conservacion con N/A", region.PK_IdRegion);
        }

        public RespuestaGenerica deshabilitarRegion(string code)
        {
            Region region = _context.Regions.Where(
                s => s.Code == code).FirstOrDefault<Region>();

            if (region == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Area de Conservacion no encontrada", null);
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado del Area de conservacion ha sido modificado", _mapper.Map<RegionDTO>(region));
        }

        public RespuestaGenerica verificarArea(string code)
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica obtenerAreaXparque(long pk_idParque)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.PK_IdSite == pk_idParque).FirstOrDefault<Site>();

                Region region = _context.Regions.Where(
                s => s.PK_IdRegion == site.FK_idRegion1).FirstOrDefault<Region>();

                if (region == null)
                {
                    return new RespuestaGenerica 
                    {
                       Codigo =  CodigosEstadoHTTP.HTTP_NOT_FOUND,
                       Mensaje = "Area no encontrada", 
                       Object = null
                    };
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Area de conservacion asociada al parque " + pk_idParque,
                    Object = _mapper.Map<RegionDTO>(region)
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }
    }
}