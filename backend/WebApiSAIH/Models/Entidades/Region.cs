using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SAIH_Backend.Datos.Entidades
{

    [Table("region")]
    public partial class Region
    {
        [Key]
        private long pkIdRegion;

        [Required]
        private string code;

        [Required]
        private string description;

        [Required]
        private string status;

        [Required]
        private string name;


        public Region()
        {
            this.pkIdRegion = 0;
            this.code = "";
            this.description = "";
            this.status = "";
            this.name = "";
        }

        public Region(long pkIdRegion, string code, string name, string description, string status)
        {
            this.pkIdRegion = pkIdRegion;
            this.code = code;
            this.description = description;
            this.status = status;
            this.name = name;
        }

        public long PK_IdRegion { get => pkIdRegion; set => pkIdRegion = value; }
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public string Name { get => name; set => name = value; }


    }
}