import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { Mensaje, IResource } from 'src/app/interfaces/IResource';

@Injectable({ providedIn: 'root' })
export class ResourceService {
  private urlApp: string;
  private urlAPI: string;
  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Resource/';
  }

  get refresh$() {
    return this._refres$;
  }

  obtenerResources(): Observable<IResource[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(res => res.object));
  }

  guardarResource(resource: IResource): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, resource);
  }

  actualizarResource(id: number, resource: IResource): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, resource);
  }

  eliminarResource(code: string): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + code);
  }

  cambiarEstado(code: string): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'toggle-status/' + code).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
