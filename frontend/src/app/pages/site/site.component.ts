import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SiteService } from 'src/app/services/Site/site.service';
import { RegionService } from 'src/app/services/Region/region.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { ISite } from 'src/app/interfaces/ISite';
import { IRegion } from 'src/app/interfaces/IRegion';
import { SiteDialogComponent } from './site-dialog/site-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-site',
  templateUrl: './site.component.html',
  styleUrls: ['./site.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class SiteComponent implements OnInit {
  parques: ISite[] = [];
  areasList: IRegion[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'nombre', 'area', 'editar', 'estado'];

  constructor(
    private parqueService: SiteService,
    private areaService: RegionService,
    private toastr: ToastService,
    private dialog: MatDialog,
    private confirmDialog: ConfirmDialogService,
    public permissions: PermissionsService
  ) {
    this.soloLectura = !this.permissions.esCompleto('mantenimientos');
    if (this.soloLectura) {
      this.columnas = this.columnas.filter(c => c !== 'editar');
    }
  }

  ngOnInit(): void {
    this.cargarParques();
    this.areaService.obtenerRegion().subscribe(areas => this.areasList = areas);
    this.parqueService.refresh$.subscribe(() => this.cargarParques());
  }

  cargarParques(): void {
    this.parqueService.obtenerTodosSite().subscribe(parques => this.parques = parques);
  }

  name(id?: number): string {
    return this.areasList.find(a => a.pK_IdRegion === id)?.name || 'N/A';
  }

  get parquesFiltrados(): ISite[] {
    const texto = this.search.trim().toLowerCase();
    if (!texto) return this.parques;
    return this.parques.filter(p =>
      p.name?.toLowerCase().includes(texto) || p.code?.toLowerCase().includes(texto)
    );
  }

  nuevoParque(): void {
    this.dialog.open(SiteDialogComponent, { width: '600px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargarParques(); });
  }

  editarParque(parque: ISite): void {
    this.dialog.open(SiteDialogComponent, { width: '600px', maxWidth: '95vw', data: parque })
      .afterClosed().subscribe(ok => { if (ok) this.cargarParques(); });
  }

  cambiarEstado(parque: ISite): void {
    this.confirmDialog.confirm({
      title: 'Change status',
      message: `Change the status of "${parque.name}"?`
    }).subscribe(confirmed => {
      if (confirmed && parque.code) {
        this.parqueService.cambiarEstado(parque.code).subscribe({
          next: () => this.toastr.success('Status updated', 'Success'),
          error: () => this.toastr.error('Could not change the status', 'Error')
        });
      }
    });
  }
}
