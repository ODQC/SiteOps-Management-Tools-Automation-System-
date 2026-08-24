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

        public RespuestaGenerica employeesActivos()
        {
            try
            {
                List<Employee> employeesActivos = _context.Employees.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees activos en el sistema: ",
                    Object = employeesActivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
                
        }

        public RespuestaGenerica employeesInactivos()
        {
            try
            {
                List<Employee> employeesInactivos = _context.Employees.Where(
                s => s.Status == Status.INACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees inactivos en el sistema: ",
                    Object = employeesInactivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica employeesRegistradosTotal()
        {
            try
            {
                List<Employee> employeesTotal = _context.Employees.ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees registrados en total: ",
                    Object = employeesTotal.Count()
                };
                
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica taskesCompletadas(String name)
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
                    Mensaje = "Cantidad de tasks completadas en el parque " + name + ": ",
                    Object = taskesCompletadas2.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica planesDeTrabajoAsignados(String nombreAsp)
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
                    Mensaje = "La cantidad de planes de trabajo: ",
                    Object = planesTrabajo.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica employeesActivosXAsp(String nombreAsp)
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
                    Mensaje = "Cantidad de employees activos en este Parque Nacional: ",
                    Object = employeesActivos.Count()
                };
            }
            catch(Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cantidadAdministradorParque()
        {
            try
            {
                Role rolAdminParque = _context.Roles.Where(
                    s => s.Name == Roles.ROL_ADMINISTRADOR_PARQUE).FirstOrDefault<Role>();

                List<Employee> employeesAdministradorParque = _context.Employees.Where(
                    s => s.FK_idRole1 == rolAdminParque.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de administradores de Parque: ",
                    Object = employeesAdministradorParque.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cantidadGuardaparques()
        {
            try
            {
                Role rolGuardaparque = _context.Roles.Where(
                    s => s.Name == Roles.ROL_GUARDAPARQUE).FirstOrDefault<Role>();

                List<Employee> employeesGuardaparque = _context.Employees.Where(
                    s => s.FK_idRole1 == rolGuardaparque.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees activos en este Parque Nacional: ",
                    Object = employeesGuardaparque.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cantidadAdminTI()
        {
            try
            {
                Role rolAdminTI = _context.Roles.Where(
                s => s.Name == Roles.ROL_ADMINISTRADOR_TI).FirstOrDefault<Role>();

                List<Employee> employeesAdminTI = _context.Employees.Where(
                s => s.FK_idRole1 == rolAdminTI.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees admin TI: ",
                    Object = employeesAdminTI.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica cantidadSupervisores()
        {
            try
            {
                Role rolSupervisor = _context.Roles.Where(
                s => s.Name == Roles.ROL_SUPERVISOR).FirstOrDefault<Role>();

                List<Employee> employeesSupervisor = _context.Employees.Where(
                s => s.FK_idRole1 == rolSupervisor.PK_idRole).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de employees supervisores: ",
                    Object = employeesSupervisor.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica siteesActivos()
        {
            try
            {
                List<Site> siteesActivos = _context.Sites.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de parques de Nacionales activos en el sistema: ",
                    Object = siteesActivos.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica areasConservacionActivas()
        {
            try
            {
                List<Region> areasConservacionActivas = _context.Regions.Where(
                s => s.Status == Status.ACTIVE).ToList();

                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                    Mensaje = "Cantidad de areas de conservacion activas en el sistema: ",
                    Object = areasConservacionActivas.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesCompletadasParque(string name)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesCompletadasParque = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks completadas en el parque " + name + ": ",
                        Object = porcentajeTaskesCompletadasParque
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks completadas en el parque " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica planesCompletados(string name)
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
                    Mensaje = "Cantidad de planes completados en el parque " + name + ": ",
                    Object = planesCompletados.Count()
                };
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesEnProcesoParque(string name)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesEnProcesoParque = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks en proceso en el parque " + name + ": ",
                        Object = porcentajeTaskesEnProcesoParque
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks en proceso en el parque " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesPendientesParque(string name)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesPendientesParque = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el parque " + name + ": ",
                        Object = porcentajeTaskesPendientesParque
                    };
                } 
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el parque " + name + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesCompletadasEmployee(string nationalId)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesCompletadasEmployee = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el nationalId " + nationalId + ": ",
                        Object = porcentajeTaskesCompletadasEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el nationalId " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesEnProcesoEmployee(string nationalId)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesEnProcesoEmployee = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks en proceso en el  " + nationalId + ": ",
                        Object = porcentajeTaskesEnProcesoEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks en proceso en el  " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeTaskesPendientesEmployee(string nationalId)
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
                    decimal porcentaje = Decimal.Divide(dec1, dec2);
                    decimal porcentajeTaskesPendientesEmployee = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el  " + nationalId + ": ",
                        Object = porcentajeTaskesPendientesEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el  " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }

        public RespuestaGenerica porcentajeGeneralTaskesEmployee(string nationalId)
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
                    decimal porcentaje = Decimal.Divide(dec1 + dec2, dec3);
                    decimal porcentajeGeneralTaskesEmployee = porcentaje * 100;

                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el  " + nationalId + ": ",
                        Object = porcentajeGeneralTaskesEmployee
                    };
                }
                else
                {
                    return new RespuestaGenerica
                    {
                        Codigo = CodigosEstadoHTTP.HTTP_STATUS_OK,
                        Mensaje = "Porcentaje de tasks pendientes en el  " + nationalId + ": ",
                        Object = 0
                    };
                }
            }
            catch (Exception e)
            {
                return new RespuestaGenerica
                {
                    Codigo = CodigosEstadoHTTP.HTTP_INTERNAL_SERVER_ERROR,
                    Mensaje = "Hubo un error en el servidor",
                    Object = e.Message
                };
            }
        }
    }
}
