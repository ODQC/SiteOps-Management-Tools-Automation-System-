import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IDepartment, Mensaje2 } from 'src/app/interfaces/IDepartment';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private urlApp: string;

  private urlAPI: string;

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Department/';
  }

  obtenerDepartment(): Observable<IDepartment[]> {
    return this.http.get<Mensaje>(`${this.urlApp + this.urlAPI}`)

      .pipe(map(this.transformarDepartment));
  }

  private transformarDepartment(respuesta: Mensaje): IDepartment[] {
    const departmentList: IDepartment[] = respuesta.object.map(
      (department) => {
        return {
          pk_IdDepartment: department.pk_IdDepartment,
          code: department.code,
          name: department.name,
          description: department.description,
          status: department.status,
        };
      }
    );
 
    return departmentList;
  }

  private tranformarDepartmentNA(respuesta: Mensaje2){
    const departmentNA: number = respuesta.object;
    return departmentNA;
  }

  public obtenerDepartmentNA(){
    return this.http.get<Mensaje2>(this.urlApp + this.urlAPI  + 'departmentNA').pipe(map(this.tranformarDepartmentNA));
  }
}
