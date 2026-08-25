import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ResourceGoalService } from 'src/app/services/ResourceGoal/resource-goal.service';
import { GoalService } from 'src/app/services/Goal/goal.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';
import { IResource } from 'src/app/interfaces/IResource';
import { IGoal } from 'src/app/interfaces/IGoal';
import { IResourceGoal } from 'src/app/interfaces/IResourceGoal';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-linked-goals-dialog',
  templateUrl: './linked-goals-dialog.component.html',
  styleUrls: ['./linked-goals-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class LinkedGoalsDialogComponent implements OnInit {
  vinculos: IResourceGoal[] = [];
  goalsList: IGoal[] = [];
  columnas: string[] = ['codigo', 'nombre', 'quitar'];
  formulario: UntypedFormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public resource: IResource,
    private dialogRef: MatDialogRef<LinkedGoalsDialogComponent>,
    private fb: UntypedFormBuilder,
    private resourceGoalService: ResourceGoalService,
    private goalService: GoalService,
    private toastr: ToastService,
    public permissions: PermissionsService
  ) {
    this.formulario = this.fb.group({
      fK_idGoal2: ['', Validators.required]
    });
    if (!this.permissions.esCompleto('resources')) {
      this.columnas = this.columnas.filter(c => c !== 'quitar');
    }
  }

  ngOnInit(): void {
    this.goalService.obtenerGoals().subscribe(o => this.goalsList = o);
    this.cargarVinculos();
  }

  cargarVinculos(): void {
    this.resourceGoalService.obtenerTodos().subscribe((v: IResourceGoal[]) => {
      this.vinculos = v.filter((x: IResourceGoal) => x.fK_idResource2 === this.resource.pK_idResource);
    });
  }

  name(id: number): string {
    return this.goalsList.find(o => o.pK_idGoal === id)?.name || 'N/A';
  }

  code(id: number): string {
    return this.goalsList.find(o => o.pK_idGoal === id)?.code || '';
  }

  get goalsDisponibles(): IGoal[] {
    const vinculadosIds = new Set(this.vinculos.map(v => v.fK_idGoal2));
    return this.goalsList.filter(o => !vinculadosIds.has(o.pK_idGoal));
  }

  vincular(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }
    const fK_idGoal2 = this.formulario.value.fK_idGoal2;
    this.resourceGoalService.vincular(this.resource.pK_idResource, fK_idGoal2).subscribe({
      next: () => {
        this.toastr.success('Goal linked', 'Success');
        this.formulario.reset();
        this.cargarVinculos();
      },
      error: (err: HttpErrorResponse) => this.toastr.error(err?.error?.mensaje || 'Internal server error', 'Could not link')
    });
  }

  quitar(vinculo: IResourceGoal): void {
    this.resourceGoalService.desvincular(vinculo.pK_idGoalResource).subscribe({
      next: () => {
        this.toastr.success('Goal unlinked', 'Success');
        this.cargarVinculos();
      },
      error: () => this.toastr.error('Could not unlink', 'Error')
    });
  }

  cerrar(): void {
    this.dialogRef.close();
  }
}
