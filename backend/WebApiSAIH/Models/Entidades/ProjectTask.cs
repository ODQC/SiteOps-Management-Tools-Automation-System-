using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiSAIH.Models.Entidades
{
    [Table("project_task")]
    public class ProjectTask
    {
        [Key]
        private long pK_idProjectTask;

        [ForeignKey("FK_idProject")]
        [Required]
        private long fK_idProject;

        [ForeignKey("FK_idTask")]
        [Required]
        private long fK_idTask;

        public ProjectTask()
        {
            this.pK_idProjectTask = 0;
            this.fK_idProject = 0;
            this.fK_idTask = 0;
        }

        public ProjectTask(long pK_idProjectTask, long fK_idProject, long fK_idTask)
        {
            this.pK_idProjectTask = pK_idProjectTask;
            this.fK_idProject = fK_idProject;
            this.fK_idTask = fK_idTask;
        }

        public long PK_idProjectTask { get => pK_idProjectTask; set => pK_idProjectTask = value; }
        public long FK_idProject { get => fK_idProject; set => fK_idProject = value; }
        public long FK_idTask { get => fK_idTask; set => fK_idTask = value; }
    }
}
