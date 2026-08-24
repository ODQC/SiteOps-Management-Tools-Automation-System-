export interface Mensaje {
  codigo:  string;
  mensaje: string;
  object:  Employees[];
}
//Se crea la interfacede Employee que recibe el objeto Json de Employee
export interface Employees {
  pK_idEmployee?:           number;
  nationalId:          string;
  firstName:          string;
  lastName:  string;
  secondLastName: string;
  phone:        string;
  email:           string;
  password:      string;
 // fotoProfile:             null;
 // rutaFoto:               null;
  status:          string;
  fK_idDepartment1:     number;
  fK_idSite1:   number;
  fK_idRole1:       number;
}
