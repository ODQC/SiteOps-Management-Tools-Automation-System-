export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IDepartment[];
}

export interface Mensaje2 {
  codigo: string;
  mensaje: string;
  object: number;
}

export interface IDepartment {
  pk_IdDepartment: number;
  code?: string;
  name: string;
  description?: string;
  status?: string;
}






