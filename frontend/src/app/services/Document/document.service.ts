import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DocumentService {
  private urlApp: string;
  private urlAPI: string;

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Document/';
  }

  subirDocument(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post(this.urlApp + this.urlAPI, formData);
  }

  getEmployeeImage(nationalId: string): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'employeeImage/' + nationalId);
  }
}
