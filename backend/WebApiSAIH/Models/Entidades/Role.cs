using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Datos.Entidades
{
    [Table("role")]
    public partial class Role
    {
        [Key]
        private long pK_idRole;
        [Required]
        private string code;
        [Required]
        private string name;
        [Required]
        private string description;
        [Required]
        private string status;

        public Role()
        {
            this.pK_idRole = 0;
            this.code = "";
            this.name = "";
            this.description = "";
            this.status = "";
        }

        public Role(long pK_idRole, string code, string name, string description, string status)
        {
            this.pK_idRole = pK_idRole;
            this.code = code;
            this.name = name;
            this.description = description;
            this.status = status;
        }

        public long PK_idRole { get => pK_idRole; set => pK_idRole = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }


    }
}