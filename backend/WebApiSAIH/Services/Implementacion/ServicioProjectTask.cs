using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAIH_Backend.Servicios.Clases_estaticas;
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
    public class ServicioProjectTask : IServicioProjectTask
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioProjectTask(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica deleteProjectTask(long idProjectTaskDTO)
        {
            ProjectTask projectTask = _context.ProjectTasks.Where(
                s => s.PK_idProjectTask == idProjectTaskDTO).FirstOrDefault<ProjectTask>();

            if (projectTask == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Project task not found", null);
            }

            try
            {
                _context.ProjectTasks.Remove(projectTask);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project Task" + projectTask.PK_idProjectTask + " deleted successfully", 
                _mapper.Map<ProjectTaskDTO>(projectTask));
        }

        public RespuestaGenerica createProjectTask(ProjectTaskDTO projectTaskDTO)
        {
            try
            {
                ProjectTask projectTask = _mapper.Map<ProjectTask>(projectTaskDTO);

                _context.ProjectTasks.Add(projectTask);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Project task registered", _mapper.Map<ProjectTaskDTO>(projectTask));
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

        public RespuestaGenerica createProjectTaskList(List<ProjectTaskDTO> projectTaskDTOs)
        {
            try
            {
                List<ProjectTask> listProjectTask2 = _mapper.Map<List<ProjectTask>>(projectTaskDTOs);
                List<ProjectTask> listProjectTask1 = new List<ProjectTask>();

                for (int i = 0; i < listProjectTask2.Count; i++)
                {
                    listProjectTask1.Add(_context.ProjectTasks.Add(listProjectTask2[i]).Entity);
                }

                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Project tasks registered", 
                    _mapper.Map<List<ProjectTaskDTO>>(listProjectTask1));
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

        public RespuestaGenerica getProjectTask()
        {
            List<ProjectTask> projectTasks = null;
            try
            {
                projectTasks = _context.ProjectTasks.ToList();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Server error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project tasks listed", _mapper.Map<List<ProjectTaskDTO>>(projectTasks));
        }

        public RespuestaGenerica checkExistingLink(long fk_idProject, long fk_idTask)
        {
            try
            {
                ProjectTask projectTask = _context.ProjectTasks.Where(
                    s => s.FK_idProject == fk_idProject && s.FK_idTask == fk_idTask).FirstOrDefault<ProjectTask>();

                if (projectTask != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project task already exists", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Project task does not exist", false);
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
    }
}
