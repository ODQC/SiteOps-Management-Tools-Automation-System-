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

        public RespuestaGenerica toggleProjectStatus(long idProjectDTO, long pk_idEmployee)
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
                    Mensaje = "This employee has no associated projects",
                    Object = null
                };
            }

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
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
                            Mensaje = "There is already an active project",
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The project's status has been changed", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica deleteProject(long idProjectDTO)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == idProjectDTO).FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
            }

            try
            {
                _context.Projects.Remove(project);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project: " + project.Code + " deleted successfully", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica createProject(ProjectDTO projectDTO)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.Code == projectDTO.Code && s.Fk_IdEmployee1 == projectDTO.Fk_IdEmployee1).FirstOrDefault<Project>();

                if (project != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This project is already registered in the system", null);
                }

                Project project2 = _mapper.Map<Project>(projectDTO);

                _context.Projects.Add(project2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Project registered", _mapper.Map<ProjectDTO>(project2));
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

        public RespuestaGenerica updateProject(long pk_IdProject, ProjectDTO projectDTO)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.PK_idProject == pk_IdProject).FirstOrDefault<Project>();

                if (project == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", "No project was updated");
                }

                Project projectCodigo = _context.Projects.Where(
                s => s.Code == projectDTO.Code && s.Fk_IdEmployee1 == projectDTO.Fk_IdEmployee1).FirstOrDefault<Project>();

                if (projectCodigo != null && (projectCodigo != project))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This project code is already in use", "");
                }

                project = covertirDTOAEntidad(project, projectDTO);
                _context.Update(project);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project updated", _mapper.Map<ProjectDTO>(project));
            }
            catch (Exception e)
            {
                if (!ProjectExists(pk_IdProject))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool ProjectExists(long pk_Idproject)
        {
            return _context.Projects.Any(e => e.PK_idProject == pk_Idproject);
        }

        public RespuestaGenerica getProject(long idProjectDTO)
        {
            Project project = _context.Projects.Where(
                s => s.PK_idProject == idProjectDTO).FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project", _mapper.Map<ProjectDTO>(project));
        }

        public RespuestaGenerica getProjects()
        {
            List<Project> projects = _context.Projects
                       .Where(s => s.Code != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Projects listed", _mapper.Map<List<ProjectDTO>>(projects));
        }

        public RespuestaGenerica getNAProjectId()
        {
            Project project = _context.Projects
                .Where(s => s.Code == "N/A").FirstOrDefault<Project>();

            if (project == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A project found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project PK for N/A", project.PK_idProject);
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

        public RespuestaGenerica tasksByProject(long pk_idProject)
        {
            try
            {
                Project project = _context.Projects.Where(
                 s => s.PK_idProject == pk_idProject).FirstOrDefault<Project>();

                if (project == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
                }

                List<ProjectTask> listProjectTask = _context.ProjectTasks.Where(
                    s => s.FK_idProject == project.PK_idProject).ToList();

                List<TaskItem> tasks = new List<TaskItem>();

                for (int i = 0; i < listProjectTask.Count; i++)
                {
                    tasks.Add(_context.Tasks.Where(
                        s => s.PK_idTaskItem == listProjectTask[i].FK_idTask).FirstOrDefault<TaskItem>());
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Tasks associated with the project " + pk_idProject,
                    _mapper.Map<List<TaskItemDTO>>(tasks));
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica checkProject(string code, long fk_idEmployee)
        {
            try
            {
                Project project = _context.Projects.Where(
                s => s.Code == code && s.Fk_IdEmployee1 == fk_idEmployee).FirstOrDefault<Project>();

                if (project != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Project en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo Project disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica projectByEmployee(long fk_idEmployee)
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
                        Mensaje = "Project not found",
                        Object = null
                    };
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Project associated with employee " +  fk_idEmployee,
                    Object = _mapper.Map<List<ProjectDTO>>(planesTrabajo)
                };
            }
            catch(Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Internal Server Error",
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project not found", null);
            }

            if (progress != Status.COMPLETED && progress != Status.IN_PROGRESS && progress != Status.PENDING)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Mensaje = "The status is not valid",
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
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The project's progress has been changed", _mapper.Map<ProjectDTO>(project));
        }
    }
}
