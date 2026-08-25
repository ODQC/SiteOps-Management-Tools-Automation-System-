import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { map, shareReplay } from 'rxjs/operators';
import { ToastService } from 'src/app/services/Toast/toast.service';
import { EmployeesService } from 'src/app/services/Employees/employees.service';
import { ConfirmDialogService } from 'src/app/shared/confirm-dialog/confirm-dialog.service';
import { IAction } from 'src/app/interfaces/IAction';
import { AuditLog } from 'src/app/SystemActions/AuditLog';
import { Action } from 'src/app/SystemActions/Action';
import { AuditLogService } from 'src/app/services/AuditLog/audit-log.service';
import { PermissionsService } from 'src/app/services/Permissions/permissions.service';
import { DocumentService } from 'src/app/services/Document/document.service';
import { ThemeService } from 'src/app/services/Theme/theme.service';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class SidebarComponent implements OnInit, IAction {

  isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset).pipe(
    map(result => result.matches),
    shareReplay()
  );

  sidenavOpened = true;
  fotoProfile: string | null = null;

  constructor(
    public employeeService: EmployeesService,
    public permissions: PermissionsService,
    public themeService: ThemeService,
    private documentService: DocumentService,
    private router: Router,
    private taskService: AuditLogService,
    private toastr: ToastService,
    private breakpointObserver: BreakpointObserver,
    private confirmDialog: ConfirmDialogService
  ) { }

  ngOnInit(): void {
    this.isHandset$.subscribe(isHandset => {
      this.sidenavOpened = !isHandset;
    });

    const nationalId = this.employeeService.cargarProfileEmployee().NationalId;
    this.documentService.getEmployeeImage(nationalId).subscribe({
      next: (res: any) => {
        this.fotoProfile = res?.object ? 'data:image/jpeg;base64,' + res.object : null;
      },
      error: () => {
        this.fotoProfile = null;
      }
    });
  }

  logout(): void {
    this.confirmDialog.confirm({
      title: 'Log out?',
      message: 'Are you sure you want to log out?',
      confirmText: 'Log out'
    }).subscribe(confirmed => {
      if (confirmed) {
        let profileU = this.employeeService.cargarProfileEmployee();
        let descripcion: string = this.generarDescripcion(profileU.NationalId, Action.FinalizarSesion);
        let task = new AuditLog(parseInt(profileU.UserID), descripcion);
        this.registrarTask(task);
        localStorage.removeItem('token');
        this.router.navigateByUrl('/login');
      }
    });
  }

  generarDescripcion(nationalId: string, descripcion: string): string {
    return `Employee ${nationalId}${descripcion}`;
  }

  registrarTask(taskU: AuditLog): void {
    this.taskService.registrarAuditLog(taskU).subscribe(
      (res: any) => {
        if (res.codigo == '201') {
          console.log('Task registrada');
        }
      },
      (err: any) => {
        console.log(err.mensaje);
        this.toastr.error(err.codigo, 'Internal server error...');
      }
    );
  }

  administrarEmployees(idAdmin: string, descripcion: string, nationalIdRegistro: string): string {
    return `Employee ${idAdmin}${descripcion}${nationalIdRegistro}`;
  }
}
