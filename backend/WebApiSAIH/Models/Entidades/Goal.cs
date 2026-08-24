using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Models.Entidades
{
    [Table("goal")]
    public class Goal
    {
        [Key]
        private long pK_idGoal;

        [Required]
        private String code;

        [Required]
        private String name;

        [Required]
        private String description;

        [Required]
        private String year;

        [Required]
        private String status;

        public Goal()
        {
            this.pK_idGoal = 0;
            this.code = "";
            this.name = "";
            this.description = "";
            this.year = "";
            this.status = "";

        }

        public Goal(long pKIdGoal, string code, string name, string description, string year, string status)
        {
            this.pK_idGoal = pKIdGoal;
            this.code = code;
            this.name = name;
            this.description = description;
            this.year = year;
            this.status = status;
        }

        public long PK_idGoal { get => pK_idGoal; set => pK_idGoal = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Year { get => year; set => year = value; }
        public string Status { get => status; set => status = value; }
    }
}