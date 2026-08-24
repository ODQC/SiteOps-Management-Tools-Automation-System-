using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.Interfaces;
using SAIH_Backend.Servicios.VO;
using WebApiSAIH.Models;

namespace SAIH_Backend.Servicios.Implementacion
{
    public class ServicioRole : IServicioRole
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ServicioRole(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deshabilitarRole(string idRole)
        {
            Role role = _context.EmployeeRoles.Where(
                s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Rol no encontrado", null);
            }

            if (role.Status == Status.ACTIVE)
            {
                role.Status = Status.INACTIVE;
            }
            else if (role.Status == Status.INACTIVE)
            {
                role.Status = Status.ACTIVE;
            }

            try
            {
                _context.Update(role);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado del Rol ha sido modificado", _mapper.Map<RoleDTO>(role));
        }

        public RespuestaGenerica eliminarRole(string idRole)
        {
            Role role = _context.EmployeeRoles.Where(
                s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Rol no encontrado", null);
            }

            try
            {
                _context.EmployeeRoles.Remove(role);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Rol " + role.Code + " eliminado exitosamente", _mapper.Map<RoleDTO>(role));
        }

        public RespuestaGenerica guardarRole(RoleDTO roleDTO)
        {
            try
            {
                Role role = _context.EmployeeRoles.Where(
                 s => s.Code == roleDTO.Code).FirstOrDefault<Role>();

                if (role != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El rol ya esta registrado en el sistema", null);
                }

                Role role2 = _mapper.Map<Role>(roleDTO);

                if (validarCampos(role2) != null)
                {
                    return validarCampos(role2);
                }

                _context.EmployeeRoles.Add(role2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Rol registrado", _mapper.Map<RoleDTO>(role2));
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

        private RespuestaGenerica validarCampos(Role role)
        {
            List<String> listaErrores = new List<String>();

            Role role2 = _context.EmployeeRoles.Where(
                 s => s.Code == role.Code).FirstOrDefault<Role>();

            if (role2 != null)
            {
                listaErrores.Add(Error.ROLE_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Error de validaciones", listaErrores);
        }

        public RespuestaGenerica modificarRole(long pK_idRole, RoleDTO roleDTO)
        {
            try
            {
                Role role = _context.EmployeeRoles.Where(
                 s => s.PK_idRole == pK_idRole).FirstOrDefault<Role>();

                if (role == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Rol no encontrado", "No se actualizo ningun rol");
                }
                    
                Role rolCodigo = _context.EmployeeRoles.Where(
                 s => s.Code == roleDTO.Code).FirstOrDefault<Role>();

                if (rolCodigo != null && (rolCodigo != role))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo de rol ya está siendo utilizado", "");
                }

                role = covertirDTOAEntidad(role, roleDTO);
                _context.Update(role);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Rol actualizado", _mapper.Map<RoleDTO>(role));
            }
            catch (Exception e)
            {
                if (!RolExists(pK_idRole))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Rol no encontrado", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
                }
            }
        }

        private bool RolExists(long pK_idRole)
        {
            return _context.EmployeeRoles.Any(e => e.PK_idRole == pK_idRole);
        }

        public RespuestaGenerica obtenerRoless()
        {
            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Roles desplegados", _context.EmployeeRoles.ToList());
        }

        public RespuestaGenerica obtenerRole(string idRole)
        {
            Role role = _context.EmployeeRoles
                .Where(s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el rol", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Rol", _mapper.Map<RoleDTO>(role));
        }


        private Role covertirDTOAEntidad(Role role, RoleDTO roleDTO)
        {
            role.Code = roleDTO.Code;
            role.Name = roleDTO.Name;
            role.Description = roleDTO.Description;
            role.Status = roleDTO.Status;

            return role;
        }

        public RespuestaGenerica obtenerRole(long pk_idRole)
        {
            Role role = _context.EmployeeRoles
                .Where(s => s.PK_idRole == pk_idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el rol", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Rol", _mapper.Map<RoleDTO>(role));
        }
    }
}