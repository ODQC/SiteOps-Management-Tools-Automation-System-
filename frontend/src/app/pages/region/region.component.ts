import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RegionService } from 'src/app/services/Region/region.service';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { IRegion } from 'src/app/interfaces/IRegion';
import { RegionDialogComponent } from './region-dialog/region-dialog.component';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';

@Component({
  selector: 'app-region',
  templateUrl: './region.component.html',
  styleUrls: ['./region.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class RegionComponent implements OnInit {
  areas: IRegion[] = [];
  search: string = '';
  soloLectura = false;
  columnas: string[] = ['codigo', 'nombre', 'descripcion', 'editar', 'estado'];

  constructor(
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
    this.cargarAreas();
    this.areaService.refresh$.subscribe(() => this.cargarAreas());
  }

  cargarAreas(): void {
    this.areaService.obtenerRegion().subscribe(areas => this.areas = areas);
  }

  get areasFiltradas(): IRegion[] {
    const texto = this.search.trim().toLowerCase();
    if (!texto) return this.areas;
    return this.areas.filter(a =>
      a.name?.toLowerCase().includes(texto) || a.code?.toLowerCase().includes(texto)
    );
  }

  nuevaArea(): void {
    this.dialog.open(RegionDialogComponent, { width: '600px', maxWidth: '95vw' })
      .afterClosed().subscribe(ok => { if (ok) this.cargarAreas(); });
  }

  editarArea(area: IRegion): void {
    this.dialog.open(RegionDialogComponent, { width: '600px', maxWidth: '95vw', data: area })
      .afterClosed().subscribe(ok => { if (ok) this.cargarAreas(); });
  }

  cambiarEstado(area: IRegion): void {
    this.confirmDialog.confirm({
      title: 'Change status',
      message: `Change the status of "${area.name}"?`
    }).subscribe(confirmed => {
      if (confirmed && area.code) {
        this.areaService.cambiarEstado(area.code).subscribe({
          next: () => this.toastr.success('Status updated', 'Success'),
          error: () => this.toastr.error('Could not change the status', 'Error')
        });
      }
    });
  }
}
