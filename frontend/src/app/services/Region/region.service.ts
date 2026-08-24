import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Mensaje, IRegion, Mensaje2 } from 'src/app/interfaces/IRegion';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

export class RegionService {

  private urlApp: string;
  private urlAPI: string;

  private _refres$ = new Subject<void>();

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Region/';

  }

  get refresh$() {
    return this._refres$;
  }

  obtenerRegion(): Observable<IRegion[]> {
    return this.http.get<Mensaje>(`${this.urlApp + this.urlAPI}`).pipe(map(this.tranformarAreasConservacion));
  }

  private tranformarAreasConservacion(respuesta: Mensaje): IRegion[] {

    const areasList: IRegion[] = respuesta.object.map(area => {
      return {
        pK_IdRegion: area.pK_IdRegion,
        code: area.code,
        description: area.description,
        status: area.status,
        name: area.name,
      }
    })
    return areasList;
  }

  private tranformarAreaNA(respuesta: Mensaje2){
    const areaNA: number = respuesta.object;
    return areaNA;
  }

  public obtenerAreaNA(){
    return this.http.get<Mensaje2>(this.urlApp + this.urlAPI  + 'regionNA').pipe(map(this.tranformarAreaNA));
  }

  guardarArea(area: IRegion): Observable<any> {
    return this.http.post(this.urlApp + this.urlAPI, area);
  }

  actualizarArea(id: number, area: IRegion): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + id, area);
  }

  eliminarArea(code: string): Observable<any> {
    return this.http.delete(this.urlApp + this.urlAPI + code);
  }

  cambiarEstado(code: string): Observable<any> {
    return this.http.get(this.urlApp + this.urlAPI + 'cambiarEstado/' + code).pipe(
      map(res => { this._refres$.next(); return res; })
    );
  }
}
