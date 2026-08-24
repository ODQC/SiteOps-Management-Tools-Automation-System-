export interface Mensaje {
  codigo:  string;
  mensaje: string;
  object:  ISite[];
}

export interface Mensaje2 {
  codigo:  string;
  mensaje: string;
  object:  number;
}

export interface ISite {
  pK_IdSite: number;
  code?: string;
  description?: string;
  status?: string;
  name: string;
  fK_idRegion1?: number;
}
