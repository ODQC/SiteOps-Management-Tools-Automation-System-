import { Injectable } from '@angular/core';
import { IRole } from 'src/app/interfaces/IRole';
import { tipo, tipoIdentificacion } from 'src/app/interfaces/identificationType';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  public roleList: IRole[] = [];

  constructor() { 
  }

  private tipoId: tipoIdentificacion[] = [
    {
      tipoId: 1,
      nombre: 'Nacional'
    },
    {
      tipoId: 2,
      nombre: 'Dimex'
    }
  ];

  private tipoCedulas: tipo[] = [
    {
      codigo: 1,
      tipoId: 1,
    },
    {
      codigo: 2,
      tipoId: 2,
    }
  ];

  getTipoId(): tipoIdentificacion[]{
    return this.tipoId;
  }

  getTipo(): tipo[]{
    return this.tipoCedulas;
  }
}
