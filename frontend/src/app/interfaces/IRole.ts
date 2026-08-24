export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IRole[];
}

export interface IRole {
    pK_idRole: number;
    code?: string;
    name: string;
    description?: string;
    status?: string;
}
