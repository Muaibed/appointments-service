import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DoctorsService, Doctor } from '../doctors';

@Component({
  selector: 'app-doctor-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: `./doctor-form.html`
})
export class DoctorsFormComponent implements OnInit {
  doctor: Doctor = { id: 0, fullName: '', specialty: '', email: '' };
  isEdit = false;

  constructor(
    private service: DoctorsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.service.getDoctor(+id).subscribe(a => (this.doctor = a));
    }
  }

  save() {
    if (this.isEdit) {
      this.service.updateDoctor(this.doctor.id!, this.doctor).subscribe(() => this.router.navigate(['/doctors']));
    } else {
      this.service.createDoctor(this.doctor).subscribe(() => this.router.navigate(['/doctors']));
    }
  }
}
