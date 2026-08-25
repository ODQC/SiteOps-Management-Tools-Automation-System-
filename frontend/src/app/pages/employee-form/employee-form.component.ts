import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IRole } from 'src/app/interfaces/IRole';
import { EmployeesService } from '../../services/Employees/employees.service';
import { RoleService } from '../../services/Role/role.service';
import { RegionService } from '../../services/Region/region.service';
import { IRegion } from '../../interfaces/IRegion';
import { DepartmentService } from '../../services/Department/department.service';
import { SiteService } from '../../services/Site/site.service';
import { IDepartment } from '../../interfaces/IDepartment';
import { ISite } from 'src/app/interfaces/ISite';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';
import { IAction } from 'src/app/interfaces/IAction';
import { AuditLog } from 'src/app/SystemActions/AuditLog';
import { Action } from 'src/app/SystemActions/Action';
import { Employees } from 'src/app/models/employees';


@Component({
    selector: 'app-employee-form',
    templateUrl: './employee-form.component.html',
    styleUrls: ['./employee-form.component.css'],
    providers: [EmployeesService],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})

export class EmployeeFormComponent implements OnInit, IAction {

  formulario: UntypedFormGroup;

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

  constructor(private formularioBuilder: UntypedFormBuilder,
    private employeeService: EmployeesService,
    private toastr: ToastService,
    private roleService: RoleService,
    private areasService: RegionService,
    private departmentService: DepartmentService,
    private siteService: SiteService,
    private taskService : AuditLogService) {

    this.formulario = this.formularioBuilder.group({
      nationalId: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20), Validators.pattern(/^[a-zA-Z0-9-]+$/)]],
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
  }

  ngOnInit(): void {
    this.getRoles();
    this.getAreas();
    this.getDepartments();
    this.getParqueNA();
    this.getAreaNA();
    this.getDepartmentNA();
  }

//método para generar descripciones de tasks de employee
generarDescripcion(nationalId :string, descripcion:string):string{
  return "El employee"+ nationalId + descripcion
}

public guardarEmployee() {
    const employee: Employees = {
      nationalId: this.formulario.get('nationalId')?.value,
      firstName: this.formulario.get('nombre')?.value,
      lastName: this.formulario.get('apellidoUno')?.value,
      secondLastName: this.formulario.get('apellidoDos')?.value,
      phone: this.formulario.get('phone')?.value,
      email: this.formulario.get('correo')?.value,
      password: this.formulario.get('passwordDos')?.value,
      status: 'Activo',
      fK_idRole1: this.retornarPkRol(this.formulario.get('rol')?.value),
      fK_idDepartment1: this.retornarPkDepartment(this.formulario.get('department')?.value),
      fK_idSite1: this.retornarPkParque(this.formulario.get('site')?.value)
    };

    this.employeeService.guardarEmployee(employee).subscribe(
      (res: any) => {
        if (res.codigo == '201') {
          this.formulario.reset();
          this.toastr.success('Employee creado', '¡Registro exitoso!');
          let profileU = this.employeeService.cargarProfileEmployee();
          let descripcion:string = this.administrarEmployees(profileU.NationalId,Action.AgregarEmployee,employee.nationalId);
          let task = new AuditLog(parseInt(profileU.UserID),descripcion);
          this.registrarTask(task)
        }
        else if (res.codigo == '200') {
          if (res.mensaje == "El employee ya está registrado en el sistema") {
            this.toastr.error(res.mensaje, 'Error en el ingreso del employee!');
          }
          else {
            this.toastr.error(res.object, res.mensaje);
          }
        }
      },
      err => {
        console.log(err.mensaje);
        this.toastr.error(err.codigo, 'Error interno en el servidor...');
      }
    );
  }
  administrarEmployees(idAdmin:string, descripciion:string, nationalIdRegistro:string ):string{
    return `El employee ${idAdmin}${descripciion}${nationalIdRegistro}`
    }
  registrarTask(taskU : AuditLog){
    this.taskService.registrarAuditLog(taskU).subscribe(
      (res: any) => {
        if(res.codigo == '201'){

          console.log('Task registrada');
        }

      },
      err =>{
        console.log(err.mensaje);
          this.toastr.error(err.codigo, 'Error interno en el servidor...');
      }
    );

  }

  public get nationalIdNoValido() {
    return this.formulario.get('nationalId')?.invalid && this.formulario.get('nationalId')?.touched;
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
    this.departmentList2 = []
    this.parqueList2 = []
    if(nombre == 'Site Manager'){
      this.areasList2 = this.areasList;
      this.parqueList2 = this.parqueList;
    }
    else if(nombre == 'Employee'){
      this.areasList2 = this.areasList;
      this.parqueList2 = this.parqueList;
    }
    else if(nombre == 'Supervisor'){
      this.departmentList2 = this.departmentList;
    }
  }

  public getRoles(){
    this.roleService.obtenerRol().subscribe(roleL => {
      this.roleList = roleL
    });
  }

  public getAreaNA(){
    this.areasService.obtenerAreaNA().subscribe(areaNA => {
      this.areaNA = areaNA
    });
  }

  public getDepartmentNA(){
    this.departmentService.obtenerDepartmentNA().subscribe(departmentNA => {
      this.departmentNA = departmentNA
    });
  }

  public getParqueNA(){
    this.siteService.obtenerParqueNA().subscribe(parqueNA => {
      this.idParqueNA = parqueNA
    });
  }

  public getAreas(){
    this.areasService.obtenerRegion().subscribe(areaList => { this.areasList = areaList });
    this.getParques();
  }

  public getDepartments(){
    this.departmentService.obtenerDepartment().subscribe(departamentList => {
      this.departmentList = departamentList
    });
  }

  public getParques(){
    this.siteService.obtenerSite(this.retornarPkArea(this.formulario.get('areaSilvestre')?.value)).subscribe(parqueList => {
      this.parqueList = parqueList
    });
  }

  public retornarPkDepartment(nombre: string): number{

    if(this.formulario.get('rol')?.value == 'Site Manager' || this.formulario.get('rol')?.value == 'Employee'){
      console.log(this.departmentNA);
      return this.departmentNA;
    }

    if(this.formulario.get('rol')?.value == 'Admin'){
      for(let elemento of this.departmentList){
        if(elemento.name == "Department TI"){
          return elemento.pk_IdDepartment;
        }
      }
    }

    for(let elemento of this.departmentList){
      if(elemento.name == nombre){
        console.log(elemento.pk_IdDepartment);
        return elemento.pk_IdDepartment;
      }
    }
    return 0;
  }

  public retornarPkParque(nombre: string): number{

    if(this.formulario.get('rol')?.value == 'Supervisor' || this.formulario.get('rol')?.value == 'Admin'){
      return this.idParqueNA;
    }

    for(let elemento of this.parqueList){
      if(elemento.name == nombre){
        return elemento.pK_IdSite;
      }
    }
    return 0;
  }

  public retornarPkRol(nombre: string): number{
    for(let elemento of this.roleList){
      if(elemento.name == nombre){
        return elemento.pK_idRole;
      }
    }
    return 0;
  }

  public retornarPkArea(nombre: string): number{
    for(let elemento of this.areasList){
      if(elemento.name == nombre){
        return elemento.pK_IdRegion;
      }
    }
    return 0;
  }

  getErrorMessage(field: string): string{
    let mensaje = '';
    if(this.formulario.get(field)?.errors?.required){
      mensaje = 'El campo no puede estar vacío!';
    }else if(this.formulario.get(field)?.hasError('pattern')){
      mensaje = 'No es una password válida, debe tener al menos una letra mayúscula, un número, una letra minúscula y al menos un caracter especial ($@!%*?&)';
    }else if(this.formulario.get(field)?.hasError('minlength') ||  this.formulario.get(field)?.hasError('maxlength')){
      mensaje = `Debe tener de 8 a 16 caracteres`;
    }
    return mensaje;
  }

  getErrorMessageId(field: string): string{
    let mensaje = '';
    if(this.formulario.get(field)?.errors?.required){
      mensaje = 'El campo no puede estar vacío!';
    }else if(this.formulario.get(field)?.hasError('pattern')){
      mensaje = 'Solo se permiten letras, números y guiones';
    }else if(this.formulario.get(field)?.hasError('minlength') || this.formulario.get(field)?.hasError('maxlength')){
      mensaje = 'Debe tener de 4 a 20 caracteres';
    }
    return mensaje;
  }

  esCampoValido(field: string): boolean{
      if((this.formulario.get(field)?.touched  && !this.formulario.get(field)?.valid)){
        return true;
      }
      else{
        return false;
      }
  }

  // ======================== password
  showPassword(input: any): any {
    input.type = input.type === 'password' ? 'text' : 'password';
  }

}
