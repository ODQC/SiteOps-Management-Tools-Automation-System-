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

    public class ServicioDepartment : IServicioDepartment
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioDepartment(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica toggleDepartmentStatus(string idDepartmentDTO)
        {
            Department department = _context.Departments.Where(
                s => s.CodigoDepartment == idDepartmentDTO).FirstOrDefault<Department>();

            if (department == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", null);
            }

            if (department.EstadoDepartment == Status.ACTIVE)
            {
                department.EstadoDepartment = Status.INACTIVE;
            }
            else if (department.EstadoDepartment == Status.INACTIVE)
            {
                department.EstadoDepartment = Status.ACTIVE;
            }

            try
            {
                _context.Update(department);
                 _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The department's status has been changed", _mapper.Map<DepartmentDTO>(department));
        }

        public RespuestaGenerica deleteDepartment(string idDepartmentDTO)
        {
            Department department = _context.Departments.Where(
                s => s.CodigoDepartment == idDepartmentDTO).FirstOrDefault<Department>();

            if (department == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", null);
            }

            try
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Department " + department.CodigoDepartment + " deleted successfully", _mapper.Map<DepartmentDTO>(department));
        }

        public RespuestaGenerica createDepartment(DepartmentDTO departmentDTO)
        {
            try
            {
                Department department = _context.Departments.Where(
                 s => s.CodigoDepartment == departmentDTO.CodigoDepartment).FirstOrDefault<Department>();

                if (department != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This department is already registered in the system", null);
                }

                Department department2 = _mapper.Map<Department>(departmentDTO);

                if (validarCampos(department2) != null)
                {
                    return validarCampos(department2);
                }

                _context.Departments.Add(department2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Department registered", _mapper.Map<DepartmentDTO>(department2));
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

        public RespuestaGenerica updateDepartment(long pk_IdDepartment, DepartmentDTO departmentDTO)
        {
            try
            {
                Department department = _context.Departments.Where(
                 s => s.PK_idDepartment == pk_IdDepartment).FirstOrDefault<Department>();

                if (department == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", "No department was updated");
                }

                Department departmentCodigo = _context.Departments.Where(
                 s => s.CodigoDepartment == departmentDTO.CodigoDepartment).FirstOrDefault<Department>();

                if (departmentCodigo != null && (departmentCodigo != department))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This department code is already in use", "");
                }

                department = covertirDTOAEntidad(department, departmentDTO);
                _context.Update(department);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Department updated", _mapper.Map<DepartmentDTO>(department));
            }
            catch (Exception e)
            {
                if (!DepartmentExists(pk_IdDepartment))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool DepartmentExists(long pk_IdDepartment)
        {
            return _context.Departments.Any(e => e.PK_idDepartment == pk_IdDepartment);
        }

        public RespuestaGenerica getDepartment(string idDepartmentDTO)
        {
            Department department = _context.Departments
                .Where(s => s.CodigoDepartment == idDepartmentDTO).FirstOrDefault<Department>();

            if (department == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Department not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Department", _mapper.Map<DepartmentDTO>(department));
        }

        public RespuestaGenerica getDepartments()
        {
            List<Department> departments = _context.Departments
                       .Where(s => s.CodigoDepartment != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Departments listed", _mapper.Map<List<DepartmentDTO>>(departments));
        }

        public RespuestaGenerica getNADepartmentId()
        {
            Department department = _context.Departments
                .Where(s => s.CodigoDepartment == "N/A").FirstOrDefault<Department>();

            if (department == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A department found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Department PK for N/A", department.PK_idDepartment);
        }

        private Department covertirDTOAEntidad(Department department, DepartmentDTO departmentDTO)
        {
            department.CodigoDepartment = departmentDTO.CodigoDepartment;
            department.NombreDepartment = departmentDTO.NombreDepartment;
            department.DescripcionDepartment = departmentDTO.DescripcionDepartment;
            department.EstadoDepartment = departmentDTO.EstadoDepartment;


            return department;
        }

        private RespuestaGenerica validarCampos(Department department)
        {
            List<String> listaErrores = new List<String>();

            Department department2 = _context.Departments.Where(
                 s => s.CodigoDepartment == department.CodigoDepartment).FirstOrDefault<Department>();

            if (department2 != null)
            {
                listaErrores.Add(Error.DEPARTMENT_ALREADY_REGISTERED);
            }

            if (listaErrores.Count() == 0)
            {
                return null;
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Validation error", listaErrores);
        }

        public RespuestaGenerica checkDepartment(string code)
        {
            try
            {
                Department department = _context.Departments.Where(
                s => s.CodigoDepartment == code).FirstOrDefault<Department>();

                if (department != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Department en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Department disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }
    }
}