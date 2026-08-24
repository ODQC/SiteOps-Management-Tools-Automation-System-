import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IGoal } from 'src/app/interfaces/IGoal';

@Injectable({ providedIn: 'root' })
export class GoalService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Goal/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerGoals(): Observable<IGoal[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  guardarGoal(goal: IGoal): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, goal);
  }

  actualizarGoal(id: number, goal: IGoal): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, goal);
  }

  eliminarGoal(code: string): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + code);
  }

  cambiarEstado(code: string): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'cambiarEstado/' + code).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
