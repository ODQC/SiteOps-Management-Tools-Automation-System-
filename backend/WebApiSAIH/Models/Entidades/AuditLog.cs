using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAIH_Backend.Datos.Entidades
{
    [Table("auditLog")]
    public class AuditLog
    {

        [Required]
        private string description;
        [Required]
        private DateTime date;   
        [ForeignKey("FK_idEmployee2")]
        private long? fk_IdEmployee2;
        [Key]
        private long pk_idAuditLog;

        public AuditLog(string description, DateTime date, long? fk_IdEmployee2, long pk_idAuditLog)
        {
            this.description = description;
            this.date = date;
            this.fk_IdEmployee2 = fk_IdEmployee2;
            this.pk_idAuditLog = pk_idAuditLog;
        }

        public AuditLog()
        {
            this.date = new DateTime();
            this.description = "";
            this.fk_IdEmployee2 = 0;
            this.pk_idAuditLog = 0;
        }

        public long PK_idAuditLog { get => pk_idAuditLog; set => pk_idAuditLog = value; }
        public long? Fk_IdEmployee2 { get => fk_IdEmployee2; set => fk_IdEmployee2 = value; }
        public string Description { get => description; set => description = value; }
        public DateTime Date { get => date; set => date = value; }
    }
}
