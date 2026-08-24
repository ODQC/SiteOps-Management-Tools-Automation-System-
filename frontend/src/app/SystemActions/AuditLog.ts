export class AuditLog
{
    pK_idAuditLog: number;
    fk_IdEmployee2:         number;
    description: string;
    date:       string;
    
    constructor(idEmployee :number, descripcion : string){
        this.pK_idAuditLog = 0;
        this.fk_IdEmployee2 = idEmployee;
        this.description = descripcion;
        this.date =  this.getFecha();
    }
    //método para generar descripciones de tasks de employee
    generarDescripcion(cedula :string, descripcion:string):string{
        return "El employee"+ cedula + descripcion
    }
    getFecha():string{
        const tiempoTranscurrido = Date.now();
        const hoy = new Date(tiempoTranscurrido);
        var fecha:string = hoy.toISOString();
        return fecha
    }
   
}