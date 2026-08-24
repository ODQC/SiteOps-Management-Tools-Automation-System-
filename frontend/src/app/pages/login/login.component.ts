import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IAuthentication } from 'src/app/interfaces/IAuthentication';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';
import { Action } from 'src/app/SystemActions/Action';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IAction } from 'src/app/interfaces/IAction';
import { AuditLog } from 'src/app/SystemActions/AuditLog';
import { MatDialog } from '@angular/material/dialog';
import { ForgotPasswordDialogComponent } from './forgot-password-dialog/forgot-password-dialog.component';
import { ThemeService } from 'src/app/services/Theme/theme.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})

export class LoginComponent implements OnInit, IAction {
  private isValidEmail = /^[a-z]+[a-z0-9._]+@+sinac+\.+go+\.+cr$/;  // sirve para el sinac

  loginForm: UntypedFormGroup;
  hidePassword = true;

  constructor(
    private fb: UntypedFormBuilder,
    private employeesService: EmployeesService,
    private router: Router,
    private toastr: ToastService,
    private taskService: AuditLogService,
    private dialog: MatDialog,
    public themeService: ThemeService,
  ) {

    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.pattern(this.isValidEmail)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]]
    });
  }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null) {
      this.router.navigateByUrl('/sidebar');
    }
  }

  // ================    Validacions del inicio de sesion ===================
  getErrorMessage(field: string): string {
    let mensaje = '';
    if (this.loginForm.get(field)?.errors?.required) {
      mensaje = 'Campo vacío!';
    } else if (this.loginForm.get(field)?.hasError('pattern')) {
      mensaje = 'No es un email valido';
    } else if (this.loginForm.get(field)?.hasError('minlength')) {
      mensaje = `Debe tener almenos 8 caracteres`;
    } else if (this.loginForm.get(field)?.hasError('maxlength')) {
      mensaje = `No debe sobrepasar 16 caracteres`;
    }
    return mensaje;
  }

  esCampoValido(field: string): boolean {
    return !!(this.loginForm.get(field)?.touched && !this.loginForm.get(field)?.valid);
  }

  goToLink(url: string) {
    window.open(url, "_blank");
  }

  inicioSesion() {
    const autenticacionModel: IAuthentication = {
      correoElectronico: this.loginForm.get('username')?.value,
      password: this.loginForm.get('password')?.value
    }

    this.employeesService.login(autenticacionModel).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.mensaje);
        let profileU = this.employeesService.cargarProfileEmployee();
        let descripcion: string = this.generarDescripcion(profileU.NationalId, Action.InicioSesion);
        let task = new AuditLog(parseInt(profileU.UserID), descripcion);
        this.registrarTask(task)
        this.router.navigateByUrl('/sidebar');
      },
      err => {
        if (err.status == 400)
          this.toastr.error('Employee o password incorrectos', 'Autentificación fallida');
        else
          console.log(err);
      }
    );
  }

  abrirRecuperarPassword(): void {
    this.dialog.open(ForgotPasswordDialogComponent, { width: '420px' });
  }

  registrarTask(taskU: AuditLog): void {
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
