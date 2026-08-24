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
    public class ServicioTaskItem : IServicioTaskItem
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioTaskItem(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica toggleTaskStatus(long idTaskDTO)
        {
            TaskItem task = _context.Tasks.Where(
                s => s.PK_idTaskItem == idTaskDTO).FirstOrDefault<TaskItem>();

            if (task == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
            }

            if (task.TaskStatus == Status.ACTIVE)
            {
                task.TaskStatus = Status.INACTIVE;
            }
            else if (task.TaskStatus == Status.INACTIVE)
            {
                task.TaskStatus = Status.ACTIVE;
            }

            try
            {
                _context.Update(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The task's status has been changed", _mapper.Map<TaskItemDTO>(task));
        }

        public RespuestaGenerica deleteTask(long idTaskDTO)
        {
            TaskItem task = _context.Tasks.Where(
                s => s.PK_idTaskItem == idTaskDTO).FirstOrDefault<TaskItem>();

            if (task == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
            }

            try
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.InnerException.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Task: " + task.Code + " deleted successfully", _mapper.Map<TaskItemDTO>(task));
        }

        public RespuestaGenerica createTask(TaskItemDTO taskDTO)
        {
            try
            {
                TaskItem task = _context.Tasks.Where(
                 s => s.Code == taskDTO.Code && s.FK_idProject == taskDTO.FK_idProject).FirstOrDefault<TaskItem>();

                if (task != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This task is already registered in the system", null);
                }

                TaskItem task2 = _mapper.Map<TaskItem>(taskDTO);

                _context.Tasks.Add(task2);
                _context.SaveChanges();

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_CREATED, "Task registered", _mapper.Map<TaskItemDTO>(task2));
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

        public RespuestaGenerica updateTask(long pk_IdTask, TaskItemDTO taskDTO)
        {
            try
            {
                TaskItem task = _context.Tasks.Where(
                s => s.PK_idTaskItem == pk_IdTask).FirstOrDefault<TaskItem>();

                if (task == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", "No task was updated");
                }

                TaskItem taskCodigo = _context.Tasks.Where(
                s => s.Code == taskDTO.Code && s.FK_idProject == taskDTO.FK_idProject).FirstOrDefault<TaskItem>();

                if (taskCodigo != null && (taskCodigo != task))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "This task code is already in use", "");
                }

                task = covertirDTOAEntidad(task, taskDTO);
                _context.Update(task);
                _context.SaveChanges();
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Task updated", _mapper.Map<TaskItemDTO>(task));
            }
            catch (Exception e)
            {
                if (!TaskExists(pk_IdTask))
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
                }
                else
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
                }
            }
        }

        private bool TaskExists(long pk_IdTask)
        {
            return _context.Tasks.Any(e => e.PK_idTaskItem == pk_IdTask);
        }

        public RespuestaGenerica getTask(long idTaskDTO)
        {
            TaskItem task = _context.Tasks.Where(
                s => s.PK_idTaskItem == idTaskDTO).FirstOrDefault<TaskItem>();

            if (task == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "TaskItem", _mapper.Map<TaskItemDTO>(task));
        }

        public RespuestaGenerica getTasks()
        {
            List<TaskItem> task = _context.Tasks
                       .Where(s => s.Code != "N/A")
                       .ToList();

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Tasks listed", _mapper.Map<List<TaskItemDTO>>(task));
        }

        public RespuestaGenerica getNATaskId()
        {
            TaskItem task = _context.Tasks
                .Where(s => s.Code == "N/A").FirstOrDefault<TaskItem>();

            if (task == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "No N/A task found", null);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Task PK for N/A", task.PK_idTaskItem);
        }

        private TaskItem covertirDTOAEntidad(TaskItem task, TaskItemDTO taskDTO)
        {
            task.PK_idTaskItem = taskDTO.PK_idTaskItem;
            task.Name = taskDTO.Name;
            task.Code = task.Code;
            task.Notes = taskDTO.Notes;
            task.CompletionDate = taskDTO.CompletionDate;
            task.Collaborators = taskDTO.Collaborators;
            task.Fk_IdGoal1 = taskDTO.Fk_IdGoal1;
            task.Status = taskDTO.Status;
            
            return task;
        }

        public RespuestaGenerica projectsByTask(string code)
        {
            try
            {
                TaskItem task = _context.Tasks.Where(
                 s => s.Code == code).FirstOrDefault<TaskItem>();

                if (task == null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
                }

                List<ProjectTask> listProjectTask = _context.ProjectTasks.Where(
                    s => s.FK_idTask == task.PK_idTaskItem).ToList();

                List<Project> projects = new List<Project>();

                for (int i = 0; i < listProjectTask.Count; i++)
                {
                    projects.Add(_context.Projects.Where(
                        s => s.PK_idProject == listProjectTask[i].FK_idProject).FirstOrDefault<Project>());
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Projects associated with the task " + code,
                    _mapper.Map<List<ProjectDTO>>(projects));
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica checkTask(string code, long fk_idProject)
        {
            try
            {
                TaskItem task = _context.Tasks.Where(
                s => s.Code == code && s.FK_idProject == fk_idProject).FirstOrDefault<TaskItem>();

                if (task != null)
                {
                    return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo TaskItem en uso", true);
                }

                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "Codigo TaskItem disponible", false);
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }
        }

        public RespuestaGenerica tasksByProject(long fk_idProject)
        {
            try
            {
                List<TaskItem> tasks = _context.Tasks.Where(
                s => s.FK_idProject == fk_idProject).ToList();

                if (tasks == null)
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_NOT_FOUND,
                        Mensaje = "No tasks found for this project",
                        Object = null
                    };
                }

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Tasks associated with the project " + fk_idProject,
                    Object = _mapper.Map<List<TaskItemDTO>>(tasks)
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Internal Server Error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cambiarTaskStatus(long pk_idTaskItem, string estado)
        {
            TaskItem task = _context.Tasks.Where(
                s => s.PK_idTaskItem == pk_idTaskItem).FirstOrDefault<TaskItem>();

            if (task == null)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_NOT_FOUND, "Task not found", null);
            }

            if (estado != Status.COMPLETED && estado != Status.IN_PROGRESS && estado != Status.PENDING)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_BAD_REQUEST,
                    Mensaje = "The status is not valid",
                    Object = null
                };
            }

            task.Status = estado;

            try
            {
                _context.Update(task);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR, "Internal Server Error", e.Message);
            }

            return new RespuestaGenerica(CodigosEstadoHTTP.HTTP_STATUS_OK, "The task's status has been changed", _mapper.Map<TaskItemDTO>(task));
        }
    }
}
