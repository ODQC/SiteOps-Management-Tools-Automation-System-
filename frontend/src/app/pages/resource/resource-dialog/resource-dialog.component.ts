import { Component, Inject, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ResourceService } from 'src/app/services/Resource/resource.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IResource } from 'src/app/interfaces/IResource';

@Component({
  selector: 'app-resource-dialog',
  templateUrl: './resource-dialog.component.html',
  styleUrls: ['./resource-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class ResourceDialogComponent {
  formulario: UntypedFormGroup;
  esEdicion: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IResource | null,
    private dialogRef: MatDialogRef<ResourceDialogComponent>,
    private fb: UntypedFormBuilder,
    private resourceService: ResourceService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      type: ['', Validators.required],
      description: ['', Validators.required],
      status: ['Active', Validators.required]
    });

    if (data) {
      this.formulario.patchValue(data);
    }
  }

  esCampoValido(field: string): boolean {
    const control = this.formulario.get(field);
    return !!(control?.touched && control?.invalid);
  }

  cancelar(): void {
    this.dialogRef.close(false);
  }

  guardar(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const payload = { ...(this.data || {}), ...this.formulario.value };

    const request$ = this.esEdicion
      ? this.resourceService.actualizarResource(this.data!.pK_idResource, payload)
      : this.resourceService.guardarResource(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Resource updated' : 'Resource created', 'Success');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Internal server error', 'Could not save');
      }
    });
  }
}
