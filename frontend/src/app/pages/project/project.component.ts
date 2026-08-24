import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ProjectService } from 'src/app/services/Project/project.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IProject } from 'src/app/interfaces/IProject';
import { IRegion } from 'src/app/interfaces/IRegion';
import { Employees } from 'src/app/models/employees';
import { ProjectDialogComponent } from './project-dialog/project-dialog.component';
import { TasksDialogComponent } from './tasks-dialog/tasks-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class ProjectComponent implements OnInit {
  planes: IProject[] = [];
  areasList: IRegion[] = [];
  employeesList: Employees[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'responsable', 'area', 'progress', 'tasks', 'estado'];

  constructor(
    private planService: ProjectService,
    private areaService: RegionService,
    private employeesService: EmployeesService,
    private toastr: ToastService,
    private dialog: MatDialog,
    public permissions: PermissionsService
  ) {
    this.soloLectura = !this.permissions.esCompleto('planesTrabajo');
  }

  ngOnInit(): void {
    this.cargar();
    this.areaService.obtenerRegion().subscribe(areas => this.areasList = areas);
    this.employeesService.obtenerEmployees().subscribe(employees => this.employeesList = employees);
    this.planService.refresh$.subscribe(() => this.cargar());
  }

  cargar(): void {
    this.planService.obtenerPlanesTrabajo().subscribe(p => this.planes = p);
  }

  name(id?: number): string {
    return this.areasList.find(a => a.pK_IdRegion === id)?.name || 'N/A';
  }

  nombreResponsable(id?: number): string {
    const u = this.employeesList.find(u => u.pK_idEmployee === id);
    return u ? `${u.firstName} ${u.lastName}` : 'N/A';
  }

  get planesFiltrados(): IProject[] {
    let base = this.planes;
    if (this.permissions.rolActual() === 'Employee') {
      const profile = this.employeesService.cargarProfileEmployee();
      const propio = this.employeesList.find(u => u.email === profile.Email);
      base = base.filter(p => propio && p.fk_IdEmployee1 === propio.pK_idEmployee);
    }
    const texto = this.search.trim().toLowerCase();
    if (!texto) return base;
    return base.filter(p => p.code?.toLowerCase().includes(texto));
  }

  nuevo(): void {
    this.dialog.open(ProjectDialogComponent, { width: '640px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  editar(plan: IProject): void {
    this.dialog.open(ProjectDialogComponent, { width: '640px', maxWidth: '95vw', data: plan })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  verTaskes(plan: IProject): void {
    this.dialog.open(TasksDialogComponent, { width: '640px', maxWidth: '95vw', data: plan });
  }

  cambiarEstado(plan: IProject): void {
    if (!plan.fk_IdEmployee1) return;
    this.planService.cambiarEstado(plan.pK_idProject, plan.fk_IdEmployee1).subscribe({
      next: () => { this.toastr.success('Estado actualizado', 'Éxito'); this.cargar(); },
      error: () => this.toastr.error('No se pudo cambiar el estado', 'Error')
    });
  }
}
