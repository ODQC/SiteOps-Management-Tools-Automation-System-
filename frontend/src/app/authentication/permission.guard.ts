import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { Modulo, PermissionsService } from '../services/Permissions/permissions.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard {
  constructor(private permissions: PermissionsService, private router: Router) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const modulo = next.data['modulo'] as Modulo | undefined;
    if (!modulo || this.permissions.tieneAcceso(modulo)) {
      return true;
    }
    this.router.navigate(['/sidebar']);
    return false;
  }
}
