import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TaskService } from 'src/app/services/Task/task.service';
import { GoalService } from 'src/app/services/Goal/goal.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ITask } from 'src/app/interfaces/ITask';
import { IGoal } from 'src/app/interfaces/IGoal';
import { IProject } from 'src/app/interfaces/IProject';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-tasks-dialog',
  templateUrl: './tasks-dialog.component.html',
  styleUrls: ['./tasks-dialog.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class TasksDialogComponent implements OnInit {
  tasks: ITask[] = [];
  goalsList: IGoal[] = [];
  formulario: UntypedFormGroup;
  mostrarFormulario = false;
  columnas: string[] = ['codigo', 'nombre', 'fecha', 'estado'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public plan: IProject,
    private dialogRef: MatDialogRef<TasksDialogComponent>,
    private fb: UntypedFormBuilder,
    private taskService: TaskService,
    private goalService: GoalService,
    private toastr: ToastService,
    public permissions: PermissionsService
  ) {
    this.formulario = this.fb.group({
      code: ['', Validators.required],
      name: ['', Validators.required],
      fk_IdGoal1: ['', Validators.required],
      completionDate: ['', Validators.required],
      collaborators: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.cargarTaskes();
    this.goalService.obtenerGoals().subscribe(o => this.goalsList = o);
  }

  cargarTaskes(): void {
    this.taskService.obtenerTaskesPorPlan(this.plan.pK_idProject).subscribe((res: any) => {
      this.tasks = res?.object || res || [];
    });
  }

  toggleFormulario(): void {
    this.mostrarFormulario = !this.mostrarFormulario;
  }

  crearTask(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    const value = this.formulario.value;
    const payload: ITask = {
      pK_idTask: 0,
      ...value,
      completionDate: new Date(value.completionDate).toISOString(),
      estado: 'Pendiente',
      taskStatus: 'Pendiente',
      fK_idProject: this.plan.pK_idProject
    };

    this.taskService.guardarTask(payload).subscribe({
      next: () => {
        this.toastr.success('Task creada', 'Éxito');
        this.formulario.reset();
        this.mostrarFormulario = false;
        this.cargarTaskes();
      },
      error: (err) => {
        this.toastr.error(err?.error?.mensaje || 'Error interno del servidor', 'No se pudo crear la task');
      }
    });
  }

  esCampoValido(field: string): boolean {
    const control = this.formulario.get(field);
    return !!(control?.touched && control?.invalid);
  }

  cerrar(): void {
    this.dialogRef.close();
  }
}
