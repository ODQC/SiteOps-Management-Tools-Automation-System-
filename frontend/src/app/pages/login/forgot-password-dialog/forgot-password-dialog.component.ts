import { Component, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { ToastService } from 'src/app/services/Toast/toast.service';

@Component({
  selector: 'app-forgot-password-dialog',
  templateUrl: './forgot-password-dialog.component.html',
  styleUrls: ['./forgot-password-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class ForgotPasswordDialogComponent {
  private isValidEmail = /^[a-z]+[a-z0-9._]+@+sinac+\.+go+\.+cr$/;

  recuperarPasswordForm: UntypedFormGroup;

  constructor(
    private fb: UntypedFormBuilder,
    private employeesService: EmployeesService,
    private toastr: ToastService,
    private dialogRef: MatDialogRef<ForgotPasswordDialogComponent>
  ) {
    this.recuperarPasswordForm = this.fb.group({
      recuperarPassword: ['', [Validators.required, Validators.pattern(this.isValidEmail)]],
    });
  }

  getErrorMensaje(field: string): string {
    let mensaje = '';
    if (this.recuperarPasswordForm.get(field)?.errors?.required) {
      mensaje = '¡El campo se encuentra vacío!';
    } else if (this.recuperarPasswordForm.get(field)?.hasError('pattern')) {
      mensaje = 'Sólo se permiten correos intitucionales.';
    }
    return mensaje;
  }

  esCampoValidoRecuperar(field: string): boolean {
    return !!(this.recuperarPasswordForm.get(field)?.touched && !this.recuperarPasswordForm.get(field)?.valid);
  }

  onReset(): void {
    const correoElectronico = this.recuperarPasswordForm.get('recuperarPassword')?.value;
    if (correoElectronico !== "") {
      this.employeesService.recuperarPassword(correoElectronico).subscribe();
      this.toastr.success('Recuperacion de password', 'Correo enviado.');
    }
    this.dialogRef.close();
  }

  cerrar(): void {
    this.dialogRef.close();
  }
}
