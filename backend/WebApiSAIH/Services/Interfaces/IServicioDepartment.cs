using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.Interfaces
{
    public interface IServicioDepartment
    {
        RespuestaGenerica guardarDepartment(DepartmentDTO departmentDTO);
        RespuestaGenerica obtenerDepartment(string idDepartmentDTO);
        RespuestaGenerica obtenerDepartments();
        RespuestaGenerica deshabilitarDepartment(string idDepartmentDTO);
        RespuestaGenerica eliminarDepartment(string idDepartmentDTO);
        RespuestaGenerica modificarDepartment(long pk_IdDepartment, DepartmentDTO departmentDTO);
        RespuestaGenerica obtenerPKDepartmentNA();
        RespuestaGenerica verificarDepartment(string codigoDepartment);
    }
}