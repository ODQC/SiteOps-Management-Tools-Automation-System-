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
        RespuestaGenerica guardarRole(RoleDTO roleDTO);
        RespuestaGenerica obtenerRole(string idRole);
        RespuestaGenerica obtenerRoless();
        RespuestaGenerica deshabilitarRole(string idRole);
        RespuestaGenerica eliminarRole(string idRole);
        RespuestaGenerica modificarRole(long pK_idRole, RoleDTO roleDTO);
        RespuestaGenerica obtenerRole(long pk_idRole);
    }
}