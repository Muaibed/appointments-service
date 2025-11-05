import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';

export interface Patient {
  id: number;
  fullName: string;
  email: string;
  dateOfBirth: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientsService {
  private patientsUrl = `${environment.apiUrl}/patients`;

  constructor(private http: HttpClient) {}

  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.patientsUrl);
  }

  getPatient(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.patientsUrl}/${id}`);
  }

  createPatient(data: any): Observable<Patient> {
    return this.http.post<Patient>(this.patientsUrl, data);
  }

  updatePatient(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.patientsUrl}/${id}`, data);
  }

  deletePatient(id: number): Observable<void> {
    return this.http.delete<void>(`${this.patientsUrl}/${id}`);
  }
}
