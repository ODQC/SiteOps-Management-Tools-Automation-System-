import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Mensaje,IAuditLog } from 'src/app/interfaces/IAuditLog';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { AuditLog } from 'src/app/SystemActions/AuditLog';


@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
   //Url del servidor
   private urlApp: string;
   //Url servicio
   private urlAPI: string;
   //public list: Employees[];
   private _refres$ = new Subject<void>();
   private actualizarFormulario = new BehaviorSubject<IAuditLog>({} as any);

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/AuditLog';
  }

  registrarAuditLog(auditLog: AuditLog): Observable<IAuditLog> {
    return this.http.post<IAuditLog>(this.urlApp + this.urlAPI, this.transformarModelAInterface(auditLog));
  }

  obtenerAuditLogs(): Observable<IAuditLog[]> {
    return this.http.get<Mensaje>(`${this.urlApp + this.urlAPI}`)

      .pipe(map(this.transformarTaskemployee));
  }

  obtenerTaskesPorEmployee(id: number): Observable<IAuditLog[]> {

    return this.http.get<Mensaje>(this.urlApp + this.urlAPI  + id)

    .pipe(
      map(this.transformarTaskemployee)
    )
  }

  get refresh$(){
    return this._refres$;
  }
 private transformarModelAInterface(model : AuditLog):IAuditLog{
  let iTaskU : IAuditLog = {

          pK_idAuditLog : model.pK_idAuditLog,
          fk_IdEmployee2         : model.fk_IdEmployee2,
          description : model.description,
          date       : model.date,
  }

    return iTaskU;
 }
  private transformarTaskemployee(respuesta: Mensaje): IAuditLog[] {
    const AuditLogList: IAuditLog[] = respuesta.object.map(
      (actU) => {
        return {
          pK_idAuditLog: actU.pK_idAuditLog,
          fk_IdEmployee2:         actU.fk_IdEmployee2,
          description: actU.description,
          date:       actU.date,
        };
      }
    );

    return AuditLogList;
  }

}
