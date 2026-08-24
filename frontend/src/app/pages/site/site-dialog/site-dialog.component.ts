import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SiteService } from 'src/app/services/Site/site.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ISite } from 'src/app/interfaces/ISite';
import { IRegion } from 'src/app/interfaces/IRegion';

@Component({
  selector: 'app-site-dialog',
  templateUrl: './site-dialog.component.html',
  styleUrls: ['./site-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class SiteDialogComponent implements OnInit {
  formulario: UntypedFormGroup;
  esEdicion: boolean;
  areasList: IRegion[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ISite | null,
    private dialogRef: MatDialogRef<SiteDialogComponent>,
    private fb: UntypedFormBuilder,
    private parqueService: SiteService,
    private areaService: RegionService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required],
      status: ['Activo', Validators.required],
      fK_idRegion1: [null, Validators.required]
    });

    if (data) {
      this.formulario.patchValue(data);
    }
  }

  ngOnInit(): void {
    this.areaService.obtenerRegion().subscribe(areas => this.areasList = areas);
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
      ? this.parqueService.actualizarParque(this.data!.pK_IdSite, payload)
      : this.parqueService.guardarParque(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Parque actualizado' : 'Parque creado', 'Éxito');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Error interno del servidor', 'No se pudo guardar');
      }
    });
  }
}
