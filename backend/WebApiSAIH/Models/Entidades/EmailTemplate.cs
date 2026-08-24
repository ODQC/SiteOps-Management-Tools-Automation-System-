using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Models.Entidades
{
    [Table("email_template")]
    public partial class EmailTemplate
    {
        [Key]
        private long pk_idTemplate;
        [Required]
        private string content;
        [Required]
        private string name;

        public EmailTemplate()
        {
            pk_idTemplate = 0;
            content = "";
            name = "";
        }

        public long PK_idTemplate { get => pk_idTemplate; set => pk_idTemplate = value; }
        public string Content { get => content; set => content = value; }
        public string Name { get => name; set => name = value; }
    }
}
