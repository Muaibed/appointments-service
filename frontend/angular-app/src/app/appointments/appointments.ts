import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';

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
  private apiUrl = `${environment.apiUrl}`;
  private appointmentsUrl = `${this.apiUrl}/appointments`;

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

    return this.http.get<Appointment[]>(this.appointmentsUrl, { params });  
  }

  getAppointment(id: number): Observable<Appointment> {
    return this.http.get<Appointment>(`${this.appointmentsUrl}/${id}`);
  }

  createAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.post<Appointment>(this.appointmentsUrl, appointment, { headers: { 'Content-Type': 'application/json' }});
  }

  updateAppointment(id: number, appointment: Appointment): Observable<void> {
    return this.http.put<void>(`${this.appointmentsUrl}/${id}`, appointment);
  }

  deleteAppointment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.appointmentsUrl}/${id}`);
  }

  getDoctors(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/doctors`);
  }

  getPatients(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/patients`);
  }
}
