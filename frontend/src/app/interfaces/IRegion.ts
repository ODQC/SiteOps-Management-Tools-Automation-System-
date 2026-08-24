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

//Se crea la interface de Área de conservación que recibe el objeto Json de Area de conservación
export interface IRegion {
  pK_IdRegion: number;
  code?: string;
  description?: string;
  status?: string;
  name: string;
}
