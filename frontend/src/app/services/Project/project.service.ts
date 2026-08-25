import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IProject } from 'src/app/interfaces/IProject';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Project/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerPlanesTrabajo(): Observable<IProject[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  guardarProject(plan: IProject): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, plan);
  }

  actualizarProject(id: number, plan: IProject): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, plan);
  }

  eliminarProject(id: number): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + id);
  }

  cambiarEstado(idPlan: number, idEmployee: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'toggle-status/' + idPlan + '/' + idEmployee).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }

  taskesPorPlan(id: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'tasksByProject/' + id);
  }
}
