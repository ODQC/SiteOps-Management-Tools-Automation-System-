using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class ProjectTaskDTO
    {
        [DataMember]
        private long pK_idProjectTask;

        [DataMember]
        private long fK_idProject;

        [DataMember]
        private long fK_idTask;

        public ProjectTaskDTO()
        {
            this.pK_idProjectTask = 0;
            this.fK_idProject = 0;
            this.fK_idTask = 0;
        }

        public ProjectTaskDTO(long pK_idProjectTask, long fK_idProject, long fK_idTask)
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
