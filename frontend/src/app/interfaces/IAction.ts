import { AuditLog } from "../SystemActions/AuditLog";

// Interface for employee audit log entries
export interface IAction{

     //generates and saves an employee audit log entry
    registrarTask(taskU : AuditLog):void ;
     //generates the description for an employee audit log entry
    generarDescripcion(nationalId :string, descripcion:string):string;
    //generates the description for an employee record change made by an admin
    administrarEmployees(idAdmin:string, descripciion:string, nationalIdRegistro:string ):string

}
