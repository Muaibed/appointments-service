import { Component, OnInit } from '@angular/core';
import { FormsModule, } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AppointmentsService, Appointment } from '../appointments';


@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: `./appointment-form.html`
})
export class AppointmentsFormComponent implements OnInit {
  appointment: Appointment = { id: 0, doctorId: 0, doctorName: '', patientId: 0, patientName: '', date: '', status: '' };
  isEdit = false;

  doctors: any[] = [];
  patients: any[] = [];

  constructor(
    private service: AppointmentsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.service.getAppointment(+id).subscribe(a => {
        a.date = a.date.substring(0, 16);
       (this.appointment = a)
      });
    }
    this.loadDoctors();
    this.loadPatients();
  }

  loadDoctors() {
    this.service.getDoctors().subscribe(data => (this.doctors = data));
  }

  loadPatients() {
    this.service.getPatients().subscribe(data => (this.patients = data));
  }

  // onSubmit() {
  //   if (this.form.valid) {
  //     this.service.createAppointment(this.form.value).subscribe({
  //       next: res => alert('Appointment created successfully!'),
  //       error: err => console.error(err)
  //     });
  //   }
  // }
  save() {
    if (this.isEdit) {
      this.service.updateAppointment(this.appointment.id!, this.appointment).subscribe(() => this.router.navigate(['/appointments']));
    } else {
      this.service.createAppointment(this.appointment).subscribe(() => this.router.navigate(['/appointments']));
    }
  }
}
