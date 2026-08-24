import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeFormComponent } from './pages/employee-form/employee-form.component';
import { SidebarComponent } from './pages/sidebar/sidebar.component';
import { EmployeeTableComponent } from './pages/employee-table/employee-table/employee-table.component';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { PermissionGuard } from './authentication/permission.guard';
import { LoginComponent } from './pages/login/login.component';
import { RegionComponent } from './pages/region/region.component';
import { SiteComponent } from './pages/site/site.component';
import { ResourceComponent } from './pages/resource/resource.component';
import { GoalComponent } from './pages/goal/goal.component';
import { ProjectComponent } from './pages/project/project.component';
import { DeliverableComponent } from './pages/deliverable/deliverable.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MyProfileComponent } from './pages/my-profile/my-profile.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'sidebar',
    component: SidebarComponent,
    canActivate: [AuthenticationGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'my-profile', component: MyProfileComponent },
      { path: 'registro', component: EmployeeFormComponent, canActivate: [PermissionGuard], data: { modulo: 'employeesRegistro' } },
      { path: 'consultas', component: EmployeeTableComponent, canActivate: [PermissionGuard], data: { modulo: 'employeesConsulta' } },
      { path: 'areas-conservacion', component: RegionComponent, canActivate: [PermissionGuard], data: { modulo: 'mantenimientos' } },
      { path: 'parques-nacionales', component: SiteComponent, canActivate: [PermissionGuard], data: { modulo: 'mantenimientos' } },
      { path: 'resources', component: ResourceComponent, canActivate: [PermissionGuard], data: { modulo: 'resources' } },
      { path: 'goals', component: GoalComponent, canActivate: [PermissionGuard], data: { modulo: 'goals' } },
      { path: 'planes-trabajo', component: ProjectComponent, canActivate: [PermissionGuard], data: { modulo: 'planesTrabajo' } },
      { path: 'deliverables', component: DeliverableComponent, canActivate: [PermissionGuard], data: { modulo: 'deliverables' } }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
