import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ResourceService } from 'src/app/services/Resource/resource.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { IResource } from 'src/app/interfaces/IResource';
import { ResourceDialogComponent } from './resource-dialog/resource-dialog.component';
import { LinkedGoalsDialogComponent } from './linked-goals-dialog/linked-goals-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-resource',
  templateUrl: './resource.component.html',
  styleUrls: ['./resource.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class ResourceComponent implements OnInit {
  resources: IResource[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'tipo', 'descripcion', 'goals', 'editar', 'estado'];

  constructor(
    private resourceService: ResourceService,
    private toastr: ToastService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    public permissions: PermissionsService
  ) {
    this.soloLectura = !this.permissions.esCompleto('resources');
    if (this.soloLectura) {
      this.columnas = this.columnas.filter(c => c !== 'editar');
    }
  }

  ngOnInit(): void {
    this.cargar();
    this.resourceService.refresh$.subscribe(() => this.cargar());
  }

  cargar(): void {
    this.resourceService.obtenerResources().subscribe(h => this.resources = h);
  }

  get resourcesFiltradas(): IResource[] {
    const texto = this.search.trim().toLowerCase();
    if (!texto) return this.resources;
    return this.resources.filter(h =>
      h.type?.toLowerCase().includes(texto) || h.code?.toLowerCase().includes(texto)
    );
  }

  nueva(): void {
    this.dialog.open(ResourceDialogComponent, { width: '600px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  editar(resource: IResource): void {
    this.dialog.open(ResourceDialogComponent, { width: '600px', maxWidth: '95vw', data: resource })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  verGoals(resource: IResource): void {
    this.dialog.open(LinkedGoalsDialogComponent, { width: '640px', maxWidth: '95vw', data: resource });
  }

  cambiarEstado(resource: IResource): void {
    this.confirmDialog.confirm({
      title: 'Cambiar estado',
      message: `¿Cambiar el estado de "${resource.type}"?`
    }).subscribe(confirmed => {
      if (confirmed && resource.code) {
        this.resourceService.cambiarEstado(resource.code).subscribe({
          next: () => this.toastr.success('Estado actualizado', 'Éxito'),
          error: () => this.toastr.error('No se pudo cambiar el estado', 'Error')
        });
      }
    });
  }
}
