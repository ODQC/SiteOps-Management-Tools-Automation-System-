import { Pipe, PipeTransform } from '@angular/core';
import { Employees } from 'src/app/models/employees';
import { IRole } from 'src/app/interfaces/IRole';
import { IDepartment } from 'src/app/interfaces/IDepartment';
import { ISite } from 'src/app/interfaces/ISite';

@Pipe({
    name: 'filter',
    standalone: false
})
export class FilterPipe implements PipeTransform {

  transform(
    employees: Employees[],
    page: number = 0,
    search: string = '',
    searchType: string = '',
    statusFilter: string,
    roleFilter: string,
    areaFilter: string,
    siteFilter: string,
    departmentFilter: string,
    roleList: IRole[] = [],
    departmentList: IDepartment[] = [],
    siteList: ISite[] = []
  ): Employees[] {

    if (statusFilter === 'Activos') {
      employees = employees.filter(user => user.status?.includes('Activo'));
    } else if (statusFilter === 'Inactivos') {
      employees = employees.filter(user => user.status?.includes('Inactivo'));
    }

    if (searchType === 'Nombre') {
      employees = employees.filter(user => user.firstName?.includes(search));
    } else if (searchType === 'Apellidos') {
      employees = employees.filter(user => user.lastName?.includes(search) || user.secondLastName?.includes(search));
    } else if (searchType === 'Identificacion') {
      employees = employees.filter(user => user.nationalId?.includes(search));
    }

    if (roleFilter && roleFilter !== 'Todos') {
      const role = roleList.find(r => r.name === roleFilter);
      if (role) {
        employees = employees.filter(user => user.fK_idRole1 === role.pK_idRole);
      }
    }

    if (departmentFilter && departmentFilter !== 'Todos') {
      const department = departmentList.find(d => d.name === departmentFilter);
      if (department) {
        employees = employees.filter(user => user.fK_idDepartment1 === department.pk_IdDepartment);
      }
    }

    if (siteFilter && siteFilter !== 'Todos') {
      const site = siteList.find(s => s.name === siteFilter);
      if (site) {
        employees = employees.filter(user => user.fK_idSite1 === site.pK_IdSite);
      }
    }

    return employees.slice(page, page + 5);
  }
}
