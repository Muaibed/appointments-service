import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AppointmentsService, Appointment } from '../appointments';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-appointments-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: `./appointments-list.html`
})
export class AppointmentsListComponent implements OnInit {
  appointments: Appointment[] = [];

  constructor(private service: AppointmentsService, private router: Router) {}

  doctors: any[] = [];
  patients: any[] = [];
  filters = {
    doctorId: null,
    status: '',
    date: ''
  };
  
  ngOnInit() {
    this.service.getAppointments().subscribe((data: any) => this.appointments = data);

    // Load all appointments initially
    this.loadAppointments();
    this.loadDoctors();
    this.loadPatients();
  }

  loadDoctors() {
    this.service.getDoctors().subscribe(data => (this.doctors = data));
  }

  loadPatients() {
    this.service.getPatients().subscribe(data => (this.patients = data));
  }

  loadAppointments() {
  // Build query params based on filters
  const params: any = {};
  if (this.filters.doctorId) params.doctorId = this.filters.doctorId;
  if (this.filters.status) params.status = this.filters.status;
  if (this.filters.date) params.date = this.filters.date;

  this.service.getAppointments(params).subscribe((data: any) => {
    this.appointments = data;
    });
  }

  applyFilters() {
    this.loadAppointments();
  }

  create() {
    this.router.navigate(['/appointments/new']);
  }

  edit(id: number) {
    this.router.navigate(['/appointments/edit', id]);
  }

  delete(id: number) {
    this.service.deleteAppointment(id).subscribe(() => {
      this.appointments = this.appointments.filter(a => a.id !== id);
    });
  }
}
