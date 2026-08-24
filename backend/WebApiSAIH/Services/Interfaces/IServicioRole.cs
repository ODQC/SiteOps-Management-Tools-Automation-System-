using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Interfaces
{
    public interface IServicioRole
    {
        RespuestaGenerica createRole(RoleDTO roleDTO);
        RespuestaGenerica obtenerRole(string idRole);
        RespuestaGenerica getRoles();
        RespuestaGenerica toggleRoleStatus(string idRole);
        RespuestaGenerica deleteRole(string idRole);
        RespuestaGenerica updateRole(long pK_idRole, RoleDTO roleDTO);
        RespuestaGenerica obtenerRole(long pk_idRole);
    }
}