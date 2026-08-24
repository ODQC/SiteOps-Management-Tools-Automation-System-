using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Datos.Entidades
{
    [Table("department")]
    public partial class Department
    {
        [Key]
        private long pk_IdDepartment;
        [Required]
        private string codigoDepartment;
        [Required]
        private string nombreDepartment;
        [Required]
        private string descripcionDepartment;
        [Required]
        private string estadoDepartment;

        public Department()
        {
            this.pk_IdDepartment = 0;
            this.codigoDepartment = "";
            this.nombreDepartment = "";
            this.descripcionDepartment = "";
            this.estadoDepartment = "";

        }

        public Department(long pkIdDepartment, string codigoDepartment, string nombreDepartment, string descripcionDepartment, string estadoDepartment)
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