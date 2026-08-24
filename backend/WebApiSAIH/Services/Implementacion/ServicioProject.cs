using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.Errores;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.DTO;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioProject : IServicioProject
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioProject(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deshabilitarProject(long idProjectDTO, long pk_idEmployee)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == idProjectDTO).FirstOrDefault<Project>();

            List<Project> planesTrabajo = _context.Projects.Where(
                s => s.Fk_IdEmployee1 == pk_idEmployee).ToList();

            if (planesTrabajo.Count == 0)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "El employee no tiene planes asociados",
                    Object = null
                };
            }

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Plan de trabajo no encontrado", null);
            }

            if (project.Status == Status.INACTIVE)
            {
                for (int i = 0; i < planesTrabajo.Count; i++)
                {
                    if (planesTrabajo[i].Status == Status.ACTIVE)
                    {
                        return new RespuestaGenerica
                        {
                            Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                            Mensaje = "Ya existe un plan de trabajo activo",
                            Object = null
                        };
                    }
                    else
                    {
                        if (planesTrabajo[i].Status == Status.ACTIVE && project == planesTrabajo[i])
                        {
                            planesTrabajo[i].Status = Status.INACTIVE;
                        }
                        else if (planesTrabajo[i].Status == Status.INACTIVE && project == planesTrabajo[i])
                        {
                            planesTrabajo[i].Status = Status.ACTIVE;
                        }

                    }
                }
            }
            else
            {
                project.Status = Status.INACTIVE;
            }
            
            try
            {
                _context.Update(project);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El estado del plan de trabajo ha sido modificado", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica eliminarProject(long idProjectDTO)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == idProjectDTO).FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Plan de trabajo no encontrado", null);
            }

            try
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Plan de trabajo: " + project.Code + " eliminado exitosamente", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica guardarProject(ProjectDTO projectDTO)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.Code == projectDTO.Code && s.Fk_IdEmployee1 == projectDTO.Fk_IdEmployee1).FirstOrDefault<Project>();

                if (project != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El plan de trabajo ya esta registrado en el sistema", null);
                }

                Project project2 = _mapper.Map<Project>(projectDTO);

                _context.Projects.Add(project2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Plan de trabajo registrado", _mapper.Map<ProjectDTO>(project2));
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

        public RespuestaGenerica modificarProject(long pk_IdProject, ProjectDTO projectDTO)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.PK_idProject == pk_IdProject).FirstOrDefault<Project>();

                if (project == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Plan de trabajo no encontrado", "No se actualizo ningun plan de trabajo");
                }

                Project projectCodigo = _context.Projects.Where(
                s => s.Code == projectDTO.Code && s.Fk_IdEmployee1 == projectDTO.Fk_IdEmployee1).FirstOrDefault<Project>();

                if (projectCodigo != null && (projectCodigo != project))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El codigo del plan de trabajo ya está siendo utilizado", "");
                }

                project = covertirDTOAEntidad(project, projectDTO);
                _context.Update(project);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Plan de trabajo actualizado", _mapper.Map<ProjectDTO>(project));
            }
            catch (Exception e)
            {
                if (!ProjectExists(pk_IdProject))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Plan de trabajo no encontrado", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
                }
            }
        }

        private bool ProjectExists(long pk_Idproject)
        {
            return _context.Projects.Any(e => e.PK_idProject == pk_Idproject);
        }

        public RespuestaGenerica obtenerProject(long idProjectDTO)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == idProjectDTO).FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el plan de trabajo", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Plan de trabajo", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica obtenerProjects()
        {
            List<Project> projects = _context.Projects
                       .Where(s => s.Code != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Plan de trabajo desplegados", _mapper.Map<List<ProjectDTO>>(projects));
        }

        public RespuestaGenerica obtenerPKProjectNA()
        {
            Project project = _context.Projects
                .Where(s => s.Code == "N/A").FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el plan de trabajo con N/A", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "PK plan de trabajo con N/A", project.PK_idProject);
        }

        private Project covertirDTOAEntidad(Project project, ProjectDTO projectDTO)
        {
            project.Code = projectDTO.Code;
            project.Status = projectDTO.Status;
            project.StartDate = projectDTO.StartDate;
            project.EndDate = projectDTO.EndDate;
            project.FK_idRegion2 = projectDTO.FK_idRegion2;
            project.Fk_IdEmployee1 = projectDTO.Fk_IdEmployee1;
            project.PK_idProject = projectDTO.PK_idProject;
            project.Progress = projectDTO.Progress;

            return project;
        }

        public RespuestaGenerica taskesXproject(long pk_idProject)
        {
            try
            {
                Project project = _context.Projects.Where(
                 s => s.PK_idProject == pk_idProject).FirstOrDefault<Project>();

                if (project == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No se encontro el project", null);
                }

                List<ProjectTask> listProjectTask = _context.ProjectTasks.Where(
                    s => s.FK_idProject == project.PK_idProject).ToList();

                List<TaskItem> tasks = new List<TaskItem>();

                for (int i = 0; i < listProjectTask.Count; i++)
                {
                    tasks.Add(_context.Tasks.Where(
                        s => s.PK_idTaskItem == listProjectTask[i].FK_idTask).FirstOrDefault<TaskItem>());
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Tasks asociadas al plan de trabajo " + pk_idProject,
                    _mapper.Map<List<TaskItemDTO>>(tasks));
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica verificarProject(string codigoProject, long fk_idEmployee)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.Code == codigoProject && s.Fk_IdEmployee1 == fk_idEmployee).FirstOrDefault<Project>();

                if (project != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Project en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Project disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }
        }

        public RespuestaGenerica projectXemployee(long fk_idEmployee)
        {
            try
            {
                List<Project> planesTrabajo = _context.Projects.Where(
                s => s.Fk_IdEmployee1 == fk_idEmployee).ToList();

                if (planesTrabajo == null)
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_NOT_FOUND,
                        Mensaje = "No se encontro el project",
                        Object = null
                    };
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Plan de trabajo asociado al employee " +  fk_idEmployee,
                    Object = _mapper.Map<List<ProjectDTO>>(planesTrabajo)
                };
            }
            catch(Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Error Interno del Servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cambiarProgesoPlan(long pk_idProject, string progress)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == pk_idProject).FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Plan trabajo no encontrado", null);
            }

            if (progress != Status.COMPLETED && progress != Status.IN_PROGRESS && progress != Status.PENDING)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Mensaje = "El estado no es valido",
                    Object = null
                };
            }

            project.Progress = progress;

            try
            {
                _context.Update(project);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Error Interno del Servidor", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "El progress del plan trabajo ha sido modificado", _mapper.Map<ProjectDTO>(project));
        }
    }
}
