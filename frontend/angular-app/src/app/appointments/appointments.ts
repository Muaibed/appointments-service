import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Appointment {
  id: number;
  date: string;
  doctorId: number;
  doctorName: string;
  patientId: number;
  patientName: string;
  status: string;
}

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  private baseUrl = 'http://localhost:5004/api'
  private apiUrl = `${this.baseUrl}/appointments`;

  constructor(private http: HttpClient) {}

  getAppointments(filters?:any): Observable<Appointment[]> {
    let params = new HttpParams();

    if (filters) {
      if (filters.doctorId != null) {
        params = params.set('doctorId', filters.doctorId);
      }
      if (filters.status) {
        params = params.set('status', filters.status);
      }
      if (filters.date) {
        params = params.set('date', filters.date);
      }
    }

  return this.http.get<Appointment[]>(this.apiUrl, { params });  }

  getAppointment(id: number): Observable<Appointment> {
    return this.http.get<Appointment>(`${this.apiUrl}/${id}`);
  }

  createAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.post<Appointment>(this.apiUrl, appointment, { headers: { 'Content-Type': 'application/json' }});
  }

  updateAppointment(id: number, appointment: Appointment): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, appointment);
  }

  deleteAppointment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getDoctors(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/doctors`);
  }

  getPatients(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/patients`);
  }
}
