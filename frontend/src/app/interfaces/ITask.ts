export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: ITask[];
}

export interface ITask {
  pK_idTaskItem: number;
  fk_IdGoal1?: number;
  completionDate?: string;
  code?: string;
  name?: string;
  collaborators?: string;
  status?: string;
  taskStatus?: string;
  notes?: string;
  fK_idProject?: number;
}
