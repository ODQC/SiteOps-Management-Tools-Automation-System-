import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { tipo, tipoIdentificacion } from 'src/app/interfaces/identificationType';
import { DataService } from 'src/app/services/Data/data.service';
import { EmployeesService } from '../../services/Employees/employees.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { IRegion } from 'src/app/interfaces/IRegion';
import { DepartmentService } from 'src/app/services/Department/department.service';
import { IDepartment } from 'src/app/interfaces/IDepartment';
import { SiteService } from 'src/app/services/Site/site.service';
import { ISite } from 'src/app/interfaces/ISite';
import { RoleService } from 'src/app/services/Role/role.service';
import { IRole } from 'src/app/interfaces/IRole';
import { Employees } from 'src/app/models/employees';
import { IAction } from 'src/app/interfaces/IAction';
import { Action } from 'src/app/SystemActions/Action';
import { AuditLog } from 'src/app/SystemActions/AuditLog';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';

@Component({
    selector: 'app-edit-employee',
    templateUrl: './edit-employee.component.html',
    styleUrls: ['./edit-employee.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})

export class EditEmployeeComponent implements OnInit, IAction {

  formulario: UntypedFormGroup;

  //Roles de employees
  public roleList: IRole[] = [];
  public areasList: IRegion[] = [];
  public areasList2: IRegion[] = [];
  public departmentList: IDepartment[] = [];
  public departmentList2: IDepartment[] = [];
  public parqueList: ISite[] = [];
  public parqueList2: ISite[] = [];
  public idParqueNA!: number;
  public departmentNA!: number;
  public areaNA!: number;

  public defaultValue: string;

  public selectedRol: IRole = {
    pK_idRole: 0,
    name: ''
  };
  public selectedArea: IRegion = {
    pK_IdRegion: 0,
    name: ''
  };
  public selectedDepartment: IDepartment = {
    pk_IdDepartment: 0,
    name: ''
  };
  public selectedParque: ISite = {
    pK_IdSite: 0,
    name: ''
  };

  //Tipos de identificaciones
  public tipoIdentificacion: tipoIdentificacion[] = [];
  public tipoCedulas: tipo[] = [];
  public selectedId: tipoIdentificacion = {
    tipoId: 0,
    nombre: ''
  };

  private employee: Employees;

  constructor(
    @Inject(MAT_DIALOG_DATA) employee: Employees,
    private dialogRef: MatDialogRef<EditEmployeeComponent>,
    private formularioBuilder: UntypedFormBuilder,
    private dataService: DataService,
    private employeeService: EmployeesService,
    private toastr: ToastService,
    private siteService: SiteService,
    private areaService: RegionService,
    private departmentService: DepartmentService,
    private roleService: RoleService,
    private areasService: RegionService,
    private taskService: AuditLogService
  ) {
    this.employee = employee;

    this.formulario = this.formularioBuilder.group({
      nombre: ['', Validators.required],
      apellidoUno: ['', Validators.required],
      apellidoDos: ['', Validators.required],
      phone: ['', [Validators.required, Validators.maxLength(8), Validators.minLength(8), Validators.pattern(/^[0-9]\d*$/)]],
      correo: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]],
      passwordUno: ['', [Validators.required, Validators.maxLength(16), Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@!%*?&])[A-Za-z\d$@$!%*?&]/)]],
      passwordDos: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@!%*?&])[A-Za-z\d$@$!%*?&]{8,16}/)]],
      rol: ['', Validators.required],
      department: ['', Validators.required],
      areaSilvestre: ['', Validators.required],
      site: ['', Validators.required]
    });
    this.defaultValue = '';
  }

  ngOnInit(): void {
    this.getRoles();
    this.getAreas();
    this.getDepartments();
    this.getParqueNA();
    this.getAreaNA();
    this.getDepartmentNA();
    this.tipoIdentificacion = this.dataService.getTipoId();

    this.formulario.patchValue({
      nombre: this.employee.firstName,
      apellidoUno: this.employee.lastName,
      apellidoDos: this.employee.secondLastName,
      phone: this.employee.phone,
      correo: this.employee.email,
      passwordUno: this.employee.password,
      passwordDos: this.employee.password,
      department: this.employee.fK_idDepartment1,
      areaSilvestre: this.employee.fK_idSite1,
      rol: this.employee.fK_idRole1
    });
    this.defaultValue = this.retornarRol(this.employee.fK_idRole1.toString());
    this.onSelected(this.defaultValue);
  }

  public retornarCedula(): string {
    return this.employee?.nationalId;
  }

  public verificarInputID() {
    if (this.selectedId.tipoId == 1) {
      return this.formulario.get('cedulaNacional');
    }
    return this.formulario.get('cedulaDimex');
  }

  public get nombreNoValido() {
    return this.formulario.get('nombre')?.invalid && this.formulario.get('nombre')?.touched;
  }

  public get apellido1NoValido() {
    return this.formulario.get('apellidoUno')?.invalid && this.formulario.get('apellidoUno')?.touched;
  }

  public get apellido2NoValido() {
    return this.formulario.get('apellidoDos')?.invalid && this.formulario.get('apellidoDos')?.touched;
  }

  public get phoneNoValido() {
    return this.formulario.get('phone')?.invalid && this.formulario.get('phone')?.touched;
  }

  public get correoNoValido() {
    return this.formulario.get('correo')?.invalid && this.formulario.get('correo')?.touched;
  }

  public get password2NoValid() {
    return this.formulario.get('passwordDos')?.touched;
  }

  public get password2NoValido() {
    const pass1 = this.formulario.get('passwordUno')?.value;
    const pass2 = this.formulario.get('passwordDos')?.value;

    return (pass1 === pass2) ? false : true;
  }

  public onSelected(nombre: string) {
    this.areasList2 = [];
    this.departmentList2 = [];
    this.parqueList2 = [];

    if (this.retornarRol(nombre) == 'Site Manager') {
      this.areasList2 = this.areasList;
      this.parqueList2 = this.parqueList;
    }
    else if (this.retornarRol(nombre) == 'Employee') {
      this.areasList2 = this.areasList;
      this.parqueList2 = this.parqueList;
    }
    else if (this.retornarRol(nombre) == 'Supervisor') {
      this.departmentList2 = this.departmentList;
    }
  }

  public retornarRol(pkRol: string): string {
    for (let rol of this.roleList) {
      if (rol.pK_idRole == Number(pkRol)) {
        return rol.name;
      }
    }
    return "";
  }

  public onSelectedId(id: any) {
    this.tipoCedulas = this.dataService.getTipo().filter(item => item.tipoId == id.value);
  }

  showPassword(input: any): any {
    input.type = input.type === 'password' ? 'text' : 'password';
  }

  cancelar(): void {
    this.dialogRef.close(false);
  }

  editar() {
    const employee: Employees = {
      nationalId: this.retornarCedula(),
      firstName: this.formulario.get('nombre')?.value,
      lastName: this.formulario.get('apellidoUno')?.value,
      secondLastName: this.formulario.get('apellidoDos')?.value,
      phone: this.formulario.get('phone')?.value,
      email: this.formulario.get('correo')?.value,
      password: this.formulario.get('passwordDos')?.value,
      status: this.employee.status,
      fK_idRole1: Number(this.formulario.get('rol')?.value),
      fK_idDepartment1: this.retornarPkDepartment(this.formulario.get('department')?.value),
      fK_idSite1: this.retornarPkParque(this.formulario.get('site')?.value)
    }
    this.employeeService.actualizarEmployee(this.retornarCedula(), employee).subscribe(
      (data: any) => {
        if (data.codigo == 200) {
          if (data.mensaje == "El email ya está siendo utilizado") {
            this.toastr.error(data.mensaje, 'Error en actualizar el employee!');
          }
          if (data.mensaje == "Employee actualizado") {
            this.toastr.success("Registro Actualizado", "La employee fue actualizado");
            let profileU = this.employeeService.cargarProfileEmployee();
            let descripcion: string = this.administrarEmployees(profileU.NationalId, Action.ActualizarEmployee, employee.nationalId);
            let task = new AuditLog(parseInt(profileU.UserID), descripcion);
            this.registrarTask(task)

            this.formulario.reset();
            this.dialogRef.close(true);
          }
        }

      },
      err => {
        console.log(err.mensaje);
        if (err.status == 404) {
          this.toastr.error('Employee no encontrado', 'El employee no se encontro en el sistema');
        }
        this.toastr.error(err.codigo, 'Error interno en el servidor...');
      });
  }

  getErrorMessage(field: string): string {
    let mensaje = '';
    if (this.formulario.get(field)?.errors?.required) {
      mensaje = 'El campo no puede estar vacío!';
    } else if (this.formulario.get(field)?.hasError('pattern')) {
      mensaje = 'No es una password válida, debe tener al menos una letra mayúscula, un número, una letra minúscula y al menos un caracter especial ($@!%*?&)';
    } else if (this.formulario.get(field)?.hasError('minlength') || this.formulario.get(field)?.hasError('maxlength')) {
      mensaje = `Debe tener de 8 a 16 caracteres`;
    }
    return mensaje;
  }

  esCampoValido(field: string): boolean {
    return !!(this.formulario.get(field)?.touched && !this.formulario.get(field)?.valid);
  }

  public getRoles() {
    this.roleService.obtenerRol().subscribe(roleL => {
      this.roleList = roleL
    });
  }

  public getAreaNA() {
    this.areasService.obtenerAreaNA().subscribe(areaNA => {
      this.areaNA = areaNA
    });
  }

  public getDepartmentNA() {
    this.departmentService.obtenerDepartmentNA().subscribe(departmentNA => {
      this.departmentNA = departmentNA
    });
  }

  public getParqueNA() {
    this.siteService.obtenerParqueNA().subscribe(parqueNA => {
      this.idParqueNA = parqueNA
    });
  }

  public getAreas() {
    this.areasService.obtenerRegion().subscribe(areaList => {
      this.areasList = areaList;
      this.onSelected(this.defaultValue);
    });
    this.getParques();
  }

  public getDepartments() {
    this.departmentService.obtenerDepartment().subscribe(departamentList => {
      this.departmentList = departamentList;
      this.onSelected(this.defaultValue);
    });
  }

  public getParques() {
    this.siteService.obtenerSite(this.retornarPkArea(this.formulario.get('areaSilvestre')?.value)).subscribe(parqueList => {
      this.parqueList = parqueList
    });
  }

  public retornarPkDepartment(nombre: string): number {

    if (this.retornarRol(this.formulario.get('rol')?.value) == 'Site Manager' || this.retornarRol(this.formulario.get('rol')?.value) == 'Employee') {
      return this.departmentNA;
    }

    if (this.retornarRol(this.formulario.get('rol')?.value) == 'Admin') {
      for (let elemento of this.departmentList) {
        if (elemento.name == "Department TI") {
          return elemento.pk_IdDepartment;
        }
      }
    }

    for (let elemento of this.departmentList) {
      if (elemento.name == nombre) {
        return elemento.pk_IdDepartment;
      }
    }
    return 0;
  }

  public retornarPkParque(nombre: string): number {

    if (this.retornarRol(this.formulario.get('rol')?.value) == 'Supervisor' || this.retornarRol(this.formulario.get('rol')?.value) == 'Admin') {
      return this.idParqueNA;
    }

    if (nombre == "") {
      return this.employee.fK_idSite1;
    }

    for (let elemento of this.parqueList) {
      if (elemento.name == nombre) {
        return elemento.pK_IdSite;
      }
    }
    return 0;
  }

  public retornarPkRol(nombre: string): number {
    for (let elemento of this.roleList) {
      if (elemento.name == nombre) {
        return elemento.pK_idRole;
      }
    }
    return 0;
  }

  public retornarPkArea(nombre: string): number {
    for (let elemento of this.areasList) {
      if (elemento.name == nombre) {
        return elemento.pK_IdRegion;
      }
    }
    return 0;
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
        this.toastr.error(err.codigo, 'Error interno en el servidor...');
      }
    );
  }

  generarDescripcion(cedula: string, descripcion: string): string {
    return `El employee ${cedula}${descripcion}`;
  }

  administrarEmployees(idAdmin: string, descripcion: string, cedulaRegistro: string): string {
    return `El employee ${idAdmin}${descripcion}${cedulaRegistro}`;
  }
}
