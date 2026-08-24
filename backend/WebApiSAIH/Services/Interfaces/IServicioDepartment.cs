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
        RespuestaGenerica createDepartment(DepartmentDTO departmentDTO);
        RespuestaGenerica getDepartment(string idDepartmentDTO);
        RespuestaGenerica getDepartments();
        RespuestaGenerica toggleDepartmentStatus(string idDepartmentDTO);
        RespuestaGenerica deleteDepartment(string idDepartmentDTO);
        RespuestaGenerica updateDepartment(long pk_IdDepartment, DepartmentDTO departmentDTO);
        RespuestaGenerica getNADepartmentId();
        RespuestaGenerica checkDepartment(string code);
    }
}