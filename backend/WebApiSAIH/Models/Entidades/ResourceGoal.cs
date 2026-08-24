using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Models.Entidades
{
    [Table("resource_goal")]
    public class ResourceGoal
    {
        [Key]
        private long pk_IdGoalResource;

        [ForeignKey("FK_idResource2")]
        [Required]
        private long fk_idResource2;

        [ForeignKey("FK_idGoal2")]
        [Required]
        private long fk_idGoal2;

        public ResourceGoal()
        {
            this.pk_IdGoalResource = 0;
            this.fk_idResource2 = 0;
            this.fk_idGoal2 = 0;
        }

        public ResourceGoal(long pk_IdGoalResource, long fk_idResource2, long fk_idGoal2)
        {
            this.pk_IdGoalResource = pk_IdGoalResource;
            this.fk_idResource2 = fk_idResource2;
            this.fk_idGoal2 = fk_idGoal2;
        }

        public long PK_idGoalResource { get => pk_IdGoalResource; set => pk_IdGoalResource = value; }
        public long Fk_idResource2 { get => fk_idResource2; set => fk_idResource2 = value; }
        public long Fk_idGoal2 { get => fk_idGoal2; set => fk_idGoal2 = value; }
    }
}
