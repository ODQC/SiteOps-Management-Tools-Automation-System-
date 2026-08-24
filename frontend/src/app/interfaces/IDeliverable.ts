export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IDeliverable[];
}

export interface IDeliverable {
  pK_idDeliverable: number;
  code?: string;
  status?: string;
  fK_idDocument?: number;
  fK_idTask1?: number;
  descripcion?: string;
  nombre?: string;
  documentName?: string;
}
