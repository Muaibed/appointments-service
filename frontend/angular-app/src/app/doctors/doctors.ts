import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';

export interface Doctor {
  id: number;
  fullName: string;
  specialty: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  private doctorsUrl = `${environment.apiUrl}/doctors`;

  constructor(private http: HttpClient) {}

  getDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.doctorsUrl);
  }

  getDoctor(id: number): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.doctorsUrl}/${id}`);
  }

  createDoctor(data: any): Observable<Doctor> {
    return this.http.post<Doctor>(this.doctorsUrl, data);
  }

  updateDoctor(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.doctorsUrl}/${id}`, data);
  }

  deleteDoctor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.doctorsUrl}/${id}`);
  }
}
