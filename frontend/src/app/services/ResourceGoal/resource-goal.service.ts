import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IResourceGoal } from 'src/app/interfaces/IResourceGoal';

@Injectable({ providedIn: 'root' })
export class ResourceGoalService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/ResourceGoal/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerTodos(): Observable<IResourceGoal[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  vincular(fK_idResource2: number, fK_idGoal2: number): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, { fK_idResource2, fK_idGoal2 }).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }

  desvincular(pK_idGoalResource: number): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + pK_idGoalResource).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
