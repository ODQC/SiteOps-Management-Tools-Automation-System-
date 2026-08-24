using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WebApiSAIH.Models.Entidades;

namespace WebApiSAIH.Services.DTO
{
    [Serializable]
    [DataContract]
    public class GoalDTO
    {
        [DataMember]
        private long pK_idGoal;

        [DataMember]
        private String code;

        [DataMember]
        private String name;

        [DataMember]
        private String description;

        [DataMember]
        private String year;

        [DataMember]
        private String status;

        public GoalDTO()
        {
            this.pK_idGoal = 0;
            this.code = "";
            this.name = "";
            this.description = "";
            this.year = "";
            this.status = "";

        }

        public GoalDTO(long pKIdGoal, string code, string name, string description, string year, string status)
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
