import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { AppointmentsListComponent } from './appointments/appointments-list/appointments-list';
import { AppointmentsFormComponent } from './appointments/appointment-form/appointment-form';
import { DoctorsListComponent } from './doctors/doctors-list/doctors-list';
import { DoctorsFormComponent } from './doctors/doctor-form/doctor-form';
import { PatientsListComponent } from './patients/patients-list/patients-list';
import { PatientsFormComponent } from './patients/patient-form/patient-form';

export const routes: Routes = [
  { path: '', redirectTo: 'appointments', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'appointments', component: AppointmentsListComponent },
  { path: 'appointments/new', component: AppointmentsFormComponent },
  { path: 'appointments/edit/:id', component: AppointmentsFormComponent },
  { path: 'doctors', component: DoctorsListComponent },
  { path: 'doctors/new', component: DoctorsFormComponent },
  { path: 'doctors/edit/:id', component: DoctorsFormComponent },
  { path: 'patients', component: PatientsListComponent },
  { path: 'patients/new', component: PatientsFormComponent },
  { path: 'patients/edit/:id', component: PatientsFormComponent },
];
