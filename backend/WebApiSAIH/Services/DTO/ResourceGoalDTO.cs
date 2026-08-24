using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class ResourceGoalDTO
    {
        [DataMember]
        private long pk_idGoalResource;

        [DataMember]
        private long fk_idResource2;

        [DataMember]
        private long fk_idGoal2;

        public ResourceGoalDTO()
        {
            this.pk_idGoalResource = 0;
            this.fk_idResource2 = 0;
            this.fk_idGoal2 = 0;

        }

        public ResourceGoalDTO(long pk_idGoalResource, long fk_idResource2, long fk_idGoal2)
        {
            this.pk_idGoalResource = pk_idGoalResource;
            this.fk_idResource2 = fk_idResource2;
            this.fk_idGoal2 = fk_idGoal2;
        }

        public long PK_idGoalResource { get => pk_idGoalResource; set => pk_idGoalResource = value; }
        public long FK_idResource2 { get => fk_idResource2; set => fk_idResource2 = value; }
        public long FK_idGoal2 { get => fk_idGoal2; set => fk_idGoal2 = value; }
    }
}