using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class TaskItemDTO
    {
        [DataMember]
        private long pk_idTaskItem;

        [DataMember]
        private String code;

        [DataMember]
        private String name;

        [DataMember]
        private DateTime completionDate;

        [DataMember]
        private String collaborators;

        [DataMember]
        private String notes;

        [DataMember]
        private String taskStatus;

        [DataMember]
        private String estado;

        [DataMember]
        private long? fk_IdGoal1;

        [DataMember]
        private long? fK_idProject;

        public TaskItemDTO(long pk_idTaskItem, String code, String name, DateTime completionDate, String collaborators,
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

        public TaskItemDTO()
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
