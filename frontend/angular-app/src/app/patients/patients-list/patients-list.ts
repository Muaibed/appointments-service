import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { PatientsService, Patient } from '../patients';

@Component({
  selector: 'app-patients-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: `./patients-list.html`
})
export class PatientsListComponent implements OnInit {
  patients: Patient[] = [];

  constructor(private service: PatientsService, private router: Router) {}

  ngOnInit() {
    this.service.getPatients().subscribe(data => (this.patients = data));
  }

  create() {
    this.router.navigate(['/patients/new']);
  }

  edit(id: number) {
    this.router.navigate(['/patients/edit', id]);
  }

  delete(id: number) {
    this.service.deletePatient(id).subscribe(() => {
      this.patients = this.patients.filter(a => a.id !== id);
    });
  }
}
