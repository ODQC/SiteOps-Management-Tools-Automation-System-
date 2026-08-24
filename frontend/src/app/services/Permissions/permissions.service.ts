import { Injectable } from '@angular/core';
import { EmployeesService } from '../Employees/employees.service';

export type NivelAcceso = 'ninguno' | 'lectura' | 'completo';

export type Modulo =
  | 'employeesRegistro'
  | 'employeesConsulta'
  | 'mantenimientos'
  | 'resources'
  | 'goals'
  | 'planesTrabajo'
  | 'tasks'
  | 'deliverables';

const MATRIZ: Record<string, Record<Modulo, NivelAcceso>> = {
  'Admin': {
    employeesRegistro: 'completo',
    employeesConsulta: 'completo',
    mantenimientos: 'completo',
    resources: 'completo',
    goals: 'completo',
    planesTrabajo: 'completo',
    tasks: 'completo',
    deliverables: 'completo'
  },
  'Site Manager': {
    employeesRegistro: 'ninguno',
    employeesConsulta: 'lectura',
    mantenimientos: 'ninguno',
    resources: 'completo',
    goals: 'completo',
    planesTrabajo: 'completo',
    tasks: 'completo',
    deliverables: 'ninguno'
  },
  'Employee': {
    employeesRegistro: 'ninguno',
    employeesConsulta: 'ninguno',
    mantenimientos: 'ninguno',
    resources: 'ninguno',
    goals: 'ninguno',
    planesTrabajo: 'lectura',
    tasks: 'completo',
    deliverables: 'completo'
  },
  'Supervisor': {
    employeesRegistro: 'ninguno',
    employeesConsulta: 'lectura',
    mantenimientos: 'lectura',
    resources: 'lectura',
    goals: 'lectura',
    planesTrabajo: 'lectura',
    tasks: 'lectura',
    deliverables: 'lectura'
  }
};

const CLAIM_ROLE = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

@Injectable({ providedIn: 'root' })
export class PermissionsService {

  constructor(private employeesService: EmployeesService) {}

  rolActual(): string {
    const profile: any = this.employeesService.cargarProfileEmployee();
    const role = profile?.role ?? profile?.[CLAIM_ROLE];
    if (Array.isArray(role)) {
      return role[0] || '';
    }
    return role || '';
  }

  nivel(modulo: Modulo): NivelAcceso {
    const rol = this.rolActual();
    return MATRIZ[rol]?.[modulo] ?? 'ninguno';
  }

  tieneAcceso(modulo: Modulo): boolean {
    return this.nivel(modulo) !== 'ninguno';
  }

  esCompleto(modulo: Modulo): boolean {
    return this.nivel(modulo) === 'completo';
  }
}
