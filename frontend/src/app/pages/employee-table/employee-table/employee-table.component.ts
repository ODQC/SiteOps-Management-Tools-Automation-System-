import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { Subscription } from 'rxjs';
import { EmployeesService } from '../../../services/Employees/employees.service';
import { MatDialog } from '@angular/material/dialog';
import { IRole } from 'src/app/interfaces/IRole';
import { IRegion } from 'src/app/interfaces/IRegion';
import { IDepartment } from 'src/app/interfaces/IDepartment';
import { ISite } from 'src/app/interfaces/ISite';
import { SiteService } from 'src/app/services/Site/site.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { DepartmentService } from 'src/app/services/Department/department.service';
import { RoleService } from 'src/app/services/Role/role.service';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';
import { EditEmployeeComponent } from '../../edit-employee/edit-employee.component';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { IAction } from 'src/app/interfaces/IAction';
import { Action } from 'src/app/SystemActions/Action';
import { AuditLog } from 'src/app/SystemActions/AuditLog';
import { Employees } from 'src/app/models/employees';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
    selector: 'app-employee-table',
    templateUrl: './employee-table.component.html',
    styleUrls: ['./employee-table.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class EmployeeTableComponent implements OnInit, IAction {

  public employees: Employees[] = [];
  public suscription: Subscription = new Subscription;
  public pagina: number = 0;
  public search: string = '';
  public tipoBusqueda: string = '';

  public roleList: IRole[] = [];
  public areasList: IRegion[] = [];
  public departmentList: IDepartment[] = [];
  public parqueList: ISite[] = [];

  public columnas: string[] = ['identificacion', 'nombre', 'correo', 'phone', 'department', 'rol', 'asp', 'editar', 'estado'];

  constructor(public employeeService: EmployeesService,
    private toastr: ToastService, private dialog: MatDialog,
    private siteService: SiteService,
    private areaService: RegionService,
    private departmentService: DepartmentService,
    private roleService: RoleService,
    private areasService: RegionService,
    private taskService: AuditLogService,
    private confirmDialog: ConfirmDialogService,
    private employeesService: EmployeesService,
    public permissions: PermissionsService) { }

  get soloLectura(): boolean {
    return !this.permissions.esCompleto('employeesConsulta');
  }

  get employeesVisibles(): Employees[] {
    const rol = this.permissions.rolActual();
    if (rol === 'Site Manager') {
      return this.employees.filter(u => this.getRoleName(u.fK_idRole1) === 'Employee');
    }
    if (rol === 'Supervisor') {
      return this.employees.filter(u => this.getRoleName(u.fK_idRole1) !== 'Admin');
    }
    return this.employees;
  }

  generarDescripcion(nationalId: string, descripcion: string): string {
    throw new Error('Method not implemented.');
  }

  ngOnInit(): void {
    this.getRoles();
    this.getAreas();
    this.getDepartments();
    this.getParques();

    this.employeeService.obtenerEmployees().subscribe(employeess => {
      this.employees = employeess;
    });

    this.suscription = this.employeeService.refresh$.subscribe(() => {
      this.employeeService.obtenerEmployees().subscribe(employees => {
        this.employees = employees;
      })
    })
  }

  public getRoles() {
    this.roleService.obtenerRol().subscribe(roleL => {
      this.roleList = roleL
    });
  }

  public getAreas() {
    this.areasService.obtenerRegion().subscribe(areaList => { this.areasList = areaList });
  }

  public getDepartments() {
    this.departmentService.obtenerDepartment().subscribe(departamentList => {
      this.departmentList = departamentList
    });
  }

  public getParques() {
    this.siteService.obtenerTodosSite().subscribe(parqueList => {
      this.parqueList = parqueList
    });
  }

  editar(employee: Employees) {
    this.dialog.open(EditEmployeeComponent, {
      width: '720px',
      maxWidth: '95vw',
      data: employee
    }).afterClosed().subscribe(actualizado => {
      if (actualizado) {
        this.employeeService.obtenerEmployees().subscribe(employees => {
          this.employees = employees;
        });
      }
    });
  }

  paginaSiguiente() {
    this.pagina += 5;
  }
  paginaAnterior() {
    if (this.pagina > 0) {
      this.pagina -= 5;
    }
  }

  onSearchEmployee(search: string) {
    this.pagina = 0;
    this.search = search;
  }

  public getRoleName(id: number): string {
    for (let elemento of this.roleList) {
      if (elemento.pK_idRole == id) {
        return elemento.name;
      }
    }
    return '';
  }

  public getDepartmentName(id: number) {
    for (let elemento of this.departmentList) {
      if (elemento.pk_IdDepartment == id) {
        return elemento.name;
      }
    }
    return 'N/A';
  }

  deshabilitarEmployee(nationalId: string) {
    this.confirmDialog.confirm({
      title: 'Change status',
      message: 'Are you sure you want to change this employee’s status?'
    }).subscribe(confirmed => {
      if (confirmed) {
        this.employeeService.deshabilitarEmployee(nationalId).subscribe(
          (res: any) => {
            if (res.codigo == "200") {
              if (res.object.status == 'Activo') {
                this.toastr.success('Employee enabled successfully', 'Enabled!');
                let profileU = this.employeesService.cargarProfileEmployee();
                let descripcion: string = this.administrarEmployees(profileU.NationalId, Action.ActivarEmployee, nationalId);
                let task = new AuditLog(parseInt(profileU.UserID), descripcion);
                this.registrarTask(task)
              } else {
                this.toastr.warning('Employee disabled successfully', 'Disabled!');
                let profileU = this.employeesService.cargarProfileEmployee();
                let descripcion: string = this.administrarEmployees(profileU.NationalId, Action.DesactivarEmployee, nationalId);
                let task = new AuditLog(parseInt(profileU.UserID), descripcion);
                this.registrarTask(task)
              }
            }
            this.employeeService.obtenerEmployees();
          }, err => {
            if (err.status == 400) {
              this.toastr.error('Could not change the employee status', 'An active site manager already exists!');
            }
          })
      }
    })
  }

  administrarEmployees(idAdmin: string, descripciion: string, nationalIdRegistro: string): string {
    return `Employee ${idAdmin}${descripciion}${nationalIdRegistro}`
  }

  //========================= filter de busqueda ================================
  opcionBusqueda: string = 'Identificacion';
  capturarTipoBusqueda() {
    this.opcionBusqueda = this.opcionBusqueda;
  }
  opcionBusquedaEstado: string = 'Todos';
  capturarTipoBusquedaEstado() {
    this.opcionBusquedaEstado = this.opcionBusquedaEstado;
  }
  opcionBusquedaRol: string = 'Todos';
  capturarTipoBusquedaRol() {
    this.opcionBusquedaRol = this.opcionBusquedaRol;
  }
  opcionBusquedaArea: string = 'Todos';
  capturarTipoBusquedaArea() {
    this.opcionBusquedaArea = this.opcionBusquedaArea;
  }
  opcionBusquedaParque: string = 'Todos';
  capturarTipoBusquedaParque() {
    this.opcionBusquedaParque = this.opcionBusquedaParque;
  }
  opcionBusquedaDepartment: string = 'Todos';
  capturarTipoBusquedaDepartment() {
    this.opcionBusquedaDepartment = this.opcionBusquedaDepartment;
  }

  registrarTask(taskU: AuditLog) {
    this.taskService.registrarAuditLog(taskU).subscribe(
      (res: any) => {
        if (res.codigo == '201') {
          console.log('Task registrada');
        }
      },
      err => {
        console.log(err.mensaje);
        this.toastr.error(err.codigo, 'Internal server error...');
      }
    );
  }

  public getSiteName(id: number) {
    for (let elemento of this.parqueList) {
      if (elemento.pK_IdSite == id) {
        return elemento.name;
      }
    }
    return '';
  }
}
