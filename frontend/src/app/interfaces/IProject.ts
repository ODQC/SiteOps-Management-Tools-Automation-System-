export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IProject[];
}

export interface IProject {
  pK_idProject: number;
  fk_IdEmployee1?: number;
  fK_idRegion2?: number;
  code?: string;
  status?: string;
  endDate?: string;
  startDate?: string;
  progress?: string;
}
