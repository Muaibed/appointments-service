import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { DoctorsService, Doctor } from '../doctors';

@Component({
  selector: 'app-doctors-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: `./doctors-list.html`,
  styleUrls: ['../../app.scss']
})
export class DoctorsListComponent implements OnInit {
  doctors: Doctor[] = [];

  constructor(private service: DoctorsService, private router: Router) {}

  ngOnInit() {
    this.service.getDoctors().subscribe(data => (this.doctors = data));
  }

  create() {
    this.router.navigate(['/doctors/new']);
  }

  edit(id: number) {
    this.router.navigate(['/doctors/edit', id]);
  }

  delete(id: number) {
    this.service.deleteDoctor(id).subscribe(() => {
      this.doctors = this.doctors.filter(a => a.id !== id);
    });
  }
}
