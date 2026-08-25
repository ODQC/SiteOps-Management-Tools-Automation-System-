import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeliverableService } from 'src/app/services/Deliverable/deliverable.service';
import { TaskService } from 'src/app/services/Task/task.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { IDeliverable } from 'src/app/interfaces/IDeliverable';
import { ITask } from 'src/app/interfaces/ITask';
import { DeliverableDialogComponent } from './deliverable-dialog/deliverable-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-deliverable',
  templateUrl: './deliverable.component.html',
  styleUrls: ['./deliverable.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class DeliverableComponent implements OnInit {
  deliverables: IDeliverable[] = [];
  taskesList: ITask[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'nombre', 'task', 'document', 'editar', 'estado'];

  constructor(
    private deliverableService: DeliverableService,
    private taskService: TaskService,
    private toastr: ToastService,
    private dialog: MatDialog,
    public permissions: PermissionsService
  ) {
    this.soloLectura = !this.permissions.esCompleto('deliverables');
    if (this.soloLectura) {
      this.columnas = this.columnas.filter(c => c !== 'editar');
    }
  }

  ngOnInit(): void {
    this.cargar();
    this.taskService.obtenerTaskes().subscribe(a => this.taskesList = a);
  }

  cargar(): void {
    this.deliverableService.obtenerDeliverables().subscribe(e => this.deliverables = e);
  }

  name(id?: number): string {
    return this.taskesList.find(a => a.pK_idTaskItem === id)?.name || 'N/A';
  }

  get deliverablesFiltradas(): IDeliverable[] {
    const texto = this.search.trim().toLowerCase();
    if (!texto) return this.deliverables;
    return this.deliverables.filter(e =>
      e.name?.toLowerCase().includes(texto) || e.code?.toLowerCase().includes(texto)
    );
  }

  nueva(): void {
    this.dialog.open(DeliverableDialogComponent, { width: '640px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  editar(deliverable: IDeliverable): void {
    this.dialog.open(DeliverableDialogComponent, { width: '640px', maxWidth: '95vw', data: deliverable })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  cambiarEstado(deliverable: IDeliverable): void {
    this.deliverableService.cambiarEstado(deliverable.pK_idDeliverable).subscribe({
      next: () => { this.toastr.success('Status updated', 'Success'); this.cargar(); },
      error: () => this.toastr.error('Could not change the status', 'Error')
    });
  }
}
