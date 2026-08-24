import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectService } from 'src/app/services/Project/project.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IProject } from 'src/app/interfaces/IProject';
import { IRegion } from 'src/app/interfaces/IRegion';
import { Employees } from 'src/app/models/employees';

@Component({
  selector: 'app-project-dialog',
  templateUrl: './project-dialog.component.html',
  styleUrls: ['./project-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class ProjectDialogComponent implements OnInit {
  formulario: UntypedFormGroup;
  esEdicion: boolean;
  areasList: IRegion[] = [];
  employeesList: Employees[] = [];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IProject | null,
    private dialogRef: MatDialogRef<ProjectDialogComponent>,
    private fb: UntypedFormBuilder,
    private planService: ProjectService,
    private areaService: RegionService,
    private employeesService: EmployeesService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      fk_IdEmployee1: ['', Validators.required],
      fK_idRegion2: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      status: ['Pendiente', Validators.required],
      progress: ['0', Validators.required]
    });

    if (data) {
      this.formulario.patchValue({
        ...data,
        startDate: data.startDate ? new Date(data.startDate) : '',
        endDate: data.endDate ? new Date(data.endDate) : ''
      });
    }
  }

  ngOnInit(): void {
    this.areaService.obtenerRegion().subscribe(areas => this.areasList = areas);
    this.employeesService.obtenerEmployees().subscribe(employees => this.employeesList = employees);
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

    const value = this.formulario.value;
    const payload = {
      ...(this.data || {}),
      ...value,
      startDate: new Date(value.startDate).toISOString(),
      endDate: new Date(value.endDate).toISOString(),
      progress: String(value.progress)
    };

    const request$ = this.esEdicion
      ? this.planService.actualizarProject(this.data!.pK_idProject, payload)
      : this.planService.guardarProject(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Plan actualizado' : 'Plan creado', 'Éxito');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Error interno del servidor', 'No se pudo guardar');
      }
    });
  }
}
