import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PatientsService, Patient } from '../patients';

@Component({
  selector: 'app-patient-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: `./patient-form.html`,
  styleUrls: ['../../app.scss']
})
export class PatientsFormComponent implements OnInit {
  patient: Patient = { id: 0, fullName: '', email: '', dateOfBirth: '' };
  isEdit = false;

  constructor(
    private service: PatientsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.service.getPatient(+id).subscribe(a => {
        a.dateOfBirth = a.dateOfBirth.substring(0, 16);
        (this.patient = a)
      });
    }
  }

  save() {
    if (this.isEdit) {
      this.service.updatePatient(this.patient.id!, this.patient).subscribe(() => this.router.navigate(['/patients']));
    } else {
      this.service.createPatient(this.patient).subscribe(() => this.router.navigate(['/patients']));
    }
  }
}
