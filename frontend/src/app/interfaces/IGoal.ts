export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IGoal[];
}

export interface IGoal {
  pK_idGoal: number;
  code?: string;
  name?: string;
  description?: string;
  year?: string;
  status?: string;
}
