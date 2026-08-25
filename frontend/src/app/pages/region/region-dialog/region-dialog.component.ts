import { Component, Inject, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RegionService } from 'src/app/services/Region/region.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IRegion } from 'src/app/interfaces/IRegion';

@Component({
  selector: 'app-region-dialog',
  templateUrl: './region-dialog.component.html',
  styleUrls: ['./region-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class RegionDialogComponent {
  formulario: UntypedFormGroup;
  esEdicion: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IRegion | null,
    private dialogRef: MatDialogRef<RegionDialogComponent>,
    private fb: UntypedFormBuilder,
    private areaService: RegionService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
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
      ? this.areaService.actualizarArea(this.data!.pK_IdRegion, payload)
      : this.areaService.guardarArea(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Region updated' : 'Region created', 'Success');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Internal server error', 'Could not save');
      }
    });
  }
}
