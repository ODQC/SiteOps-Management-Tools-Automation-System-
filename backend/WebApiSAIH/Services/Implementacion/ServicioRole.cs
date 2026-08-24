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

        public RespuestaGenerica toggleRoleStatus(string idRole)
        {
            Role role = _context.EmployeeRoles.Where(
                s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", null);
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The role's status has been changed", _mapper.Map<RoleDTO>(role));
        }

        public RespuestaGenerica deleteRole(string idRole)
        {
            Role role = _context.EmployeeRoles.Where(
                s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", null);
            }

            try
            {
                _context.EmployeeRoles.Remove(role);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Role " + role.Code + " deleted successfully", _mapper.Map<RoleDTO>(role));
        }

        public RespuestaGenerica createRole(RoleDTO roleDTO)
        {
            try
            {
                Role role = _context.EmployeeRoles.Where(
                 s => s.Code == roleDTO.Code).FirstOrDefault<Role>();

                if (role != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This role is already registered in the system", null);
                }

                Role role2 = _mapper.Map<Role>(roleDTO);

                if (validarCampos(role2) != null)
                {
                    return validarCampos(role2);
                }

                _context.EmployeeRoles.Add(role2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Role registered", _mapper.Map<RoleDTO>(role2));
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

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica updateRole(long pK_idRole, RoleDTO roleDTO)
        {
            try
            {
                Role role = _context.EmployeeRoles.Where(
                 s => s.PK_idRole == pK_idRole).FirstOrDefault<Role>();

                if (role == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", "No role was updated");
                }
                    
                Role rolCodigo = _context.EmployeeRoles.Where(
                 s => s.Code == roleDTO.Code).FirstOrDefault<Role>();

                if (rolCodigo != null && (rolCodigo != role))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This role code is already in use", "");
                }

                role = covertirDTOAEntidad(role, roleDTO);
                _context.Update(role);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Role updated", _mapper.Map<RoleDTO>(role));
            }
            catch (Exception e)
            {
                if (!RolExists(pK_idRole))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool RolExists(long pK_idRole)
        {
            return _context.EmployeeRoles.Any(e => e.PK_idRole == pK_idRole);
        }

        public RespuestaGenerica getRoles()
        {
            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Roles listed", _context.EmployeeRoles.ToList());
        }

        public RespuestaGenerica obtenerRole(string idRole)
        {
            Role role = _context.EmployeeRoles
                .Where(s => s.Code == idRole).FirstOrDefault<Role>();

            if (role == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Role", _mapper.Map<RoleDTO>(role));
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Role not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Role", _mapper.Map<RoleDTO>(role));
        }
    }
}