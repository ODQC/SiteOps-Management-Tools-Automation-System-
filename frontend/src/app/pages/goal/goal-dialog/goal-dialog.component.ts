import { Component, Inject, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GoalService } from 'src/app/services/Goal/goal.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IGoal } from 'src/app/interfaces/IGoal';

@Component({
  selector: 'app-goal-dialog',
  templateUrl: './goal-dialog.component.html',
  styleUrls: ['./goal-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class GoalDialogComponent {
  formulario: UntypedFormGroup;
  esEdicion: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IGoal | null,
    private dialogRef: MatDialogRef<GoalDialogComponent>,
    private fb: UntypedFormBuilder,
    private goalService: GoalService,
    private toastr: ToastService
  ) {
    this.esEdicion = !!data;
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      description: ['', Validators.required],
      year: ['', Validators.required],
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
      ? this.goalService.actualizarGoal(this.data!.pK_idGoal, payload)
      : this.goalService.guardarGoal(payload);

    request$.subscribe({
      next: () => {
        this.toastr.success(this.esEdicion ? 'Goal updated' : 'Goal created', 'Success');
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Internal server error', 'Could not save');
      }
    });
  }
}
