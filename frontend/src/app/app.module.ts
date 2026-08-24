import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi, withXhr } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

import { EmployeeFormComponent } from './pages/employee-form/employee-form.component';
import { EmployeeTableComponent } from './pages/employee-table/employee-table/employee-table.component';
import { SidebarComponent } from './pages/sidebar/sidebar.component';
import { EditEmployeeComponent } from './pages/edit-employee/edit-employee.component';
import { FilterPipe } from './pages/pipes/filter.pipe';
import { LoginComponent } from './pages/login/login.component';
import { EmployeesService } from './services/Employees/employees.service';
import { AutintificacionInterceptor } from './authentication/AuthenticationInterceptor';
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import { ForgotPasswordDialogComponent } from './pages/login/forgot-password-dialog/forgot-password-dialog.component';

import { RegionComponent } from './pages/region/region.component';
import { RegionDialogComponent } from './pages/region/region-dialog/region-dialog.component';
import { SiteComponent } from './pages/site/site.component';
import { SiteDialogComponent } from './pages/site/site-dialog/site-dialog.component';
import { ResourceComponent } from './pages/resource/resource.component';
import { ResourceDialogComponent } from './pages/resource/resource-dialog/resource-dialog.component';
import { LinkedGoalsDialogComponent } from './pages/resource/linked-goals-dialog/linked-goals-dialog.component';
import { GoalComponent } from './pages/goal/goal.component';
import { GoalDialogComponent } from './pages/goal/goal-dialog/goal-dialog.component';
import { ProjectComponent } from './pages/project/project.component';
import { ProjectDialogComponent } from './pages/project/project-dialog/project-dialog.component';
import { TasksDialogComponent } from './pages/project/tasks-dialog/tasks-dialog.component';
import { DeliverableComponent } from './pages/deliverable/deliverable.component';
import { DeliverableDialogComponent } from './pages/deliverable/deliverable-dialog/deliverable-dialog.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MyProfileComponent } from './pages/my-profile/my-profile.component';

@NgModule({ declarations: [
        AppComponent,
        EmployeeFormComponent,
        EmployeeTableComponent,
        SidebarComponent,
        EditEmployeeComponent,
        FilterPipe,
        LoginComponent,
        ConfirmDialogComponent,
        ForgotPasswordDialogComponent,
        RegionComponent,
        RegionDialogComponent,
        SiteComponent,
        SiteDialogComponent,
        ResourceComponent,
        ResourceDialogComponent,
        LinkedGoalsDialogComponent,
        GoalComponent,
        GoalDialogComponent,
        ProjectComponent,
        ProjectDialogComponent,
        TasksDialogComponent,
        DeliverableComponent,
        DeliverableDialogComponent,
        DashboardComponent,
        MyProfileComponent
    ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatSidenavModule,
        MatListModule,
        MatButtonModule,
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatCardModule,
        MatTableModule,
        MatDialogModule,
        MatMenuModule,
        MatDividerModule,
        MatProgressSpinnerModule,
        MatSnackBarModule,
        MatTooltipModule,
        MatChipsModule,
        MatBadgeModule,
        MatDatepickerModule,
        MatNativeDateModule], providers: [EmployeesService, {
            provide: HTTP_INTERCEPTORS,
            useClass: AutintificacionInterceptor,
            multi: true
        }, provideHttpClient(withXhr(), withInterceptorsFromDi())] })
export class AppModule { }
