export interface Mensaje {
  codigo: string;
  mensaje: string;
  object: IRegion[];
}

export interface Mensaje2 {
  codigo:  string;
  mensaje: string;
  object:  number;
}

//Region interface, receives the Region JSON object
export interface IRegion {
  pK_IdRegion: number;
  code?: string;
  description?: string;
  status?: string;
  name: string;
}
