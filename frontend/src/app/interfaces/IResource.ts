export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IResource[];
}

export interface IResource {
  pK_idResource: number;
  code?: string;
  type?: string;
  description?: string;
  status?: string;
}
