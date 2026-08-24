using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class ProjectDTO
    {
        [DataMember]
        private String code;
        [DataMember]
        private DateTime startDate;
        [DataMember]
        private DateTime endDate;
        [DataMember]
        private String status;
        [DataMember]
        private long? fk_idEmployee1;
        [DataMember]
        private long? fk_idRegion2;
        [DataMember]
        private long pk_idProject;
        [DataMember]
        private string progress;

        public ProjectDTO(String code, DateTime periodoAsignado, String status, long? fk_idEmployee1, long? fk_idResource1,
            long pk_idProject, long? fk_idRegion2)
        {
            this.code = code;
            this.status = status;
            this.fk_idEmployee1 = fk_idEmployee1;
            this.pk_idProject = pk_idProject;
            this.fk_idRegion2 = fk_idRegion2;
        }
        public ProjectDTO()
        {
            this.code = "";
            this.status = "";
            this.fk_idEmployee1 = 0;
            this.pk_idProject = 0;
            this.fk_idRegion2 = 0;
        }

        public long PK_idProject { get => pk_idProject; set => pk_idProject = value; }
        public long? Fk_IdEmployee1 { get => fk_idEmployee1; set => fk_idEmployee1 = value; }
        public long? FK_idRegion2 { get => fk_idRegion2; set => fk_idRegion2 = value; }
        public string Code { get => code; set => code = value; }
        public string Status { get => status; set => status = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public string Progress { get => progress; set => progress = value; }

    }
}
