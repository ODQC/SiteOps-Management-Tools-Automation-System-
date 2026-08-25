import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GoalService } from 'src/app/services/Goal/goal.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { IGoal } from 'src/app/interfaces/IGoal';
import { GoalDialogComponent } from './goal-dialog/goal-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-goal',
  templateUrl: './goal.component.html',
  styleUrls: ['./goal.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class GoalComponent implements OnInit {
  goals: IGoal[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'nombre', 'anno', 'editar', 'estado'];

  constructor(
    private goalService: GoalService,
    private toastr: ToastService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    public permissions: PermissionsService
  ) {
    this.soloLectura = !this.permissions.esCompleto('goals');
    if (this.soloLectura) {
      this.columnas = this.columnas.filter(c => c !== 'editar');
    }
  }

  ngOnInit(): void {
    this.cargar();
    this.goalService.refresh$.subscribe(() => this.cargar());
  }

  cargar(): void {
    this.goalService.obtenerGoals().subscribe(o => this.goals = o);
  }

  get goalsFiltrados(): IGoal[] {
    const texto = this.search.trim().toLowerCase();
    if (!texto) return this.goals;
    return this.goals.filter(o =>
      o.name?.toLowerCase().includes(texto) || o.code?.toLowerCase().includes(texto)
    );
  }

  nuevo(): void {
    this.dialog.open(GoalDialogComponent, { width: '600px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  editar(goal: IGoal): void {
    this.dialog.open(GoalDialogComponent, { width: '600px', maxWidth: '95vw', data: goal })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  cambiarEstado(goal: IGoal): void {
    this.confirmDialog.confirm({
      title: 'Change status',
      message: `Change the status of "${goal.name}"?`
    }).subscribe(confirmed => {
      if (confirmed && goal.code) {
        this.goalService.cambiarEstado(goal.code).subscribe({
          next: () => this.toastr.success('Status updated', 'Success'),
          error: () => this.toastr.error('Could not change the status', 'Error')
        });
      }
    });
  }
}
