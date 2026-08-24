import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Mensaje, ISite, Mensaje2 } from 'src/app/interfaces/ISite';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class SiteService {
  private urlApp: string;
  private urlAPI: string;

  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {

    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Site/';

  }

  get refresh$() {
    return this._refres$;
  }

  // Servicio para traer una lista de Parques Nacionales según el área de conservación
  obtenerSite(id: number): Observable<ISite[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI  + id).pipe(map(this.tranformarSite));
  }

  obtenerTodosSite(): Observable<ISite[]> {
    return this.http.get<Mensaje>(this.urlApp + this.urlAPI).pipe(map(this.tranformarSite));
  }

  private tranformarSite(respuesta: Mensaje): ISite[] {

    const siteList: ISite[] = respuesta.object.map(parque => {
      return {
        pK_IdSite: (parque as any).pK_IdSite,
        code: parque.code,
        description: parque.description,
        status: parque.status,
        name: parque.name,
        fK_idRegion1: (parque as any).fK_idRegion1

      }
    })
    return siteList;
  }

  private tranformarParqueNA(respuesta: Mensaje2){
    const parqueNA: number = respuesta.object;
    return parqueNA;
  }

  public obtenerParqueNA(){
    return this.http.get<Mensaje2>(this.urlApp + this.urlAPI  + 'parqueNA').pipe(map(this.tranformarParqueNA));
  }

  guardarParque(parque: any): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, parque);
  }

  actualizarParque(id: number, parque: any): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, parque);
  }

  eliminarParque(code: string): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + code);
  }

  cambiarEstado(code: string): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'cambiarEstado/' + code).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
