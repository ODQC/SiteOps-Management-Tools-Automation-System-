import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { forkJoin } from 'rxjs';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { RoleService } from 'src/app/services/Role/role.service';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { SiteService } from 'src/app/services/Site/site.service';
import { ResourceService } from 'src/app/services/Resource/resource.service';
import { GoalService } from 'src/app/services/Goal/goal.service';
import { ProjectService } from 'src/app/services/Project/project.service';
import { TaskService } from 'src/app/services/Task/task.service';
import { DeliverableService } from 'src/app/services/Deliverable/deliverable.service';
import { Employees } from 'src/app/models/employees';
import { IProject } from 'src/app/interfaces/IProject';
import { ITask } from 'src/app/interfaces/ITask';
import { IDeliverable } from 'src/app/interfaces/IDeliverable';

interface ConteoRol {
  name: string;
  cantidad: number;
}

interface EventoBitacora {
  descripcion: string;
  fecha: string;
}

interface PlanResumen {
  code: string;
  progress: number;
  responsable: string;
}

interface PlanPorVencer extends PlanResumen {
  diasRestantes: number;
}

interface DeliverableResumen {
  nombre: string;
  task: string;
  responsable?: string;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class DashboardComponent implements OnInit {
  rol: string = '';
  cargando = true;

  // Admin
  employeesPorRol: ConteoRol[] = [];
  employeesActivos = 0;
  employeesInactivos = 0;
  ultimosEventos: EventoBitacora[] = [];
  areasActivas = 0;
  parquesActivos = 0;
  resourcesActivas = 0;

  // Site Manager
  progressPromedio = 0;
  planes: PlanResumen[] = [];
  planesPorVencer: PlanPorVencer[] = [];
  goalsActivos = 0;
  goalsInhabilitados = 0;
  teamTotal = 0;
  teamActive = 0;

  // Employee
  miPlan: (PlanResumen & { diasRestantes: number; status?: string }) | null = null;
  taskesPendientes = 0;
  taskesCompletadas = 0;
  misDeliverables: DeliverableResumen[] = [];

  // Supervisor
  deliverablesRecientes: DeliverableResumen[] = [];
  planesPorResponsable: PlanResumen[] = [];
  taskesSinDeliverable = 0;

  constructor(
    public permissions: PermissionsService,
    private employeesService: EmployeesService,
    private roleService: RoleService,
    private auditLogService: AuditLogService,
    private areaService: RegionService,
    private parqueService: SiteService,
    private resourceService: ResourceService,
    private goalService: GoalService,
    private planService: ProjectService,
    private taskService: TaskService,
    private deliverableService: DeliverableService
  ) {}

  ngOnInit(): void {
    this.rol = this.permissions.rolActual();

    switch (this.rol) {
      case 'Admin':
        this.cargarAdministradorTI();
        break;
      case 'Site Manager':
        this.cargarAdministradorParque();
        break;
      case 'Employee':
        this.cargarEmployee();
        break;
      case 'Supervisor':
        this.cargarSupervisor();
        break;
      default:
        this.cargando = false;
    }
  }

  private diasRestantes(endDate?: string): number {
    if (!endDate) return 0;
    return Math.ceil((new Date(endDate).getTime() - Date.now()) / 86400000);
  }

  private nombreCompleto(u: Employees): string {
    return `${u.firstName} ${u.lastName}`;
  }

  private cargarAdministradorTI(): void {
    forkJoin({
      employees: this.employeesService.obtenerEmployees(),
      roles: this.roleService.obtenerRol(),
      eventos: this.auditLogService.obtenerAuditLogs(),
      areas: this.areaService.obtenerRegion(),
      parques: this.parqueService.obtenerTodosSite(),
      resources: this.resourceService.obtenerResources()
    }).subscribe(({ employees, roles, eventos, areas, parques, resources }) => {
      this.employeesPorRol = roles.map(r => ({
        name: r.name,
        cantidad: employees.filter(u => u.fK_idRole1 === r.pK_idRole).length
      }));
      this.employeesActivos = employees.filter(u => u.status === 'Active').length;
      this.employeesInactivos = employees.length - this.employeesActivos;

      this.ultimosEventos = eventos
        .slice()
        .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
        .slice(0, 5)
        .map(e => ({ descripcion: e.description, fecha: e.date }));

      this.areasActivas = areas.filter(a => a.status === 'Active').length;
      this.parquesActivos = parques.filter(p => p.status === 'Active').length;
      this.resourcesActivas = resources.filter(h => h.status === 'Active').length;

      this.cargando = false;
    });
  }

  private cargarAdministradorParque(): void {
    forkJoin({
      planes: this.planService.obtenerPlanesTrabajo(),
      employees: this.employeesService.obtenerEmployees(),
      roles: this.roleService.obtenerRol(),
      goals: this.goalService.obtenerGoals()
    }).subscribe(({ planes, employees, roles, goals }) => {
      const nombreResponsable = (id?: number) => {
        const u = employees.find(x => x.pK_idEmployee === id);
        return u ? this.nombreCompleto(u) : 'N/A';
      };

      this.planes = planes.map(p => ({
        code: p.code || '',
        progress: parseInt(p.progress || '0', 10) || 0,
        responsable: nombreResponsable(p.fk_IdEmployee1)
      }));
      this.progressPromedio = this.planes.length
        ? Math.round(this.planes.reduce((sum, p) => sum + p.progress, 0) / this.planes.length)
        : 0;

      this.planesPorVencer = planes
        .filter(p => p.status !== 'Completada')
        .map(p => ({
          code: p.code || '',
          progress: parseInt(p.progress || '0', 10) || 0,
          responsable: nombreResponsable(p.fk_IdEmployee1),
          diasRestantes: this.diasRestantes(p.endDate)
        }))
        .filter(p => p.diasRestantes >= 0 && p.diasRestantes <= 30)
        .sort((a, b) => a.diasRestantes - b.diasRestantes);

      const anioActual = new Date().getFullYear().toString();
      const goalsDelAnio = goals.filter(o => o.year === anioActual);
      this.goalsActivos = goalsDelAnio.filter(o => o.status === 'Active').length;
      this.goalsInhabilitados = goalsDelAnio.filter(o => o.status !== 'Active').length;

      const rolEmployee = roles.find(r => r.name === 'Employee');
      const teamMembers = employees.filter(u => u.fK_idRole1 === rolEmployee?.pK_idRole);
      this.teamTotal = teamMembers.length;
      this.teamActive = teamMembers.filter(u => u.status === 'Active').length;

      this.cargando = false;
    });
  }

  private cargarEmployee(): void {
    forkJoin({
      planes: this.planService.obtenerPlanesTrabajo(),
      employees: this.employeesService.obtenerEmployees()
    }).subscribe(({ planes, employees }) => {
      const profile = this.employeesService.cargarProfileEmployee();
      const propio = employees.find(u => u.email === profile.Email);
      const plan = propio ? planes.find(p => p.fk_IdEmployee1 === propio.pK_idEmployee) : undefined;

      if (!plan) {
        this.cargando = false;
        return;
      }

      this.miPlan = {
        code: plan.code || '',
        progress: parseInt(plan.progress || '0', 10) || 0,
        responsable: propio ? this.nombreCompleto(propio) : '',
        diasRestantes: this.diasRestantes(plan.endDate),
        status: plan.status
      };

      this.taskService.obtenerTaskesPorPlan(plan.pK_idProject).subscribe((res: any) => {
        const tasks: ITask[] = res?.object || res || [];
        this.taskesPendientes = tasks.filter(a => a.status === 'Pending').length;
        this.taskesCompletadas = tasks.filter(a => a.status === 'Completed').length;

        const idsTaskes = new Set(tasks.map(a => a.pK_idTaskItem));

        this.deliverableService.obtenerDeliverables().subscribe(deliverables => {
          this.misDeliverables = deliverables
            .filter(e => idsTaskes.has(e.fK_idTask1 || -1))
            .sort((a, b) => b.pK_idDeliverable - a.pK_idDeliverable)
            .slice(0, 5)
            .map(e => ({
              nombre: e.name || e.code || '',
              task: tasks.find(a => a.pK_idTaskItem === e.fK_idTask1)?.name || 'N/A'
            }));

          this.cargando = false;
        });
      });
    });
  }

  private cargarSupervisor(): void {
    forkJoin({
      deliverables: this.deliverableService.obtenerDeliverables(),
      tasks: this.taskService.obtenerTaskes(),
      planes: this.planService.obtenerPlanesTrabajo(),
      employees: this.employeesService.obtenerEmployees()
    }).subscribe(({ deliverables, tasks, planes, employees }: {
      deliverables: IDeliverable[]; tasks: ITask[]; planes: IProject[]; employees: Employees[];
    }) => {
      const nombreResponsable = (id?: number) => {
        const u = employees.find(x => x.pK_idEmployee === id);
        return u ? this.nombreCompleto(u) : 'N/A';
      };
      const responsableDeTask = (idTask?: number): string => {
        const task = tasks.find(a => a.pK_idTaskItem === idTask);
        const plan = planes.find(p => p.pK_idProject === task?.fK_idProject);
        return nombreResponsable(plan?.fk_IdEmployee1);
      };

      this.deliverablesRecientes = deliverables
        .slice()
        .sort((a, b) => b.pK_idDeliverable - a.pK_idDeliverable)
        .slice(0, 5)
        .map(e => ({
          nombre: e.name || e.code || '',
          task: tasks.find(a => a.pK_idTaskItem === e.fK_idTask1)?.name || 'N/A',
          responsable: responsableDeTask(e.fK_idTask1)
        }));

      this.planesPorResponsable = planes
        .map(p => ({
          code: p.code || '',
          progress: parseInt(p.progress || '0', 10) || 0,
          responsable: nombreResponsable(p.fk_IdEmployee1)
        }))
        .sort((a, b) => b.progress - a.progress);

      const idsConDeliverable = new Set(deliverables.map(e => e.fK_idTask1));
      this.taskesSinDeliverable = tasks.filter(a => !idsConDeliverable.has(a.pK_idTaskItem)).length;

      this.cargando = false;
    });
  }
}
