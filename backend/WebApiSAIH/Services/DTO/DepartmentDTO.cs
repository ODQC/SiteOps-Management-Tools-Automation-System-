using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SAIH_Backend.Servicios.DTO
{
    [Serializable]
    [DataContract]
    public class DepartmentDTO
    {
        [DataMember]
        private long pk_IdDepartment;
        [DataMember]
        private string codigoDepartment;
        [DataMember]
        private string nombreDepartment;
        [DataMember]
        private string descripcionDepartment;
        [DataMember]
        private string estadoDepartment;

        public DepartmentDTO()
        {
            this.pk_IdDepartment = 0;
            this.codigoDepartment = "";
            this.nombreDepartment = "";
            this.descripcionDepartment = "";
            this.estadoDepartment = "";

        }

        public DepartmentDTO(long pkIdDepartment, string codigoDepartment, string nombreDepartment, string descripcionDepartment, string estadoDepartment)
        {
            this.pk_IdDepartment = pkIdDepartment;
            this.codigoDepartment = codigoDepartment;
            this.nombreDepartment = nombreDepartment;
            this.descripcionDepartment = descripcionDepartment;
            this.estadoDepartment = estadoDepartment;
        }

        public long PK_idDepartment { get => pk_IdDepartment; set => pk_IdDepartment = value; }
        public string Code { get => codigoDepartment; set => codigoDepartment = value; }
        public string Name { get => nombreDepartment; set => nombreDepartment = value; }
        public string Description { get => descripcionDepartment; set => descripcionDepartment = value; }
        public string Status { get => estadoDepartment; set => estadoDepartment = value; }


    }
}