import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Mensaje, Employees } from 'src/app/models/employees';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import jwt_decode from "jwt-decode";
import { IEmployeeProfile } from 'src/app/interfaces/IEmployeeProfile';
import { IAuthentication } from 'src/app/interfaces/IAuthentication';


@Injectable({
  providedIn: 'root'
})

export class EmployeesService {

  //Url del servidor
  private urlApp: string;
  //Url servicio
  private urlAPI: string;
  //public list: Employees[];
  private _refres$ = new Subject<void>();
  private actualizarFormulario = new BehaviorSubject<Employees>({} as any);

  constructor(private http: HttpClient) {
    this.urlApp = environment.apiUrl;
    this.urlAPI = 'api/Employee';
  }

  login(formData: IAuthentication) {
    const credenciales = { Email: formData.correoElectronico, Password: formData.password };
    return this.http.post(this.urlApp + "api/Auth/Login", credenciales);
  }

  guardarEmployee(employee: Employees): Observable<Employees> {
    return this.http.post<Employees>(this.urlApp + this.urlAPI, employee);
  }

  obtenerEmployee(id: string): Observable<Employees> {
    return this.http.get<Employees>(this.urlApp + this.urlAPI + '/' + id);
  }

  actualizarEmployee(id:string, employee:Employees) :Observable<Employees>{
    return this.http.put<Employees>(this.urlApp + this.urlAPI + '/' + id, employee);
  }

  actualizarMiProfile(profile: { phone: string, fK_idDocument1?: number }): Observable<any> {
    return this.http.put(this.urlApp + this.urlAPI + '/my-profile', profile);
  }

  actualizar(employee: Employees){
    this.actualizarFormulario.next(employee);
  }

  obtenerEmployees$():Observable<Employees>{
    return this.actualizarFormulario.asObservable();
  // ===================================     MAIKOL    ==========================
  }

  //Trae la lista de los objetos
  obtenerEmployees(): Observable<Employees[]> {
    return this.http.get<Mensaje>(`${this.urlApp + this.urlAPI}`)
      .pipe(
        map(this.tranformarUsurarios)
      )
  }

  get refresh$() {
    return this._refres$;
  }

  private tranformarUsurarios(respuesta: Mensaje): Employees[] {
    //metodo crea el
    const employeeList: Employees[] = respuesta.object.map(usua => {
      return {
        pK_idEmployee: usua.pK_idEmployee,
        nationalId: (usua as any).nationalId,
        firstName: usua.firstName,
        lastName: usua.lastName,
        secondLastName: usua.secondLastName,
        phone: usua.phone,
        email: usua.email,
        password: usua.password,
        // fotoProfile:             null;
        // rutaFoto:               null;
        status: usua.status,
        fK_idDepartment1: usua.fK_idDepartment1,
        fK_idSite1: usua.fK_idSite1,
        fK_idRole1: usua.fK_idRole1
      }
    })
    return employeeList;
  }
  deshabilitarEmployee(id: string): Observable<Employees> {
    return this.http.get<Employees>(this.urlApp + this.urlAPI + "/cambiarEstado/" + id).pipe(
      tap(() => {
        this._refres$.next();
      })
    );
  }
  editarEmployee(id: string): Observable<Employees> {
    return this.http.get<Employees>(this.urlApp + this.urlAPI + id).pipe(
      tap(() => {
        this._refres$.next();
      })
    );
  }

  /*    cmabiar password   */
  recuperarPassword(correoElectronico: string): Observable<Employees> {
    return this.http.get<Employees>(this.urlApp + this.urlAPI + "/recuperarPassword/" + correoElectronico);
  }


  public cargarProfileEmployee():IEmployeeProfile{

    let token = localStorage.getItem("token");

    let decoded = JSON.parse(JSON.stringify(jwt_decode(token!)));
    let profileU : IEmployeeProfile = decoded;

    return profileU
  }
}
