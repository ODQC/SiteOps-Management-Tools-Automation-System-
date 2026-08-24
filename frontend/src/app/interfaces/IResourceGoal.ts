export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IResourceGoal[];
}

export interface IResourceGoal {
  pK_idGoalResource: number;
  fK_idResource2: number;
  fK_idGoal2: number;
}
