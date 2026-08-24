using AutoMapper;
using SAIH_Backend.Datos.Entidades;
using SAIH_Backend.Servicios.Clases_estaticas;
using SAIH_Backend.Servicios.Clases_Estaticas;
using SAIH_Backend.Servicios.DTO;
using SAIH_Backend.Servicios.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiSAIH.Models;
using WebApiSAIH.Models.Entidades;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH.Services.Implementacion
{
    public class ServicioEstadistica : IServicioEstadistica
    {
        private ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioEstadistica(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RespuestaGenerica activeEmployees()
        {
            try
            {
                List<Employee> employeesActivos = _context.Employees.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of active employees in the system: ",
                    Object = employeesActivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
                
        }

        public RespuestaGenerica inactiveEmployees()
        {
            try
            {
                List<Employee> employeesInactivos = _context.Employees.Where(
                s => s.Status == Status.INACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of inactive employees in the system: ",
                    Object = employeesInactivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica totalRegisteredEmployees()
        {
            try
            {
                List<Employee> employeesTotal = _context.Employees.ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Total registered employees: ",
                    Object = employeesTotal.Count()
                };
                
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica completedTasks(String name)
        {
            try
            {
                var taskesCompletadas2 = from a in _context.Tasks 
                                              join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                              join ar in _context.Regions on pl.FK_idRegion2
                                              equals ar.PK_IdRegion join par in _context.Sites on
                                              ar.PK_IdRegion equals par.FK_idRegion1
                                              where par.Name == name && 
                                              a.Status == Status.COMPLETED && pl.Status == Status.ACTIVE
                                              select new
                                              {
                                                  a
                                              };

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of completed tasks at the site " + name + ": ",
                    Object = taskesCompletadas2.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica assignedProjects(String nombreAsp)
        {
            try
            {
                Site parque = _context.Sites.Where(s => s.Name == nombreAsp)
                    .FirstOrDefault<Site>();

                List<Employee> employees = _context.Employees.Where(s => s.FK_idSite1 == parque.PK_IdSite).ToList();

                List<Project> planesTrabajo = null;

                Project project = null;

                for (int i = 0; i < employees.Count(); i++)
                {
                    project = _context.Projects.Where(s => s.Fk_IdEmployee1 == employees[i].PK_idEmployee).FirstOrDefault<Project>();

                    if (project != null)
                    {
                        planesTrabajo.Add(project);
                    }
                }
             
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of projects: ",
                    Object = planesTrabajo.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica activeEmployeesBySite(String nombreAsp)
        {
            try
            {
                Site site = _context.Sites.Where(
                s => s.Name == nombreAsp).FirstOrDefault<Site>();

                List<Employee> employeesActivos = _context.Employees.Where(
                s => s.Status == Status.ACTIVE && s.FK_idSite1 == site.PK_IdSite).ToList();
                
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of active employees at this site: ",
                    Object = employeesActivos.Count()
                };
            }
            catch(Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica countSiteManagers()
        {
            try
            {
                Role rolAdminParque = _context.EmployeeRoles.Where(
                    s => s.Name == Roles.ROLE_SITE_MANAGER).FirstOrDefault<Role>();

                List<Employee> employeesAdministradorParque = _context.Employees.Where(
                    s => s.FK_idRole1 == rolAdminParque.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of Site Managers: ",
                    Object = employeesAdministradorParque.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica countEmployees()
        {
            try
            {
                Role rolGuardaparque = _context.EmployeeRoles.Where(
                    s => s.Name == Roles.ROLE_EMPLOYEE).FirstOrDefault<Role>();

                List<Employee> employeesGuardaparque = _context.Employees.Where(
                    s => s.FK_idRole1 == rolGuardaparque.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of active employees at this site: ",
                    Object = employeesGuardaparque.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica countAdmins()
        {
            try
            {
                Role rolAdminTI = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_ADMIN).FirstOrDefault<Role>();

                List<Employee> employeesAdminTI = _context.Employees.Where(
                s => s.FK_idRole1 == rolAdminTI.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of Admin employees: ",
                    Object = employeesAdminTI.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica countSupervisors()
        {
            try
            {
                Role rolSupervisor = _context.EmployeeRoles.Where(
                s => s.Name == Roles.ROLE_SUPERVISOR).FirstOrDefault<Role>();

                List<Employee> employeesSupervisor = _context.Employees.Where(
                s => s.FK_idRole1 == rolSupervisor.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of Supervisor employees: ",
                    Object = employeesSupervisor.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica activeSites()
        {
            try
            {
                List<Site> siteesActivos = _context.Sites.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of active sites in the system: ",
                    Object = siteesActivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica activeRegions()
        {
            try
            {
                List<Region> areasConservacionActivas = _context.Regions.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of active regions in the system: ",
                    Object = areasConservacionActivas.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfCompletedTasksBySite(string name)
        {
            try
            {
                var taskesCompletadas = from a in _context.Tasks
                                              join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                              join ar in _context.Regions on pl.FK_idRegion2
                                              equals ar.PK_IdRegion
                                              join par in _context.Sites on
                                              ar.PK_IdRegion equals par.FK_idRegion1
                                              where par.Name == name &&
                                              a.Status == Status.COMPLETED && pl.Status == Status.ACTIVE
                                             select new
                                              {
                                                  a
                                              };

                var taskesTotal = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join ar in _context.Regions on pl.FK_idRegion2
                                             equals ar.PK_IdRegion
                                             join par in _context.Sites on
                                             ar.PK_IdRegion equals par.FK_idRegion1
                                             where par.Name == name && pl.Status == Status.ACTIVE
                                       select new
                                             {
                                                 a
                                             };

                Decimal dec1 = new Decimal((double)taskesCompletadas.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfCompletedTasksBySite = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of completed tasks at the site " + name + ": ",
                        Object = percentageOfCompletedTasksBySite
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of completed tasks at the site " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica completedProjects(string name)
        {
            try
            {
                var planesCompletados = from  pl in _context.Projects join ar in _context.Regions on
                                              pl.FK_idRegion2 equals ar.PK_IdRegion join par in
                                              _context.Sites on ar.PK_IdRegion equals 
                                              par.FK_idRegion1
                                              where par.Name == name &&
                                              pl.Progress == Status.COMPLETED && pl.Status == Status.ACTIVE
                                        select new
                                              {
                                                  pl
                                              };

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Number of completed projects at the site " + name + ": ",
                    Object = planesCompletados.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfInProgressTasksBySite(string name)
        {
            try
            {
                var taskesEnCurso = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join ar in _context.Regions on pl.FK_idRegion2
                                             equals ar.PK_IdRegion
                                             join par in _context.Sites on
                                             ar.PK_IdRegion equals par.FK_idRegion1
                                             where par.Name == name &&
                                             a.Status == Status.IN_PROGRESS && pl.Status == Status.ACTIVE
                                         select new
                                             {
                                                 a
                                             };

                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join ar in _context.Regions on pl.FK_idRegion2
                                       equals ar.PK_IdRegion
                                       join par in _context.Sites on
                                       ar.PK_IdRegion equals par.FK_idRegion1
                                       where par.Name == name && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesEnCurso.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfInProgressTasksBySite = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of in-progress tasks at the site " + name + ": ",
                        Object = percentageOfInProgressTasksBySite
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of in-progress tasks at the site " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfPendingTasksBySite(string name)
        {
            try
            {
                var taskesPendientes = from a in _context.Tasks
                                         join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                         join ar in _context.Regions on pl.FK_idRegion2
                                         equals ar.PK_IdRegion
                                         join par in _context.Sites on
                                         ar.PK_IdRegion equals par.FK_idRegion1
                                         where par.Name == name &&
                                         a.Status == Status.PENDING && pl.Status == Status.ACTIVE
                                            select new
                                         {
                                             a
                                         };

                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join ar in _context.Regions on pl.FK_idRegion2
                                       equals ar.PK_IdRegion
                                       join par in _context.Sites on
                                       ar.PK_IdRegion equals par.FK_idRegion1
                                       where par.Name == name && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesPendientes.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfPendingTasksBySite = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks at the site " + name + ": ",
                        Object = percentageOfPendingTasksBySite
                    };
                } 
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks at the site " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfCompletedTasksByEmployee(string nationalId)
        {
            try
            {
                var taskesCompletadas = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join u in _context.Employees on pl.Fk_IdEmployee1
                                             equals u.PK_idEmployee
                                             where u.NationalId == nationalId &&
                                             a.Status == Status.COMPLETED && pl.Status == Status.ACTIVE
                                             select new
                                             {
                                                 a
                                             };

                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join u in _context.Employees on pl.Fk_IdEmployee1
                                       equals u.PK_idEmployee
                                       where u.NationalId == nationalId && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesCompletadas.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfCompletedTasksByEmployee = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = percentageOfCompletedTasksByEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfInProgressTasksByEmployee(string nationalId)
        {
            try
            {
                var taskesEnCurso = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join u in _context.Employees on pl.Fk_IdEmployee1
                                             equals u.PK_idEmployee
                                             where u.NationalId == nationalId &&
                                             a.Status == Status.IN_PROGRESS && pl.Status == Status.ACTIVE
                                             select new
                                             {
                                                 a
                                             };

                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join u in _context.Employees on pl.Fk_IdEmployee1
                                       equals u.PK_idEmployee
                                       where u.NationalId == nationalId && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesEnCurso.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfInProgressTasksByEmployee = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of in-progress tasks for employee " + nationalId + ": ",
                        Object = percentageOfInProgressTasksByEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of in-progress tasks for employee " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfPendingTasksByEmployee(string nationalId)
        {
            try
            {
                var taskesPendientes = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join u in _context.Employees on pl.Fk_IdEmployee1
                                             equals u.PK_idEmployee
                                             where u.NationalId == nationalId &&
                                             a.Status == Status.PENDING && pl.Status == Status.ACTIVE
                                             select new
                                             {
                                                 a
                                             };

                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join u in _context.Employees on pl.Fk_IdEmployee1
                                       equals u.PK_idEmployee
                                       where u.NationalId == nationalId && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesPendientes.Count());
                Decimal dec2 = new Decimal((double)taskesTotal.Count());

                if (dec2 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1, dec2);
                    decimal percentageOfPendingTasksByEmployee = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = percentageOfPendingTasksByEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica percentageOfTasksByEmployee(string nationalId)
        {
            try
            {
                var taskesCompletadas = from a in _context.Tasks
                                             join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                             join u in _context.Employees on pl.Fk_IdEmployee1
                                             equals u.PK_idEmployee
                                             where u.NationalId == nationalId &&
                                             a.Status == Status.COMPLETED && pl.Status == Status.ACTIVE
                                             select new
                                             {
                                                 a
                                             };

                var taskesEnCurso = from a in _context.Tasks
                                         join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                         join u in _context.Employees on pl.Fk_IdEmployee1
                                         equals u.PK_idEmployee
                                         where u.NationalId == nationalId &&
                                         a.Status == Status.IN_PROGRESS && pl.Status == Status.ACTIVE
                                         select new
                                         {
                                             a
                                         };
                 
                var taskesTotal = from a in _context.Tasks
                                       join pl in _context.Projects on a.FK_idProject equals pl.PK_idProject
                                       join u in _context.Employees on pl.Fk_IdEmployee1
                                       equals u.PK_idEmployee
                                       where u.NationalId == nationalId && pl.Status == Status.ACTIVE
                                       select new
                                       {
                                           a
                                       };

                Decimal dec1 = new Decimal((double)taskesCompletadas.Count());
                Decimal dec2 = new Decimal((double)taskesEnCurso.Count() / 2);
                Decimal dec3 = new Decimal((double)taskesTotal.Count());

                if (dec3 != 0)
                {
                    decimal percentage = Decimal.Divide(dec1 + dec2, dec3);
                    decimal percentageOfTasksByEmployee = percentage * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = percentageOfTasksByEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Percentage of pending tasks for employee " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "There was a server error",
                    Object = e.Message
                };
            }
        }
    }
}
