import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IDeliverable } from 'src/app/interfaces/IDeliverable';

@Injectable({ providedIn: 'root' })
export class DeliverableService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Deliverable/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerDeliverables(): Observable<IDeliverable[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  obtenerDeliverablesPorTask(idTask: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'deliverablesByTask/' + idTask);
  }

  guardarDeliverable(deliverable: IDeliverable): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, deliverable);
  }

  actualizarDeliverable(id: number, deliverable: IDeliverable): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, deliverable);
  }

  eliminarDeliverable(id: number): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + id);
  }

  cambiarEstado(id: number): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'toggle-status/' + id).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
