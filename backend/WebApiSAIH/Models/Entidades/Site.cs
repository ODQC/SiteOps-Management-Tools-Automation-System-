using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Datos.Entidades
{
    [Table("site")]
    public partial class Site
    {
        [Key]
        private long pk_IdSite;
        [Required]
        private string code;
        [Required]
        private string description;
        [Required]
        private string status;
        [Required]
        private string name;
        [Required]
        [ForeignKey("FK_idRegion1")]
        private long fk_idRegion;

        public Site()
        {
            this.pk_IdSite = 0;
            this.code = "";
            this.description = "";
            this.status = "";
            this.name = "";
            this.fk_idRegion = 0;
        }

        public Site(long pk_IdSite, string code, string name, string description, string status, long fk_idRegion)
        {
            this.pk_IdSite = pk_IdSite;
            this.code = code;
            this.description = description;
            this.status = status;
            this.name = name;
            this.fk_idRegion = fk_idRegion;
        }

        public long PK_IdSite { get => pk_IdSite; set => pk_IdSite = value; }
        public string Code { get => code; set => code = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public string Name { get => name; set => name = value; }
        public long FK_idRegion1
        {
            get { return fk_idRegion; }
            set { fk_idRegion = value; }
        }
    }
}