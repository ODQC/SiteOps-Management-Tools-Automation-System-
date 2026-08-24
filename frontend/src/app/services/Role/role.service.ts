import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IRole } from 'src/app/interfaces/IRole';

@Injectable({
  providedIn: 'root',
})
export class RoleService {
  private urlApp: string;

  private urlAPI: string;

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Role/';
  }

  obtenerRol(): Observable<IRole[]> {
    return this.http.get<Mensaje>(`${this.urlApp + this.urlAPI}`).pipe(map(this.transformarRol));
  }

  private transformarRol(respuesta: Mensaje): IRole[] {
    const rolList: IRole[] = respuesta.object.map(
      (rol) => {
        return {
          pK_idRole: rol.pK_idRole,
          code: rol.code,
          name: rol.name,
          description: rol.description,
          status: rol.status
        };
      }
    );

    return rolList;
  }
}