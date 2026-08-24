import { AuditLog } from "../SystemActions/AuditLog";

// Interfaz diseñada para la implementación de tasks de Employee
export interface IAction{

     //método para generar y guardar tasks de employee
    registrarTask(taskU : AuditLog):void ;
     //método para generar descripciones de tasks de employee
    generarDescripcion(cedula :string, descripcion:string):string;
    //método para generar descripciones relacionadas con manenimientos de usarios hechos por el adminTI
    administrarEmployees(idAdmin:string, descripciion:string, cedulaRegistro:string ):string

}
