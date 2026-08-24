using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSAIH.Models.Entidades
{
    [Table("task")]
    public class TaskItem
    {
        [Key]
        private long pk_idTaskItem;

        [Required]
        private String code;

        [Required]
        private String name;

        [Required]
        private DateTime completionDate;

        [Required]
        private String collaborators;

        [Required]
        private String notes;

        [Required]
        private String taskStatus;

        [Required]
        private String estado;

        [ForeignKey("FK_idGoal1")]
        private long? fk_IdGoal1;

        [ForeignKey("FK_idProject")]
        private long? fK_idProject;

        public TaskItem(long pk_idTaskItem, String code, String name, DateTime completionDate, String collaborators,
            String notes, String taskStatus, String estado, long? fk_IdGoal1)
        {
            this.pk_idTaskItem = pk_idTaskItem;
            this.code = code;
            this.name = name;
            this.completionDate = completionDate;
            this.collaborators = collaborators;
            this.notes = notes;
            this.taskStatus = taskStatus;
            this.estado = estado;
            this.fk_IdGoal1 = fk_IdGoal1;
        }

        public TaskItem()
        {
            this.pk_idTaskItem = 0;
            this.completionDate = new DateTime();
            this.code = "";
            this.name = "";
            this.collaborators = "";
            this.notes = "";
            this.estado = "";
            this.taskStatus = "";
            this.fk_IdGoal1 = 0;
        }

        public long PK_idTaskItem { get => pk_idTaskItem; set => pk_idTaskItem = value; }
        public long? Fk_IdGoal1 { get => fk_IdGoal1; set => fk_IdGoal1 = value; }
        public DateTime CompletionDate { get => completionDate; set => completionDate = value; }
        public String Code { get => code; set => code = value; }
        public String Name { get => name; set => name = value; }
        public String Collaborators { get => collaborators; set => collaborators = value; }
        public String Status { get => estado; set => estado = value; }
        public String TaskStatus { get => taskStatus; set => taskStatus = value; }
        public String Notes { get => notes; set => notes = value; }
        public long? FK_idProject { get => fK_idProject; set => fK_idProject = value; }
    }
}
