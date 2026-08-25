import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, ITask } from 'src/app/interfaces/ITask';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/TaskItem/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerTaskes(): Observable<ITask[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  obtenerTaskesPorPlan(idPlan: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'tasksByProject/' + idPlan);
  }

  guardarTask(task: ITask): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, task);
  }

  actualizarTask(id: number, task: ITask): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, task);
  }

  eliminarTask(id: number): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + id);
  }

  cambiarEstado(id: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'toggle-status/' + id).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
