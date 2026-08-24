using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Models.Entidades
{
    [Table("email")]
    public class Email
    {
        [Key]
        private long pk_idEmail;
        [Required]
        private string de;
        [Required]
        private string para;
        [ForeignKey("email_template_id")]
        private long emailTemplateId;
        [Required]
        private DateTime fecha;

        public Email()
        {
            pk_idEmail = 0;
            de = "";
            para = "";
            emailTemplateId = 0;
            fecha = DateTime.Now;
        }

        public long PK_idEmail { get => pk_idEmail; set => pk_idEmail = value; }
        public string De { get => de; set => de = value; }
        public string Para { get => para; set => para = value; }
        public long Email_template_id { get => emailTemplateId; set => emailTemplateId = value; }
        public DateTime Fecha { get => fecha; set => fecha = value; }
    }
}
