export interface Mensaje {
    codigo:  string;
    mensaje: string;
    object:  IAuditLog[];
  }

export interface IAuditLog
{
    pK_idAuditLog: number;
    fk_IdEmployee2:         number;
    description: string;
    date:       string;
}
  